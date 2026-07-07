using Microsoft.AspNetCore.Mvc;
using GestionCommerciale.Api.Services;
using GestionCommerciale.Api.DTOs;

namespace GestionCommerciale.Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly ProductService _service;

    public ProductsController(ProductService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductDto>>> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetById(int id)
    {
        var product = await _service.GetByIdAsync(id);
        if (product is null) return NotFound(new { message = $"Produit {id} introuvable." });
        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> Create(CreateProductDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Nom))
            return BadRequest(new { message = "Le nom du produit est obligatoire." });

        if (dto.PrixUnitaireHT < 0)
            return BadRequest(new { message = "Le prix unitaire ne peut pas être négatif." });

        if (dto.QuantiteEnStock < 0)
            return BadRequest(new { message = "La quantité en stock ne peut pas être négative." });

        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateProductDto dto)
    {
        var success = await _service.UpdateAsync(id, dto);
        if (!success) return NotFound(new { message = $"Produit {id} introuvable." });
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _service.DeleteAsync(id);
        if (!success) return NotFound(new { message = $"Produit {id} introuvable." });
        return NoContent();
    }
}