using Cwiczenia4Api.DTOs;
using Cwiczenia4Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cwiczenia4Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PcsController : ControllerBase
{
    private readonly IPcService _pcService;

    public PcsController(IPcService pcService)
    {
        _pcService = pcService;
    }

    [HttpGet]
    public async Task<ActionResult<List<PcListItemDto>>> GetAll()
    {
        var result = await _pcService.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id}/components")]
    public async Task<ActionResult<PcDetailsDto>> GetByIdWithComponents(int id)
    {
        var result = await _pcService.GetByIdWithComponentsAsync(id);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<PcResponseDto>> Create([FromBody] CreatePcDto dto)
    {
        var result = await _pcService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetByIdWithComponents), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<PcResponseDto>> Update(int id, [FromBody] UpdatePcDto dto)
    {
        var result = await _pcService.UpdateAsync(id, dto);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _pcService.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}