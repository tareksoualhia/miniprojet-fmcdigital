namespace GestionCommerciale.Api.DTOs;

public class ClientDto
{
    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string PrenomOuRaisonSociale { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telephone { get; set; } = string.Empty;
    public string Adresse { get; set; } = string.Empty;
    public DateTime DateCreation { get; set; }
}

public class CreateClientDto
{
    public string Nom { get; set; } = string.Empty;
    public string PrenomOuRaisonSociale { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telephone { get; set; } = string.Empty;
    public string Adresse { get; set; } = string.Empty;
}

public class UpdateClientDto
{
    public string Nom { get; set; } = string.Empty;
    public string PrenomOuRaisonSociale { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telephone { get; set; } = string.Empty;
    public string Adresse { get; set; } = string.Empty;
}