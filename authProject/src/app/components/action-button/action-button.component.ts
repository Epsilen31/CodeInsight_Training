import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ICellRendererAngularComp } from 'ag-grid-angular';
import { ICellRendererParams } from 'ag-grid-community';

@Component({
  selector: 'app-action-buttons',
  templateUrl: './action-button.component.html',
  styleUrl: './action-button.component.scss',
  standalone: false,
})
export class ActionButtonComponent implements ICellRendererAngularComp {
  private params!: ICellRendererParams;

  constructor(private readonly _router: Router) {}

  agInit(params: ICellRendererParams): void {
    this.params = params;
  }

  refresh(params: ICellRendererParams): boolean {
    return false;
  }

  onView(): void {
    console.log('View button clicked for user:', this.params.data);
    this._router.navigate(['user/get-user-by-id', this.params.data.id]);
  }

  onUpdate(): void {
    console.log('Update button clicked for user:', this.params.data);
  }

  onDelete(): void {
    console.log('Delete button clicked for user:', this.params.data);
  }
}
