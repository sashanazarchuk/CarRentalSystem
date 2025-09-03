using AutoMapper;
using CarRentalSystem.Application.DTOs.Bookings;
using CarRentalSystem.Application.Interfaces.Validator;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Queries.Bookings.GetBookingById
{
    internal class GetBookingByIdQueryHandler : IRequestHandler<GetBookingByIdQuery, BookingDto>
    {
        private readonly IBookingValidator _bookingValidator;
        private readonly IMapper _mapper;
        private readonly ILogger<GetBookingByIdQueryHandler> _logger;

        public GetBookingByIdQueryHandler( IMapper mapper, ILogger<GetBookingByIdQueryHandler> logger, IBookingValidator bookingValidator)
        {
            _bookingValidator = bookingValidator ?? throw new ArgumentNullException(nameof(bookingValidator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger;
        }

        public async Task<BookingDto> Handle(GetBookingByIdQuery request, CancellationToken cancellationToken)
        {
            var booking = await _bookingValidator.ValidateBookingAndUserExistsAsync(request.id, request.UserId, request.isAdmin, "view", cancellationToken);

            var bookingDto = _mapper.Map<BookingDto>(booking);
            
            _logger.LogInformation("Booking with id {BookingId} successfully retrieved", request.id);
            return bookingDto;
        }
    }
}
