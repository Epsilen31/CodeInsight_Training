import { Injectable } from '@angular/core';
import {
  HttpTransportType,
  HubConnection,
  HubConnectionBuilder,
  HubConnectionState,
} from '@microsoft/signalr';
import { environment } from '../../environments/environment';
import { from } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class NotificationService {
  private hubConnection!: HubConnection;

  constructor() {
    this.startConnection();
  }

  private startConnection(): void {
    const token: string | null = sessionStorage.getItem('token');
    const url = `${environment.webSocketUrl}=${token}`;

    if (
      this.hubConnection &&
      this.hubConnection.state === HubConnectionState.Connected
    ) {
      console.log('SignalR connection is already established.');
      return;
    }

    this.hubConnection = new HubConnectionBuilder()
      .withUrl(url, {
        skipNegotiation: true,
        transport: HttpTransportType.WebSockets,
      })
      .withAutomaticReconnect()
      .build();

    from(this.hubConnection.start()).subscribe({
      next: (): void => {
        console.log('SignalR Connected:', this.hubConnection.connectionId);
        this.listenForMessages((message: string): void => {
          console.log('Notification received:', message);
        });
      },
      error: (error: Error): void => {
        console.error('SignalR Connection Error:', error);
      },
    });

    this.handleDisconnects();
  }

  public stopConnection(): void {
    if (
      this.hubConnection &&
      this.hubConnection.state === HubConnectionState.Connected
    ) {
      this.hubConnection
        .stop()
        .then((): void => console.log('SignalR Disconnected'));
    }
  }

  public listenForMessages(callback: (message: string) => void): void {
    if (!this.hubConnection) return;

    this.hubConnection.on('ReceiveNotification', (message: string): void => {
      console.log('New notification received:', message);
      callback(message);
    });

    this.hubConnection.on(
      'ReceivedMessage',
      (user: string, message: string): void => {
        console.log(`User: ${user}, Message: ${message}`);
      }
    );
  }

  public sendMessage(user: string, message: string): void {
    if (
      !this.hubConnection ||
      this.hubConnection.state !== HubConnectionState.Connected
    ) {
      console.error('Cannot send message -> SignalR is not connected.');
      return;
    }

    this.hubConnection
      .invoke('SendMessage', user, message)
      .catch((error: Error): void =>
        console.error('Error sending message:', error)
      );
  }

  private handleDisconnects(): void {
    if (!this.hubConnection) return;

    this.hubConnection.onclose((): void => {
      console.log('Connection lost. Attempting to reconnect');
      setTimeout((): void => this.startConnection(), 3000);
    });

    this.hubConnection.onreconnected((): void => {
      console.log('Reconnected to SignalR hub.');
    });

    this.hubConnection.onreconnecting((): void => {
      console.log('Reconnecting to SignalR hub');
    });
  }
}
