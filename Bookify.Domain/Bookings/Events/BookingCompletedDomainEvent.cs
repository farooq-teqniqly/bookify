﻿using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Bookings.Events;

public record BookingCompletedDomainEvent(Guid Id) : IDomainEvent;
