using AutoMapper;
using CarRentalSystem.Application.DTOs.CarModels;
using CarRentalSystem.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Queries.CarModels.GetAllCarModels
{
    internal class GetAllCarModelsQueryHandler : IRequestHandler<GetAllCarModelsQuery, IEnumerable<CarModelDto>>
    {
        private readonly ICarModelRepository _carModelRepository;
        private readonly IMapper _mapper;

        public GetAllCarModelsQueryHandler(ICarModelRepository carModelRepository, IMapper mapper)
        {
            _carModelRepository = carModelRepository ?? throw new ArgumentNullException(nameof(carModelRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<IEnumerable<CarModelDto>> Handle(GetAllCarModelsQuery request, CancellationToken cancellationToken)
        {
            var carModels = await _carModelRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<CarModelDto>>(carModels);
        }
    }
}