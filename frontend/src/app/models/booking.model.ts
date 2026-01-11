export interface Booking {
  id?: string;
  date: string;
  time: string;
  clientName: string;
  clientPhone: string;
  serviceType: string;
  barberName: string;
  notes?: string;
  createdAt?: string | Date;
}

export interface BookingCreateDto {
  date: string;
  time: string;
  clientName: string;
  clientPhone: string;
  serviceType: string;
  barberName: string;
  notes?: string;
}

export interface BookingStats {
  totalCount: number;
  todayCount: number;
  weekCount: number;
}

export interface ApiResponse<T> {
  isSuccess: boolean;
  data?: T;
  message?: string;
  errors?: string[];
}
