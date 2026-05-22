using Hospital.Application.DTOs;
using Hospital.Domain.Enums;

namespace Hospital.Application.Alerts;

public class BedOccupancyAlertRule : IAlertRule
{
    public IEnumerable<AlertDto> Evaluate(HospitalAlertContext context)
    {
        var occupied = context.Beds.Count(x => x.Status == BedStatus.Occupied);
        if (context.Beds.Count > 0 && occupied >= Math.Ceiling(context.Beds.Count * 0.6))
        {
            yield return new AlertDto("warning", "Leitos quase lotados", $"{occupied} de {context.Beds.Count} leitos ocupados.", "leitos");
        }
    }
}
