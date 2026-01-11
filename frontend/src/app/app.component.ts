import { Component, OnInit, signal, effect } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BookingService } from './services/booking.service';
import { ThemeService } from './services/theme.service';
import { Booking, BookingCreateDto } from './models/booking.model';
import { HeaderComponent } from './components/header/header.component';
import { StatsComponent } from './components/stats/stats.component';
import { FiltersComponent } from './components/filters/filters.component';
import { BookingListComponent } from './components/booking-list/booking-list.component';
import { BookingModalComponent } from './components/booking-modal/booking-modal.component';
import { ViewModalComponent } from './components/view-modal/view-modal.component';
import { ToastComponent } from './components/toast/toast.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    HeaderComponent,
    StatsComponent,
    FiltersComponent,
    BookingListComponent,
    BookingModalComponent,
    ViewModalComponent,
    ToastComponent
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  bookings = signal<Booking[]>([]);
  isLoading = signal(false);
  selectedBarberFilter = signal<string>('all');
  selectedDateFilter = signal<string>('all');
  showBookingModal = signal(false);
  showViewModal = signal(false);
  selectedBooking = signal<Booking | null>(null);
  toastMessage = signal<string>('');
  toastType = signal<'success' | 'error'>('success');
  showToast = signal(false);

  constructor(
    private bookingService: BookingService,
    public themeService: ThemeService
  ) {
    effect(() => {
      this.loadBookings();
    });
  }

  ngOnInit(): void {
    this.loadBookings();
  }

  loadBookings(): void {
    this.isLoading.set(true);
    this.bookingService.getAllBookings().subscribe({
      next: (bookings) => {
        this.bookings.set(bookings);
        this.isLoading.set(false);
      },
      error: (error) => {
        this.showToastMessage(error.message || 'Failed to load bookings', 'error');
        this.isLoading.set(false);
      }
    });
  }

  onNewBooking(): void {
    this.showBookingModal.set(true);
  }

  onBookingCreated(): void {
    this.showBookingModal.set(false);
    this.loadBookings();
    this.showToastMessage('Appointment booked successfully!', 'success');
  }

  onBookingDeleted(): void {
    this.showViewModal.set(false);
    this.loadBookings();
    this.showToastMessage('Appointment cancelled successfully!', 'success');
  }

  onViewBooking(booking: Booking): void {
    this.selectedBooking.set(booking);
    this.showViewModal.set(true);
  }

  onFilterChange(barber: string, dateFilter: string): void {
    this.selectedBarberFilter.set(barber);
    this.selectedDateFilter.set(dateFilter);
  }

  showToastMessage(message: string, type: 'success' | 'error'): void {
    this.toastMessage.set(message);
    this.toastType.set(type);
    this.showToast.set(true);
    setTimeout(() => {
      this.showToast.set(false);
    }, 3000);
  }
}
