using Microsoft.EntityFrameworkCore;
using GestionCommerciale.Api.Data;
using GestionCommerciale.Api.Models;
using GestionCommerciale.Api.DTOs;

namespace GestionCommerciale.Api.Services;

public class ClientService
{
    private readonly AppDbContext _context;

    public ClientService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ClientDto>> GetAllAsync()
    {
        return await _context.Clients
            .Select(c => ToDto(c))
            .ToListAsync();
    }

    public async Task<ClientDto?> GetByIdAsync(int id)
    {
        var client = await _context.Clients.FindAsync(id);
        return client is null ? null : ToDto(client);
    }

    public async Task<ClientDto> CreateAsync(CreateClientDto dto)
    {
        var client = new Client
        {
            Nom = dto.Nom,
            PrenomOuRaisonSociale = dto.PrenomOuRaisonSociale,
            Email = dto.Email,
            Telephone = dto.Telephone,
            Adresse = dto.Adresse,
            DateCreation = DateTime.UtcNow
        };

        _context.Clients.Add(client);
        await _context.SaveChangesAsync();

        return ToDto(client);
    }

    public async Task<bool> UpdateAsync(int id, UpdateClientDto dto)
    {
        var client = await _context.Clients.FindAsync(id);
        if (client is null) return false;

        client.Nom = dto.Nom;
        client.PrenomOuRaisonSociale = dto.PrenomOuRaisonSociale;
        client.Email = dto.Email;
        client.Telephone = dto.Telephone;
        client.Adresse = dto.Adresse;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var client = await _context.Clients.FindAsync(id);
        if (client is null) return false;

        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();
        return true;
    }

    private static ClientDto ToDto(Client c) => new()
    {
        Id = c.Id,
        Nom = c.Nom,
        PrenomOuRaisonSociale = c.PrenomOuRaisonSociale,
        Email = c.Email,
        Telephone = c.Telephone,
        Adresse = c.Adresse,
        DateCreation = c.DateCreation
    };
}