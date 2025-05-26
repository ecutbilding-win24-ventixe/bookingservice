using Business.Interfaces;
using Business.Models;
using Domain.Extensions;
using Microsoft.AspNetCore.Mvc;
using Presentation.Model;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookingController(IBookingService bookingService) : ControllerBase
{
    private readonly IBookingService _bookingService = bookingService;

    [HttpPost]
    public async Task<IActionResult> CreateBookingAsync([FromBody] CreateBookingRequestViewModel request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var model = request.MapTo<CreateBookingRequest>();

        var result = await _bookingService.CreateBookingAsync(model);
        return result.Succeeded
          ? Ok(result)
          : StatusCode(result.StatusCode, result);
    }

}
