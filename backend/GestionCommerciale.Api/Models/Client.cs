namespace GestionCommerciale.Api.Models;

public class Client
{
    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string PrenomOuRaisonSociale { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telephone { get; set; } = string.Empty;
    public string Adresse { get; set; } = string.Empty;
    public DateTime DateCreation { get; set; } = DateTime.UtcNow;

    public ICollection<Order> Orders { get; set; } = new List<Order>();
}