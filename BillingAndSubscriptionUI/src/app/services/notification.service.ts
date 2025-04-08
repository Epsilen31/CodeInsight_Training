import { Injectable } from '@angular/core';
import { HttpTransportType, HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr';
import { environment } from '../../environments/environment';
import { from } from 'rxjs';
import { ToastService } from './toast.service';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private hubConnection!: HubConnection;

  constructor(private readonly _toastService: ToastService) {
    this.startConnection();
  }

  private startConnection(): void {
    const token: string | null = sessionStorage.getItem('token');
    const url = `${environment.webSocketUrl}=${token}`;

    if (this.hubConnection && this.hubConnection.state === HubConnectionState.Connected) {
      return;
    }

    this.hubConnection = new HubConnectionBuilder()
      .withUrl(url, {
        skipNegotiation: true,
        transport: HttpTransportType.WebSockets
      })
      .withAutomaticReconnect()
      .build();

    from(this.hubConnection.start()).subscribe({
      next: (): void => {
        this.listenForMessages((message: string): void => {});
        this.listenForProgressBar((message: number): void => {});
      },
      error: (error: Error): void => {
        this._toastService.showError(`SignalR Connection Error: ${error.message}`);
      }
    });

    this.handleDisconnects();
  }

  public stopConnection(): void {
    if (this.hubConnection && this.hubConnection.state === HubConnectionState.Connected) {
      this.hubConnection.stop();
    }
  }

  public listenForMessages(callback: (message: string) => void): void {
    if (!this.hubConnection) return;

    this.hubConnection.on('ReceiveNotification', (message: string): void => {
      callback(message);
    });

    this.hubConnection.on('ReceivedMessage', (user: string, message: string): void => {});
  }

  // method for listen progress bar notification
  public listenForProgressBar(callback: (message: number) => void): void {
    if (!this.hubConnection) return;
    this.hubConnection.off('ReceiveProgressBar');
    // get progress bar notification
    this.hubConnection.on('ReceiveProgress', (data: { progress: number }): void => {
      const progress = typeof data === 'number' ? data : (data.progress ?? data.progress ?? 0);
      callback(progress);
    });
  }

  public sendMessage(user: string, message: string): void {
    if (!this.hubConnection || this.hubConnection.state !== HubConnectionState.Connected) {
      this._toastService.showError('Cannot send message -> SignalR is not connected.');
      return;
    }

    this.hubConnection
      .invoke('SendMessage', user, message)
      .catch((error: Error): void => this._toastService.showError(`Error Getting Progress message: ${error}`));
  }

  private handleDisconnects(): void {
    if (!this.hubConnection) return;

    this.hubConnection.onclose((): void => {
      ('Connection lost. Attempting to reconnect');
      setTimeout((): void => this.startConnection(), 3000);
    });

    this.hubConnection.onreconnected((): void => {
      ('Reconnected to SignalR hub.');
    });

    this.hubConnection.onreconnecting((): void => {
      ('Reconnecting to SignalR hub');
    });
  }
}
