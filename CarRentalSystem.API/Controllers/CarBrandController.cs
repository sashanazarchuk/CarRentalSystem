using CarRentalSystem.Application.Commands.CarBrands.CreateCarBrand;
using CarRentalSystem.Application.Commands.CarBrands.DeleteCarBrand;
using CarRentalSystem.Application.Commands.CarBrands.PatchCarBrand;
using CarRentalSystem.Application.DTOs.CarBrands;
using CarRentalSystem.Application.Queries.CarBrands.GetAllCarBrands;
using CarRentalSystem.Application.Queries.CarBrands.GetCarBrandsById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalSystem.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/carBrands")]
    [ApiController]
    public class CarBrandController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CarBrandController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCars()
        {
            var carBrand = await _mediator.Send(new GetAllCarBrandsQuery());
            return Ok(carBrand);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetCarById(int id)
        {
            var carBrand = await _mediator.Send(new GetCarBrandsByIdQuery(id));
            return Ok(carBrand);
        }


        [HttpPost]
        public async Task<IActionResult> CreateCar(CreateCarBrandCommand command)
        {
            var carBrand = await _mediator.Send(command);
            return Ok(carBrand);
        }

        
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchCar(int id, [FromBody] JsonPatchDocument<CreateCarBrandDto> patchDoc)
        {
            if (patchDoc == null) return BadRequest();

            var carBrandDto = await _mediator.Send(new PatchCarBrandCommand(id, patchDoc));
            return Ok(carBrandDto);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            await _mediator.Send(new DeleteCarBrandCommand(id));
            return NoContent();
        }
    }
}
