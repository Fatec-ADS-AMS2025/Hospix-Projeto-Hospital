using Hospital.Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Controllers;

public abstract class HospitalControllerBase : ControllerBase
{
    protected ActionResult ApiError(Exception exception)
    {
        if (exception is DomainException)
        {
            return BadRequest(new { error = exception.Message });
        }

        return Problem(exception.Message);
    }
}
