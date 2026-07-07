namespace GestionCommerciale.Api.Models;

public class Order
{
    public int Id { get; set; }
    public string NumeroCommande { get; set; } = string.Empty;
    public int ClientId { get; set; }
    public Client? Client { get; set; }
    public DateTime DateCommande { get; set; } = DateTime.UtcNow;
    public OrderStatus Statut { get; set; } = OrderStatus.Brouillon;
    public decimal TotalHT { get; set; }
    public decimal TotalTTC { get; set; }

    public ICollection<OrderLine> OrderLines { get; set; } = new List<OrderLine>();
}