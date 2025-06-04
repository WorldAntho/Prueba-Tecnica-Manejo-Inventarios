import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AppMenuComponent } from './modules/shared/components/app-menu/app-menu.component';
import { ImportsModule } from './core/models/Imports';

@Component({
  selector: 'app-root',
  standalone:true,
  imports: [ImportsModule, RouterOutlet,AppMenuComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'Sistema de Inventario';
}
