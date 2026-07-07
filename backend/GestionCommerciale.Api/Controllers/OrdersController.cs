using Microsoft.AspNetCore.Mvc;
using GestionCommerciale.Api.Services;
using GestionCommerciale.Api.DTOs;

namespace GestionCommerciale.Api.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly OrderService _service;

    public OrdersController(OrderService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<OrderDto>>> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderDto>> GetById(int id)
    {
        var order = await _service.GetByIdAsync(id);
        if (order is null) return NotFound(new { message = $"Commande {id} introuvable." });
        return Ok(order);
    }

    [HttpPost]
    public async Task<ActionResult<OrderDto>> Create(CreateOrderDto dto)
    {
        var (success, error, order) = await _service.CreateAsync(dto);
        if (!success) return BadRequest(new { message = error });
        return CreatedAtAction(nameof(GetById), new { id = order!.Id }, order);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateOrderDto dto)
    {
        var (success, error) = await _service.UpdateAsync(id, dto);
        if (!success) return BadRequest(new { message = error });
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _service.DeleteAsync(id);
        if (!success) return NotFound(new { message = $"Commande {id} introuvable." });
        return NoContent();
    }

    [HttpPost("{id}/validate")]
    public async Task<IActionResult> Validate(int id)
    {
        var (success, error) = await _service.ValidateAsync(id);
        if (!success) return BadRequest(new { message = error });
        return Ok(new { message = "Commande validée avec succès." });
    }
}