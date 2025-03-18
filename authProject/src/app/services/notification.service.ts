import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class NotificationService {
  private hubConnection!: signalR.HubConnection;

  public startConnection(): void {
    const token: string | null = sessionStorage.getItem('token');
    const url = `${environment.webSocketUrl}=${token}`;

    if (
      this.hubConnection &&
      this.hubConnection.state === signalR.HubConnectionState.Connected
    ) {
      console.log('SignalR connection is already established.');
      return;
    }

    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(url, {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets,
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection
      .start()
      .then((): void => {
        console.log('SignalR Connected:', this.hubConnection.connectionId);

        this.listenForMessages((message: string) => {
          console.log('Notification received:', message);
        });
      })
      .catch((err: Error) => console.error('SignalR Connection Error:', err));

    this.handleDisconnects();

    this.handleDisconnects();
  }

  public stopConnection(): void {
    if (
      this.hubConnection &&
      this.hubConnection.state === signalR.HubConnectionState.Connected
    ) {
      this.hubConnection.stop().then(() => console.log('SignalR Disconnected'));
    }
  }

  public listenForMessages(callback: (message: string) => void): void {
    if (!this.hubConnection) return;

    this.hubConnection.on('ReceiveNotification', (message: string) => {
      console.log('New notification received:', message);
      callback(message);
    });

    this.hubConnection.on(
      'ReceivedMessage',
      (user: string, message: string) => {
        console.log(`User: ${user}, Message: ${message}`);
      }
    );
  }

  public sendMessage(user: string, message: string): void {
    if (
      !this.hubConnection ||
      this.hubConnection.state !== signalR.HubConnectionState.Connected
    ) {
      console.error('Cannot send message -> SignalR is not connected.');
      return;
    }

    this.hubConnection
      .invoke('SendMessage', user, message)
      .catch((err: Error) => console.error('Error sending message:', err));
  }

  private handleDisconnects(): void {
    if (!this.hubConnection) return;

    this.hubConnection.onclose(() => {
      console.log('Connection lost. Attempting to reconnect');
      setTimeout(() => this.startConnection(), 3000);
    });

    this.hubConnection.onreconnected(() => {
      console.log('Reconnected to SignalR hub.');
    });

    this.hubConnection.onreconnecting(() => {
      console.log('Reconnecting to SignalR hub');
    });
  }
}
