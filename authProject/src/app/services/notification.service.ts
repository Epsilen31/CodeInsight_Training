import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { environment } from '../../environments/environment';
@Injectable({
  providedIn: 'root',
})
export class NotificationService {
  private hubConnection!: signalR.HubConnection;

  startConnection(): void {
    const token: string | null = sessionStorage.getItem('token');
    const url = `${environment.webSocketUrl}=${token}`;

    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(url, {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets,
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection
      .start()
      .then(() => console.log('SignalR Connected'))
      .catch((err) => console.error('SignalR Error:', err));
  }

  listenForMessages(callback: (message: string) => void): void {
    this.hubConnection.on('ReceiveMessage', (message: string) => {
      console.log('New message received:', message);
      callback(message);
    });
  }
}
