using Cwiczenia4Api.DTOs;

namespace Cwiczenia4Api.Services;

public interface IPcService
{
    Task<List<PcListItemDto>> GetAllAsync();
    Task<PcDetailsDto?> GetByIdWithComponentsAsync(int id);
    Task<PcResponseDto> CreateAsync(CreatePcDto dto);
    Task<PcResponseDto?> UpdateAsync(int id, UpdatePcDto dto);
    Task<bool> DeleteAsync(int id);
}