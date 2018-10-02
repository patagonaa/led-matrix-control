import { MatrixPreview } from "./matrixpreview";
import { MixerControl } from "./mixercontrol";

(() => {
    var matrixPreview = new MatrixPreview(<HTMLCanvasElement>document.getElementById("matrixPreview"), 32, 8, 16);
    matrixPreview.startListen();

    var mixercontrol = new MixerControl(<HTMLInputElement>document.getElementById("mixerControl"));
    mixercontrol.startListen();
})();

