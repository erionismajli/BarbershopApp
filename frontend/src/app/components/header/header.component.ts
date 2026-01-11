import { Component, output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ThemeService } from '../../services/theme.service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent {
  newBooking = output<void>();

  constructor(public themeService: ThemeService) {}

  onNewBooking(): void {
    this.newBooking.emit();
  }
}
