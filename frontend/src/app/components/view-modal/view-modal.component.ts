import { Component, input, output, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BookingService } from '../../services/booking.service';
import { Booking } from '../../models/booking.model';

@Component({
  selector: 'app-view-modal',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './view-modal.component.html',
  styleUrl: './view-modal.component.css'
})
export class ViewModalComponent {
  isOpen = input.required<boolean>();
  booking = input<Booking | null>(null);
  close = output<void>();
  bookingDeleted = output<void>();

  isLoading = signal(false);

  constructor(private bookingService: BookingService) {}

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

  onCancel(): void {
    if (!this.booking() || this.isLoading()) return;
    
    this.isLoading.set(true);
    this.bookingService.deleteBooking(this.booking()!.id!).subscribe({
      next: () => {
        this.isLoading.set(false);
        this.bookingDeleted.emit();
      },
      error: (error) => {
        this.isLoading.set(false);
        alert(error.message || 'Failed to cancel appointment');
      }
    });
  }

  onBackdropClick(event: Event): void {
    if (event.target === event.currentTarget) {
      this.close.emit();
    }
  }
}
