import { AvailableSources } from "./AvailableSources";
import * as ko from "knockout";
import "knockout-mapping";
import { Guid } from "../uuidv4";
import { Observable } from "rxjs";

class RainbowSourceViewModel {
    constructor(params: { id: KnockoutObservable<Guid>, onSave: Observable<void> }) {
    }
}

export function RegisterRainbowSourceComponent() {
    AvailableSources.register("rainbow");
    ko.components.register("rainbow", {
        viewModel: RainbowSourceViewModel,
        template: `<div></div>`
    });
}