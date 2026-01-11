import { Component, input, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Booking } from '../../models/booking.model';

@Component({
  selector: 'app-stats',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './stats.component.html',
  styleUrl: './stats.component.css'
})
export class StatsComponent {
  bookings = input.required<Booking[]>();

  totalCount = computed(() => this.bookings().length);

  todayCount = computed(() => {
    const today = new Date().toISOString().split('T')[0];
    return this.bookings().filter(b => b.date === today).length;
  });

  weekCount = computed(() => {
    const now = new Date();
    const weekStart = new Date(now);
    weekStart.setDate(now.getDate() - now.getDay());
    weekStart.setHours(0, 0, 0, 0);
    const weekEnd = new Date(weekStart);
    weekEnd.setDate(weekStart.getDate() + 7);

    return this.bookings().filter(b => {
      const bookingDate = new Date(b.date);
      return bookingDate >= weekStart && bookingDate < weekEnd;
    }).length;
  });
}
