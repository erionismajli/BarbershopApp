import { Component, input, output, signal, effect } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BookingService } from '../../services/booking.service';
import { BookingCreateDto } from '../../models/booking.model';

@Component({
  selector: 'app-booking-modal',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './booking-modal.component.html',
  styleUrl: './booking-modal.component.css'
})
export class BookingModalComponent {
  isOpen = input.required<boolean>();
  close = output<void>();
  bookingCreated = output<void>();

  isLoading = signal(false);
  services = ['Classic Cut', 'Beard Trim', 'Hot Shave'];
  barbers = ['Alex', 'Jordan', 'Sam', 'Taylor'];
  
  formData = signal<Partial<BookingCreateDto>>({
    date: '',
    time: '',
    clientName: '',
    clientPhone: '',
    serviceType: '',
    barberName: '',
    notes: ''
  });

  constructor(private bookingService: BookingService) {
    effect(() => {
      if (this.isOpen()) {
        const today = new Date().toISOString().split('T')[0];
        this.formData.set({
          date: today,
          time: '',
          clientName: '',
          clientPhone: '',
          serviceType: '',
          barberName: '',
          notes: ''
        } as Partial<BookingCreateDto>);
      }
    });
  }

  get minDate(): string {
    return new Date().toISOString().split('T')[0];
  }

  onSubmit(): void {
    if (this.isLoading()) return;

    const data = this.formData();
    if (!data.clientName || !data.clientPhone || !data.date || !data.time || !data.serviceType || !data.barberName) {
      return;
    }

    this.isLoading.set(true);
    this.bookingService.createBooking(data as BookingCreateDto).subscribe({
      next: () => {
        this.isLoading.set(false);
        this.bookingCreated.emit();
      },
      error: (error) => {
        this.isLoading.set(false);
        alert(error.message || 'Failed to create booking');
      }
    });
  }

  onCancel(): void {
    this.close.emit();
  }

  onBackdropClick(event: Event): void {
    if (event.target === event.currentTarget) {
      this.close.emit();
    }
  }
}
