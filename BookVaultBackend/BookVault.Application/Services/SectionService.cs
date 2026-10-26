
using BookVault.Application.DTOs.Section.Requests;
using BookVault.Application.DTOs.Section.Responses;
using BookVault.Application.Exceptions;
using BookVault.Application.Interfaces.Repositories;
using BookVault.Application.Interfaces.Services;
using BookVault.Domain.Entities;

namespace BookVault.Application.Services;

public class SectionService : ISectionService
{
    private readonly ISectionRepository _sectionRepository;

    public SectionService(ISectionRepository sectionRepository)
    {
        _sectionRepository = sectionRepository;
    }

    public async Task<IEnumerable<SectionDto>> GetAllAsync()
    {
        var sections = await _sectionRepository.GetAllAsync();

        return sections
            .Select(MapToDto)
            .ToList();
    }

    public async Task<SectionDetailsDto> GetByIdAsync(int id)
    {
        var section = await _sectionRepository.GetByIdAsync(id);

        if (section == null)
        {
            throw new NotFoundException(
                "Section not found."
            );
        }

        return MapToDetailsDto(section);
    }

    public async Task<int> CreateAsync(CreateSectionDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Title))
        {
            throw new BadRequestException(
                "Section title is required."
            );
        }

        var existingSection =
            await _sectionRepository.GetByTitleAsync(dto.Title);

        if (existingSection != null)
        {
            throw new ConflictException(
                "Section with this title already exists."
            );
        }

        var now = DateTime.UtcNow;

        var section = new Section
        {
            Title = dto.Title,
            Sectionnumber = dto.Sectionnumber,
            Capacity = dto.Capacity,
            Sectionfloor = dto.Sectionfloor,
            Description = dto.Description,
			Roomid = dto.Roomid,

			Createdat = now,
            Updatedat = now
        };

        await _sectionRepository.AddAsync(section);
        await _sectionRepository.SaveAsync();

        return section.Id;
    }

    public async Task UpdateAsync(
        int id,
        UpdateSectionDto dto)
    {
        var section =
            await _sectionRepository.GetByIdAsync(id);

        if (section == null)
        {
            throw new NotFoundException(
                "Section not found."
            );
        }

        if (string.IsNullOrWhiteSpace(dto.Title))
        {
            throw new BadRequestException(
                "Section title is required."
            );
        }

        var existingSection =
            await _sectionRepository.GetByTitleAsync(dto.Title);

        if (existingSection != null &&
            existingSection.Id != id)
        {
            throw new ConflictException(
                "Section with this title already exists."
            );
        }

        section.Title = dto.Title;
        section.Sectionnumber = dto.Sectionnumber;
        section.Capacity = dto.Capacity;
        section.Sectionfloor = dto.Sectionfloor;
        section.Description = dto.Description;
        section.Employeeid = dto.Employeeid;
		section.Roomid = dto.Roomid;

		section.Createdat = section.Createdat.HasValue
            ? DateTime.SpecifyKind(
                section.Createdat.Value,
                DateTimeKind.Utc)
            : null;

      
        section.Updatedat = DateTime.UtcNow;

        _sectionRepository.Update(section);

        await _sectionRepository.SaveAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var section =
            await _sectionRepository.GetByIdAsync(id);

        if (section == null)
        {
            throw new NotFoundException(
                "Section not found."
            );
        }

        if (section.BookCases.Any())
        {
            throw new ConflictException(
                    "This section contains bookcases. Please move the bookcases to another section before deleting this section."
                );
        }

        _sectionRepository.Delete(section);

        await _sectionRepository.SaveAsync();
    }

    private static SectionDto MapToDto(Section section)
    {
        return new SectionDto
        {
            Id = section.Id,
            Title = section.Title,
            Sectionnumber = section.Sectionnumber,
            Capacity = section.Capacity,
            Sectionfloor = section.Sectionfloor,
            Description = section.Description,
            Createdat = section.Createdat,
            Updatedat = section.Updatedat,
            Employeeid = section.Employeeid,
			Roomid = section.Roomid

		};
    }

    private static SectionDetailsDto MapToDetailsDto(
        Section section)
    {
        return new SectionDetailsDto
        {
            Id = section.Id,
            Title = section.Title,
            Sectionnumber = section.Sectionnumber,
            Capacity = section.Capacity,
            Sectionfloor = section.Sectionfloor,
            Description = section.Description,
            Createdat = section.Createdat,
            Updatedat = section.Updatedat,
            Employeeid = section.Employeeid,
			Roomid = section.Roomid

		};
    }

}