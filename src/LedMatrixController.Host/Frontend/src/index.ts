import { MatrixPreview } from "./matrixpreview";
import { SliderControl } from "./slidercontrol";
import * as ko from "knockout";
import { RegisterRainbowSourceComponent } from "./Sources/RainbowSourceComponent";
import { RegisterSourcesComponent } from "./Sources/SourcesComponent";
import { RegisterFlatColorComponent } from "./Sources/FlatColorComponent";
import { RegisterPlaybackQueueComponent } from "./Queue/QueueConfigViewModel";
import { MainViewModel } from "./MainViewModel";
import { SignalRDataServiceProvider } from "./SignalRDataService";
import { ServerConfig } from "./ServerConfig";

async function initApp(){
    var serverConfigProvider = await SignalRDataServiceProvider.get<ServerConfig>("ServerConfig");
    var serverConfig = await serverConfigProvider.get("00000000-0000-0000-0000-000000000000");

    var matrixPreview = new MatrixPreview(<HTMLCanvasElement>document.getElementById("matrixPreview"), serverConfig.width, serverConfig.height, 8);
    matrixPreview.startListen();

    ko.components.register("slider", {
        viewModel: SliderControl,
        template: '<input type="range" min="0" max="10" step="1" data-bind="value: value" />'
    });

    RegisterSourcesComponent();
    RegisterPlaybackQueueComponent();

    RegisterRainbowSourceComponent();
    RegisterFlatColorComponent();

    ko.applyBindings(new MainViewModel(), document.body);
}

initApp();