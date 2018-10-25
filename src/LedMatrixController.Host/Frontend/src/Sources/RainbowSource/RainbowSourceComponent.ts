import { AvailableSources } from "../AvailableSources";
import * as ko from "knockout";
import { Guid } from "../../uuidv4";

class RainbowSourceViewModel {
    constructor(params: { config: KnockoutObservable<Guid> }) {
    }
}

export function RegisterRainbowSourceComponent() {
    AvailableSources.register("rainbow");
    ko.components.register("rainbow", {
        viewModel: RainbowSourceViewModel,
        template: `<div></div>`
    });
}