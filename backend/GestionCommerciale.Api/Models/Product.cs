namespace GestionCommerciale.Api.Models;

public class Product
{
    public int Id { get; set; }
    public string Reference { get; set; } = string.Empty;
    public string Nom { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal PrixUnitaireHT { get; set; }
    public int QuantiteEnStock { get; set; }
    public DateTime DateCreation { get; set; } = DateTime.UtcNow;

    public ICollection<OrderLine> OrderLines { get; set; } = new List<OrderLine>();
}