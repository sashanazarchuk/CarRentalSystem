using AutoMapper;
using CarRentalSystem.Application.DTOs.CarBrands;
using CarRentalSystem.Application.Interfaces.Validator;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Queries.CarBrands.GetCarBrandsById
{
    internal class GetCarBrandsByIdQueryHandler : IRequestHandler<GetCarBrandsByIdQuery, CarBrandDto>
    {
        private readonly ICarBrandValidator _carBrandValidator;
        private readonly IMapper _mapper;
        public GetCarBrandsByIdQueryHandler(ICarBrandValidator carBrandValidator, IMapper mapper)
        {
            _carBrandValidator = carBrandValidator ?? throw new ArgumentNullException(nameof(carBrandValidator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<CarBrandDto> Handle(GetCarBrandsByIdQuery request, CancellationToken cancellationToken)
        {
            var carBrand = await _carBrandValidator.ValidateBrandExistsAsync(request.CarBrandId, cancellationToken);
            return _mapper.Map<CarBrandDto>(carBrand);
        }
    }
}