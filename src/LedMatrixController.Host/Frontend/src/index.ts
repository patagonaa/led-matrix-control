import { MatrixPreview } from "./matrixpreview";
import { SliderControl } from "./slidercontrol";
import * as ko from "knockout";
import { RegisterSimpleEffectComponent } from "./Effects/SimpleEffect/SimpleEffectComponent";
import { RegisterRainbowSourceComponent } from "./Sources/RainbowSource/RainbowSourceComponent";
import { RegisterEffectsComponent } from "./Effects/EffectsComponent";
import { RegisterEffectComponent } from "./Effects/EffectComponent";

(() => {
    var matrixPreview = new MatrixPreview(<HTMLCanvasElement>document.getElementById("matrixPreview"), 32, 8, 16);
    matrixPreview.startListen();

    ko.components.register("slider", {
        viewModel: SliderControl,
        template: '<input type="range" min="0" max="10" step="1" data-bind="value: value" />'
    });

    RegisterRainbowSourceComponent();
    RegisterSimpleEffectComponent();
    RegisterEffectComponent();
    RegisterEffectsComponent();

    ko.applyBindings();
})();

