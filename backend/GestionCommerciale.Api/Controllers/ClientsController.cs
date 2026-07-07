using Microsoft.AspNetCore.Mvc;
using GestionCommerciale.Api.Services;
using GestionCommerciale.Api.DTOs;

namespace GestionCommerciale.Api.Controllers;

[ApiController]
[Route("api/clients")]
public class ClientsController : ControllerBase
{
    private readonly ClientService _service;

    public ClientsController(ClientService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<ClientDto>>> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ClientDto>> GetById(int id)
    {
        var client = await _service.GetByIdAsync(id);
        if (client is null) return NotFound(new { message = $"Client {id} introuvable." });
        return Ok(client);
    }

    [HttpPost]
    public async Task<ActionResult<ClientDto>> Create(CreateClientDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Nom))
            return BadRequest(new { message = "Le nom du client est obligatoire." });

        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateClientDto dto)
    {
        var success = await _service.UpdateAsync(id, dto);
        if (!success) return NotFound(new { message = $"Client {id} introuvable." });
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _service.DeleteAsync(id);
        if (!success) return NotFound(new { message = $"Client {id} introuvable." });
        return NoContent();
    }
}