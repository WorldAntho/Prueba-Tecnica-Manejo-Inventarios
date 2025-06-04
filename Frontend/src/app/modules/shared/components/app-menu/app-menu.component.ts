import { ChangeDetectionStrategy, Component } from '@angular/core';
import { MessageService, ConfirmationService } from 'primeng/api';
import { ImportsModule } from '../../../../core/models/Imports';

@Component({
  selector: 'app-menu',
  imports: [
    ImportsModule
  ],
  providers: [MessageService, ConfirmationService],
  templateUrl: './app-menu.component.html',
  styleUrl: './app-menu.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AppMenuComponent { }
