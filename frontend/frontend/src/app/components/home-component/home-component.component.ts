import { Component } from '@angular/core';

@Component({
  selector: 'app-home-component',
  imports: [],
  templateUrl: './home-component.component.html',
  styleUrl: './home-component.component.css'
})
export class HomeComponentComponent {
  title: string = 'ברוכים הבאים לשדה התעופה';
  subtitle: string = 'כל מה שצריך לדעת על טיסות';
}
