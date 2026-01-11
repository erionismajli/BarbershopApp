import { Component, input, output, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Booking } from '../../models/booking.model';

@Component({
  selector: 'app-booking-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './booking-list.component.html',
  styleUrl: './booking-list.component.css'
})
export class BookingListComponent {
  bookings = input.required<Booking[]>();
  isLoading = input.required<boolean>();
  barberFilter = input.required<string>();
  dateFilter = input.required<string>();
  
  viewBooking = output<Booking>();

  filteredBookings = computed(() => {
    let filtered = this.bookings();

    if (this.barberFilter() !== 'all') {
      filtered = filtered.filter(b => b.barberName === this.barberFilter());
    }

    if (this.dateFilter() === 'upcoming') {
      filtered = filtered.filter(b => !this.isPast(b.date, b.time));
    } else if (this.dateFilter() === 'past') {
      filtered = filtered.filter(b => this.isPast(b.date, b.time));
    }

    return filtered.sort((a, b) => {
      const dateA = new Date(`${a.date}T${a.time}`);
      const dateB = new Date(`${b.date}T${b.time}`);
      return dateA.getTime() - dateB.getTime();
    });
  });

  onViewBooking(booking: Booking): void {
    this.viewBooking.emit(booking);
  }

  formatDate(dateStr: string): string {
    const date = new Date(dateStr);
    const options: Intl.DateTimeFormatOptions = { weekday: 'short', month: 'short', day: 'numeric', year: 'numeric' };
    return date.toLocaleDateString('en-US', options);
  }

  formatTime(timeStr: string): string {
    const [hours, minutes] = timeStr.split(':');
    const hour = parseInt(hours);
    const ampm = hour >= 12 ? 'PM' : 'AM';
    const displayHour = hour > 12 ? hour - 12 : hour === 0 ? 12 : hour;
    return `${displayHour}:${minutes} ${ampm}`;
  }

  isPast(dateStr: string, timeStr: string): boolean {
    const bookingDate = new Date(`${dateStr}T${timeStr}`);
    return bookingDate < new Date();
  }
}
