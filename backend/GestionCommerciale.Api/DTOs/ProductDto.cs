namespace GestionCommerciale.Api.DTOs;

public class ProductDto
{
    public int Id { get; set; }
    public string Reference { get; set; } = string.Empty;
    public string Nom { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal PrixUnitaireHT { get; set; }
    public int QuantiteEnStock { get; set; }
    public DateTime DateCreation { get; set; }
}

public class CreateProductDto
{
    public string Reference { get; set; } = string.Empty;
    public string Nom { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal PrixUnitaireHT { get; set; }
    public int QuantiteEnStock { get; set; }
}

public class UpdateProductDto
{
    public string Reference { get; set; } = string.Empty;
    public string Nom { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal PrixUnitaireHT { get; set; }
    public int QuantiteEnStock { get; set; }
}
