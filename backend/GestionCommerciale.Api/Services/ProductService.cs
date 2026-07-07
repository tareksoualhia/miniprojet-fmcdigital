using Microsoft.EntityFrameworkCore;
using GestionCommerciale.Api.Data;
using GestionCommerciale.Api.Models;
using GestionCommerciale.Api.DTOs;

namespace GestionCommerciale.Api.Services;

public class ProductService
{
    private readonly AppDbContext _context;

    public ProductService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ProductDto>> GetAllAsync()
    {
        return await _context.Products
            .Select(p => ToDto(p))
            .ToListAsync();
    }

    public async Task<ProductDto?> GetByIdAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        return product is null ? null : ToDto(product);
    }

    public async Task<ProductDto> CreateAsync(CreateProductDto dto)
    {
        var product = new Product
        {
            Reference = dto.Reference,
            Nom = dto.Nom,
            Description = dto.Description,
            PrixUnitaireHT = dto.PrixUnitaireHT,
            QuantiteEnStock = dto.QuantiteEnStock,
            DateCreation = DateTime.UtcNow
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return ToDto(product);
    }

    public async Task<bool> UpdateAsync(int id, UpdateProductDto dto)
    {
        var product = await _context.Products.FindAsync(id);
        if (product is null) return false;

        product.Reference = dto.Reference;
        product.Nom = dto.Nom;
        product.Description = dto.Description;
        product.PrixUnitaireHT = dto.PrixUnitaireHT;
        product.QuantiteEnStock = dto.QuantiteEnStock;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product is null) return false;

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return true;
    }

    private static ProductDto ToDto(Product p) => new()
    {
        Id = p.Id,
        Reference = p.Reference,
        Nom = p.Nom,
        Description = p.Description,
        PrixUnitaireHT = p.PrixUnitaireHT,
        QuantiteEnStock = p.QuantiteEnStock,
        DateCreation = p.DateCreation
    };
}