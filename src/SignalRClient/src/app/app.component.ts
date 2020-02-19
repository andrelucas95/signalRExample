import { Component, OnInit } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'SignalRClient example';
  private SERVER_URL = 'http://localhost:5000/';
  private _connection: signalR.HubConnection;

  constructor(private readonly _http: HttpClient) {
    this._connection = null;
  }

  ngOnInit() {
    // this.connection.on('staticGroup', (message: string) => this.listener(message));
    this.connection.on('receiveMessage', (user, message) => {
      this.listener(user, message);
    });
    this.open();
  }

  async listener(user: string, message: string) {
    alert(`${user}: ${message}`);
  }

  get connection() {
    if (this._connection === null) {
      this._connection = new signalR.HubConnectionBuilder()
        .withUrl(`${this.SERVER_URL}ws/example`, {
          transport: signalR.HttpTransportType.WebSockets || signalR.HttpTransportType.LongPolling
        })
        .configureLogging(signalR.LogLevel.None)
        .withAutomaticReconnect()
        .build();
    }

    return this._connection;
  }

  async open() {
    if (this.connection.state === signalR.HubConnectionState.Disconnected) {
      await this.connection.start();
      await this.connection.invoke('subscribe');
    }
  }

  sendRequest() {
    return this._http.post<any>(`${this.SERVER_URL}example`, '')
      .subscribe(() => {});
  }

}
