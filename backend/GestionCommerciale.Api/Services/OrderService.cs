using Microsoft.EntityFrameworkCore;
using GestionCommerciale.Api.Data;
using GestionCommerciale.Api.Models;
using GestionCommerciale.Api.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.VisualBasic;
using System.Data.SqlTypes;

namespace GestionCommerciale.Api.Services;

public class OrderService
{
    private const decimal TvaRate =Tvadto;
    private readonly AppDbContext _context;

    public OrderService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<OrderDto>> GetAllAsync()
    {
        var orders = await _context.Orders
            .Include(o => o.Client)
            .Include(o => o.OrderLines).ThenInclude(ol => ol.Product)
            .ToListAsync();

        return orders.Select(ToDto).ToList();
    }

    public async Task<OrderDto?> GetByIdAsync(int id)
    {
        var order = await _context.Orders
            .Include(o => o.Client)
            .Include(o => o.OrderLines).ThenInclude(ol => ol.Product)
            .FirstOrDefaultAsync(o => o.Id == id);

        return order is null ? null : ToDto(order);
    }

    public async Task<(bool Success, string? Error, OrderDto? Order)> CreateAsync(CreateOrderDto dto)
    {
        var client = await _context.Clients.FindAsync(dto.ClientId);
        if (client is null)
            return (false, "Client introuvable. Impossible de créer une commande sans client valide.", null);

        if (dto.Lignes.Count == 0)
            return (false, "Une commande doit contenir au moins une ligne.", null);

        var order = new Order
        {
            ClientId = dto.ClientId,
            NumeroCommande = GenerateOrderNumber(),
            DateCommande = DateTime.UtcNow,
            Statut = OrderStatus.Brouillon
        };

        foreach (var ligneDto in dto.Lignes)
        {
            if (ligneDto.Quantite <= 0)
                return (false, "La quantité d'une ligne doit être supérieure à zéro.", null);

            var product = await _context.Products.FindAsync(ligneDto.ProductId);
            if (product is null)
                return (false, $"Produit {ligneDto.ProductId} introuvable.", null);

            if (ligneDto.Quantite > product.QuantiteEnStock)
                return (false, $"Stock insuffisant pour le produit '{product.Nom}' (disponible: {product.QuantiteEnStock}).", null);

            order.OrderLines.Add(new OrderLine
            {
                ProductId = product.Id,
                Quantite = ligneDto.Quantite,
                PrixUnitaire = product.PrixUnitaireHT
            });
        }

        RecalculateTotals(order);

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        var created = await GetByIdAsync(order.Id);
        return (true, null, created);
    }

    public async Task<(bool Success, string? Error)> UpdateAsync(int id, UpdateOrderDto dto)
    {
        var order = await _context.Orders
            .Include(o => o.OrderLines)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order is null) return (false, "Commande introuvable.");

        if (order.Statut == OrderStatus.Validee)
            return (false, "Impossible de modifier une commande déjà validée.");

        var client = await _context.Clients.FindAsync(dto.ClientId);
        if (client is null)
            return (false, "Client introuvable.");

        if (dto.Lignes.Count == 0)
            return (false, "Une commande doit contenir au moins une ligne.");

        _context.OrderLines.RemoveRange(order.OrderLines);
        order.OrderLines.Clear();
        order.ClientId = dto.ClientId;

        foreach (var ligneDto in dto.Lignes)
        {
            if (ligneDto.Quantite <= 0)
                return (false, "La quantité d'une ligne doit être supérieure à zéro.");

            var product = await _context.Products.FindAsync(ligneDto.ProductId);
            if (product is null)
                return (false, $"Produit {ligneDto.ProductId} introuvable.");

            if (ligneDto.Quantite > product.QuantiteEnStock)
                return (false, $"Stock insuffisant pour le produit '{product.Nom}'.");

            order.OrderLines.Add(new OrderLine
            {
                ProductId = product.Id,
                Quantite = ligneDto.Quantite,
                PrixUnitaire = product.PrixUnitaireHT
            });
        }

        RecalculateTotals(order);
        await _context.SaveChangesAsync();
        return (true, null);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order is null) return false;

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<(bool Success, string? Error)> ValidateAsync(int id)
    {
        var order = await _context.Orders
            .Include(o => o.OrderLines).ThenInclude(ol => ol.Product)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order is null) return (false, "Commande introuvable.");

        if (order.Statut == OrderStatus.Validee)
            return (false, "Cette commande est déjà validée.");

        if (order.Statut == OrderStatus.Annulee)
            return (false, "Impossible de valider une commande annulée.");

        // Re-check stock before committing, in case it changed since order creation
        foreach (var ligne in order.OrderLines)
        {
            if (ligne.Product is null) continue;
            if (ligne.Quantite > ligne.Product.QuantiteEnStock)
                return (false, $"Stock insuffisant pour le produit '{ligne.Product.Nom}' (disponible: {ligne.Product.QuantiteEnStock}).");
        }

        foreach (var ligne in order.OrderLines)
        {
            ligne.Product!.QuantiteEnStock -= ligne.Quantite;
        }

        order.Statut = OrderStatus.Validee;
        await _context.SaveChangesAsync();
        return (true, null);
    }

    private static void RecalculateTotals(Order order , Tvadto tva)
    {
        order.TotalHT = order.OrderLines.Sum(l => l.Quantite * l.PrixUnitaire);
        order.TotalTTC = Math.Round(order.TotalHT * (1 + tva), 2);
    }

    private static string GenerateOrderNumber()
    {
        return $"CMD-{DateTime.UtcNow:yyyyMMddHHmmssfff}";
    }

    private static OrderDto ToDto(Order o) => new()
    {
        Id = o.Id,
        NumeroCommande = o.NumeroCommande,
        ClientId = o.ClientId,
        ClientNom = o.Client is not null ? $"{o.Client.Nom} {o.Client.PrenomOuRaisonSociale}" : "",
        DateCommande = o.DateCommande,
        Statut = o.Statut.ToString(),
        TotalHT = o.TotalHT,
        TotalTTC = o.TotalTTC,
        Lignes = o.OrderLines.Select(l => new OrderLineDto
        {
            Id = l.Id,
            ProductId = l.ProductId,
            ProductNom = l.Product?.Nom ?? "",
            Quantite = l.Quantite,
            PrixUnitaire = l.PrixUnitaire,
            TotalLigne = l.Quantite * l.PrixUnitaire
        }).ToList()
    };





    public async Task<(bool Success, string? Error,Tvadto? tva )> Createtva(Tvadto tva , int id)
    {

         var order = await _context.Orders
            .Include(o => o.OrderLines).ThenInclude(ol => ol.Product)
            .FirstOrDefaultAsync(o => o.Id == id);
        

        var tvadto = new Tvadto
        {
            orderId = tva.orderId,
            libelle = tva.libelle,
            montant = tva.montant,
            valeur = tva.valeur,
            etat = tva.etat
        };



       RecalculateTotals(order);

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        var created = await GetByIdAsync(order.Id);
        return(true,null,tva) ;
    }
}
  




