import { MatrixPreview } from "./matrixpreview";
import { SliderControl } from "./slidercontrol";
import * as ko from "knockout";
import { RegisterRainbowSourceComponent } from "./Sources/RainbowSourceComponent";
import { RegisterSourcesComponent } from "./Sources/SourcesComponent";
import { RegisterFlatColorComponent } from "./Sources/FlatColorComponent";
import { MainViewModel } from "./MainViewModel";

(() => {
    var matrixPreview = new MatrixPreview(<HTMLCanvasElement>document.getElementById("matrixPreview"), 32, 8, 16);
    matrixPreview.startListen();

    ko.components.register("slider", {
        viewModel: SliderControl,
        template: '<input type="range" min="0" max="10" step="1" data-bind="value: value" />'
    });

    RegisterSourcesComponent();

    RegisterRainbowSourceComponent();
    RegisterFlatColorComponent();

    ko.applyBindings(new MainViewModel(), document.body);
})();

