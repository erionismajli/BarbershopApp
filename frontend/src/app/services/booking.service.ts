import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { Booking, BookingCreateDto, BookingStats, ApiResponse } from '../models/booking.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class BookingService {
  private http = inject(HttpClient);
  private apiUrl = environment.apiUrl;

  getAllBookings(): Observable<Booking[]> {
    return this.http.get<ApiResponse<Booking[]>>(`${this.apiUrl}/api/bookings`)
      .pipe(
        map(response => response.data || []),
        catchError(this.handleError)
      );
  }

  getBookingById(id: string): Observable<Booking> {
    return this.http.get<ApiResponse<Booking>>(`${this.apiUrl}/api/bookings/${id}`)
      .pipe(
        map(response => {
          if (!response.data) {
            throw new Error(response.message || 'Booking not found');
          }
          return response.data;
        }),
        catchError(this.handleError)
      );
  }

  createBooking(booking: BookingCreateDto): Observable<Booking> {
    return this.http.post<ApiResponse<Booking>>(`${this.apiUrl}/api/bookings`, booking)
      .pipe(
        map(response => {
          if (!response.data) {
            throw new Error(response.message || 'Failed to create booking');
          }
          return response.data;
        }),
        catchError(this.handleError)
      );
  }

  updateBooking(id: string, booking: Partial<BookingCreateDto>): Observable<Booking> {
    return this.http.put<ApiResponse<Booking>>(`${this.apiUrl}/api/bookings/${id}`, booking)
      .pipe(
        map(response => {
          if (!response.data) {
            throw new Error(response.message || 'Failed to update booking');
          }
          return response.data;
        }),
        catchError(this.handleError)
      );
  }

  deleteBooking(id: string): Observable<void> {
    return this.http.delete<ApiResponse<void>>(`${this.apiUrl}/api/bookings/${id}`)
      .pipe(
        map(() => void 0),
        catchError(this.handleError)
      );
  }

  getStats(): Observable<BookingStats> {
    return this.http.get<ApiResponse<BookingStats>>(`${this.apiUrl}/api/bookings/stats`)
      .pipe(
        map(response => response.data || { totalCount: 0, todayCount: 0, weekCount: 0 }),
        catchError(this.handleError)
      );
  }

  private handleError = (error: HttpErrorResponse): Observable<never> => {
    let errorMessage = 'An unknown error occurred';
    
    if (error.error instanceof ErrorEvent) {
      errorMessage = `Error: ${error.error.message}`;
    } else {
      if (error.error?.message) {
        errorMessage = error.error.message;
      } else if (error.error?.errors && Array.isArray(error.error.errors)) {
        errorMessage = error.error.errors.join(', ');
      } else {
        errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
      }
    }
    
    return throwError(() => new Error(errorMessage));
  };
}
