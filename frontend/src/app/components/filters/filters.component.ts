import { Component, input, output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-filters',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './filters.component.html',
  styleUrl: './filters.component.css'
})
export class FiltersComponent {
  selectedBarber = input<string>('all');
  selectedDateFilter = input<string>('all');
  
  filterChange = output<{ barber: string; dateFilter: string }>();

  barbers = ['Alex', 'Jordan', 'Sam', 'Taylor'];

  onBarberChange(barber: string): void {
    this.filterChange.emit({
      barber,
      dateFilter: this.selectedDateFilter()
    });
  }

  onDateFilterChange(dateFilter: string): void {
    this.filterChange.emit({
      barber: this.selectedBarber(),
      dateFilter
    });
  }
}
