class MixerControl {
    constructor(mixerControl) {
        this.mixerControl = mixerControl;
    }

    async startListen() {
        const connection = new signalR.HubConnectionBuilder()
            .withUrl("/mixercontrol")
            .configureLogging(signalR.LogLevel.Information)
            .build();
        this.connection = connection;

        connection.on("MixerValueUpdated", (value) => {
            this.mixerControl.value = value * 10;
        });

        await connection.start();
        connection.invoke("GetMixerValue");
        this.mixerControl.oninput = (e) => this.mixerValueChanged(e);
    }

    mixerValueChanged(event){
        this.connection.invoke("UpdateMixerValue", event.target.value / 10);
    }
}