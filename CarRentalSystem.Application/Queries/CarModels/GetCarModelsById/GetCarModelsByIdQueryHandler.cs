using AutoMapper;
using CarRentalSystem.Application.DTOs.CarModels;
using CarRentalSystem.Application.Interfaces.Validator;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Queries.CarModels.GetCarModelsById
{
    internal class GetCarModelsByIdQueryHandler : IRequestHandler<GetCarModelsByIdQuery, CarModelDto>
    {
        
        private readonly ICarModelValidator _carModelValidator;
        private readonly IMapper _mapper;

        public GetCarModelsByIdQueryHandler( IMapper mapper, ICarModelValidator carModelValidator)
        {
            _carModelValidator = carModelValidator ?? throw new ArgumentNullException(nameof(carModelValidator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<CarModelDto> Handle(GetCarModelsByIdQuery request, CancellationToken cancellationToken)
        {
           var carModel = await _carModelValidator.ValidateCarModelExistsAsync(request.CarModelId, cancellationToken);
              return _mapper.Map<CarModelDto>(carModel);
        }
    }
}