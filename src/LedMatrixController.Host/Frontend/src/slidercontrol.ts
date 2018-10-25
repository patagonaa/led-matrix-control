import * as signalR from "@aspnet/signalr";
import * as ko from "knockout";

export class SliderControl {
    private connection: signalR.HubConnection;
    private controlName: string;
    private value: KnockoutObservable<number>;

    constructor(params: { control: string }) {
        this.controlName = params.control;
        this.value = ko.observable<number>();

        this.startListen()
            .then(async () => {
                this.value(await this.connection.invoke<number>("GetValue") * 10);
            })
            .then(() => {
                this.value.subscribe(value => this.connection.invoke("UpdateValue", value / 10));
            });
    }

    async startListen() {
        const connection = new signalR.HubConnectionBuilder()
            .withUrl(`slider/${this.controlName}`)
            .configureLogging(signalR.LogLevel.Information)
            .build();
        this.connection = connection;

        connection.on("ValueUpdated", (value: number) => {
            this.value(value * 10);
        });

        await connection.start();
    }
    
    public dispose() {
        this.connection.stop();
    }
}