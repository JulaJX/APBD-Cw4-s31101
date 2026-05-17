using Cwiczenia4Api.Data;
using Cwiczenia4Api.DTOs;
using Cwiczenia4Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Cwiczenia4Api.Services;

public class PcService : IPcService
{
    private readonly AppDbContext _context;

    public PcService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<PcListItemDto>> GetAllAsync()
    {
        return await _context.Pcs
            .Select(p => new PcListItemDto
            {
                Id = p.Id,
                Name = p.Name,
                Weight = p.Weight,
                Warranty = p.Warranty,
                CreatedAt = p.CreatedAt,
                Stock = p.Stock
            })
            .ToListAsync();
    }

    public async Task<PcDetailsDto?> GetByIdWithComponentsAsync(int id)
    {
        var pc = await _context.Pcs
            .Include(p => p.PcComponents)
                .ThenInclude(pc => pc.Component)
                    .ThenInclude(c => c.Manufacturer)
            .Include(p => p.PcComponents)
                .ThenInclude(pc => pc.Component)
                    .ThenInclude(c => c.Type)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (pc == null)
            return null;

        return new PcDetailsDto
        {
            Id = pc.Id,
            Name = pc.Name,
            Weight = pc.Weight,
            Warranty = pc.Warranty,
            CreatedAt = pc.CreatedAt,
            Stock = pc.Stock,
            Components = pc.PcComponents.Select(x => new PcComponentDto
            {
                Amount = x.Amount,
                Component = new ComponentDto
                {
                    Code = x.Component.Code,
                    Name = x.Component.Name,
                    Description = x.Component.Description,
                    Manufacturer = new ManufacturerDto
                    {
                        Id = x.Component.Manufacturer.Id,
                        Abbreviation = x.Component.Manufacturer.Abbreviation,
                        FullName = x.Component.Manufacturer.FullName,
                        FoundationDate = x.Component.Manufacturer.FoundationDate
                    },
                    Type = new ComponentTypeDto
                    {
                        Id = x.Component.Type.Id,
                        Abbreviation = x.Component.Type.Abbreviation,
                        Name = x.Component.Type.Name
                    }
                }
            }).ToList()
        };
    }

    public async Task<PcResponseDto> CreateAsync(CreatePcDto dto)
    {
        var pc = new Pc
        {
            Name = dto.Name,
            Weight = dto.Weight,
            Warranty = dto.Warranty,
            CreatedAt = dto.CreatedAt,
            Stock = dto.Stock
        };

        _context.Pcs.Add(pc);
        await _context.SaveChangesAsync();

        return new PcResponseDto
        {
            Id = pc.Id,
            Name = pc.Name,
            Weight = pc.Weight,
            Warranty = pc.Warranty,
            CreatedAt = pc.CreatedAt,
            Stock = pc.Stock
        };
    }

    public async Task<PcResponseDto?> UpdateAsync(int id, UpdatePcDto dto)
    {
        var pc = await _context.Pcs.FirstOrDefaultAsync(p => p.Id == id);

        if (pc == null)
            return null;

        pc.Name = dto.Name;
        pc.Weight = dto.Weight;
        pc.Warranty = dto.Warranty;
        pc.CreatedAt = dto.CreatedAt;
        pc.Stock = dto.Stock;

        await _context.SaveChangesAsync();

        return new PcResponseDto
        {
            Id = pc.Id,
            Name = pc.Name,
            Weight = pc.Weight,
            Warranty = pc.Warranty,
            CreatedAt = pc.CreatedAt,
            Stock = pc.Stock
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var pc = await _context.Pcs.FirstOrDefaultAsync(p => p.Id == id);

        if (pc == null)
            return false;

        _context.Pcs.Remove(pc);
        await _context.SaveChangesAsync();

        return true;
    }
}