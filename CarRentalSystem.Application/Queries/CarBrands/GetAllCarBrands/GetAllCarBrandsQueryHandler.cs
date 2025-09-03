using AutoMapper;
using CarRentalSystem.Application.DTOs.CarBrands;
using CarRentalSystem.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Queries.CarBrands.GetAllCarBrands
{
    internal class GetAllCarBrandsQueryHandler : IRequestHandler<GetAllCarBrandsQuery, IEnumerable<CarBrandDto>>
    {
        private readonly ICarBrandRepository _carBrandRepository;
        private readonly IMapper _mapper;

        public GetAllCarBrandsQueryHandler(ICarBrandRepository carBrandRepository, IMapper mapper)
        {
            _carBrandRepository = carBrandRepository ?? throw new ArgumentNullException(nameof(carBrandRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<CarBrandDto>> Handle(GetAllCarBrandsQuery request, CancellationToken cancellationToken)
        {
            var carBrands = await _carBrandRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<CarBrandDto>>(carBrands);
        }
    }
}
