import * as signalR from "@aspnet/signalr";

class MixerControl {
    private mixerControl: HTMLInputElement;
    private connection: signalR.HubConnection;

    constructor(mixerControl: HTMLInputElement) {
        this.mixerControl = mixerControl;
    }

    async startListen() {
        const connection = new signalR.HubConnectionBuilder()
            .withUrl("/mixercontrol")
            .configureLogging(signalR.LogLevel.Information)
            .build();
        this.connection = connection;

        connection.on("MixerValueUpdated", (value: number) => {
            this.mixerControl.value = (value * 10).toString();
        });

        await connection.start();
        connection.invoke("GetMixerValue");
        this.mixerControl.oninput = (e) => this.mixerValueChanged(e);
    }

    mixerValueChanged(event: Event){
        this.connection.invoke("UpdateMixerValue", parseInt((event.target as HTMLInputElement).value) / 10);
    }
}