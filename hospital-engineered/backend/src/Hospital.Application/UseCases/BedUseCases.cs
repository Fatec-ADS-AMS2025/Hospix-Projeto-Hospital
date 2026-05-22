using Hospital.Application.DTOs;
using Hospital.Application.Interfaces;
using Hospital.Application.Mapping;

namespace Hospital.Application.UseCases;

public class BedUseCases
{
    private readonly IBedRepository beds;

    public BedUseCases(IBedRepository beds)
    {
        this.beds = beds;
    }

    public async Task<IReadOnlyList<BedDto>> ListAsync(CancellationToken cancellationToken)
    {
        var result = await beds.ListAsync(cancellationToken);
        return result.Select(HospitalMapping.ToDto).ToList();
    }
}
