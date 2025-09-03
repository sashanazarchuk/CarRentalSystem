using CarRentalSystem.Application.Commands.Cars.CreateCar;
using CarRentalSystem.Application.Commands.Cars.DeleteCar;
using CarRentalSystem.Application.Commands.Cars.PatchCar;
using CarRentalSystem.Application.DTOs.Cars;
using CarRentalSystem.Application.Queries.Cars.GetAllCars;
using CarRentalSystem.Application.Queries.Cars.GetAvailableCars;
using CarRentalSystem.Application.Queries.Cars.GetCarById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalSystem.API.Controllers
{
    [Route("api/cars")]
    [ApiController]
    public class CarController : ControllerBase
    {

        private readonly IMediator _mediator;

        public CarController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }


        [HttpGet]
        public async Task<IActionResult> GetAllCars()
        {
            var car = await _mediator.Send(new GetAllCarsQuery());
            return Ok(car);
        }


        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableCars()
        {
            var car = await _mediator.Send(new GetAvailableCarsQuery());
            return Ok(car);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetCarById(int id)
        {
            var car = await _mediator.Send(new GetCarByIdQuery(id));
            return Ok(car);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateCar(CreateCarCommand command)
        {
            var car = await _mediator.Send(command);
            return Ok(car);
        }


        [Authorize(Roles = "Admin")]
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchCar(int id, [FromBody] JsonPatchDocument<PatchCarDto> patchDoc)
        {
            if (patchDoc == null) return BadRequest();

            var carDto = await _mediator.Send(new PatchCarCommand(id, patchDoc));
            return Ok(carDto);
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            await _mediator.Send(new DeleteCarCommand(id));
            return NoContent();
        }
    }
}