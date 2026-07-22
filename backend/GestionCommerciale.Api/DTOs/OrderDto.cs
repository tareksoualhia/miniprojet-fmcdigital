using GestionCommerciale.Api.Models;

namespace GestionCommerciale.Api.DTOs;

public class OrderLineDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ProductNom { get; set; } = string.Empty;
    public int Quantite { get; set; }
    public decimal PrixUnitaire { get; set; }
    public decimal TotalLigne { get; set; }
}

public class OrderDto
{
    public int Id { get; set; }
    public string NumeroCommande { get; set; } = string.Empty;
    public int ClientId { get; set; }
    public string ClientNom { get; set; } = string.Empty;
    public DateTime DateCommande { get; set; }
    public string Statut { get; set; } = string.Empty;
    public decimal TotalHT { get; set; }
    public decimal TotalTTC { get; set; }
    public List<OrderLineDto> Lignes { get; set; } = new();
}

public class CreateOrderLineDto
{
    public int ProductId { get; set; }
    public int Quantite { get; set; }
}

public class CreateOrderDto
{
    public int ClientId { get; set; }
    public List<CreateOrderLineDto> Lignes { get; set; } = new();
}

public class UpdateOrderDto
{
    public int ClientId { get; set; }
    public List<CreateOrderLineDto> Lignes { get; set; } = new();
}
public class Tvadto
{
    public int orderId { get; set; }
    
    public string libelle { get; set; } = string.Empty;
    public int montant { get; set; }
    public int valeur { get; set; }  

    public bool etat { get; set; }  
    public int Totalcount { get; set; }  
}