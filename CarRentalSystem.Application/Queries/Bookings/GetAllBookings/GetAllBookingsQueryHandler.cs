using AutoMapper;
using CarRentalSystem.Application.DTOs.Bookings;
using CarRentalSystem.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Queries.Bookings.GetAllBookings
{
    internal class GetAllBookingsQueryHandler : IRequestHandler<GetAllBookingsQuery, IEnumerable<BookingDto>>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;
        public GetAllBookingsQueryHandler(IBookingRepository bookingRepository, IMapper mapper)
        {
            _bookingRepository = bookingRepository ?? throw new ArgumentNullException(nameof(bookingRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<IEnumerable<BookingDto>> Handle(GetAllBookingsQuery request, CancellationToken cancellationToken)
        {
            var bookings = await _bookingRepository.GetAllAsync(cancellationToken);

            if (!request.isAdmin)
                bookings = bookings.Where(b => b.UserId == request.UserId).ToList();

            return _mapper.Map<IEnumerable<BookingDto>>(bookings);
        }
    }
}