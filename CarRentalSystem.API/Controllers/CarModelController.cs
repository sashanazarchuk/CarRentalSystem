using CarRentalSystem.Application.Commands.CarModels.CreateCarModel;
using CarRentalSystem.Application.Commands.CarModels.DeleteCarModel;
using CarRentalSystem.Application.Commands.CarModels.PatchCarModel;
using CarRentalSystem.Application.DTOs.CarModels;
using CarRentalSystem.Application.Queries.CarModels.GetAllCarModels;
using CarRentalSystem.Application.Queries.CarModels.GetCarModelsById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalSystem.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/carModels")]
    [ApiController]
    public class CarModelController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CarModelController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }


        [HttpGet]
        public async Task<IActionResult> GetAllCars()
        {
            var carModel = await _mediator.Send(new GetAllCarModelsQuery());
            return Ok(carModel);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetCarById(int id)
        {
            var carModel = await _mediator.Send(new GetCarModelsByIdQuery(id));
            return Ok(carModel);
        }


        [HttpPost]
        public async Task<IActionResult> CreateCar(CreateCarModelCommand command)
        {
            var carModel = await _mediator.Send(command);
            return Ok(carModel);
        }


        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchCar(int id, [FromBody] JsonPatchDocument<PatchCarModelDto> patchDoc)
        {
            if (patchDoc == null) return BadRequest();

            var carModelDto = await _mediator.Send(new PatchCarModelCommand(id, patchDoc));
            return Ok(carModelDto);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            await _mediator.Send(new DeleteCarModelCommand(id));
            return NoContent();
        }
    }
}