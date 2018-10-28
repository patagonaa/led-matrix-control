import * as signalR from "@aspnet/signalr";
import { BehaviorSubject, Observable, Subject } from "rxjs";
import { Guid } from "./uuidv4";

export class SignalRDataService<T extends { id: string }> {
    private endpoint: string;
    private connection: signalR.HubConnection;
    public initPromise: Promise<void>;
    constructor(endpoint: string) {
        this.endpoint = endpoint;
        this.initPromise = this.startListen();
    }

    private listeners: { [id: string]: Subject<T> } = {};

    private async startListen() {
        const connection = new signalR.HubConnectionBuilder()
            .withUrl(`/${this.endpoint}`)
            .configureLogging(signalR.LogLevel.Information)
            .build();

        this.connection = connection;

        connection.on("Update", (model: T) => {
            let id = model.id;
            var listener = this.listeners[id];
            if (listener != null) {
                listener.next(model);
            }
        });

        await connection.start();
    }

    public async save(model: T): Promise<void> {
        await this.connection.invoke<void>("Save", model);
    }

    public async get(id: Guid): Promise<T> {
        return await this.connection.invoke<T>("Get", id);
    }

    public observe(id: Guid): Observable<T> {
        var subject = this.listeners[id];
        if (subject != null) {
            return subject;
        }
        var subject = new Subject<T>();
        this.listeners[id] = subject;
        return subject;
    }
}

export class SignalRDataServiceProvider {
    private static knownServices: { [endpoint: string]: SignalRDataService<any> } = {};

    public static async get<T extends { id: Guid }>(endpoint: string): Promise<SignalRDataService<T>> {
        var endpointService: SignalRDataService<T> = SignalRDataServiceProvider.knownServices[endpoint];
        if (endpointService == null) {
            endpointService = new SignalRDataService<T>(endpoint);
            SignalRDataServiceProvider.knownServices[endpoint] = endpointService;
        }
        await endpointService.initPromise;
        return endpointService;
    }
}