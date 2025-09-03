using AutoMapper;
using CarRentalSystem.Application.DTOs.Car;
using CarRentalSystem.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Queries.Cars.GetAvailableCars
{
    internal class GetAvailableCarsQueryHandler: IRequestHandler<GetAvailableCarsQuery, List<CarDto>>
    {
        private readonly ICarRepository _carRepository;
        private readonly IMapper _mapper;

        public GetAvailableCarsQueryHandler(ICarRepository carRepository, IMapper mapper)
        {
            _carRepository = carRepository ?? throw new ArgumentNullException(nameof(carRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<CarDto>> Handle(GetAvailableCarsQuery request, CancellationToken cancellationToken)
        {
            var cars = await _carRepository.GetAvailableCarsAsync(cancellationToken);
            return _mapper.Map<List<CarDto>>(cars);
        }
    }
}
