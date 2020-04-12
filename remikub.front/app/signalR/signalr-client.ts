import { HubConnectionBuilder, HubConnection } from "@microsoft/signalr";

class SignalrClient {

    private _connection: HubConnection;

    public async init() {
        this._connection = new HubConnectionBuilder().withUrl("/notificationHub").build();
        await this._connection.start();
    }

    public register(methodName: string, newMethod: (...args: any[]) => void) {
        this._connection.on(methodName, newMethod);
    }

    public unregister(methodName: string) {
        this._connection.off(methodName);
    }
}

export const signalrClient = new SignalrClient();