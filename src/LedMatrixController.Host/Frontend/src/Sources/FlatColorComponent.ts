import { AvailableSources } from "./AvailableSources";
import * as ko from "knockout";
import { Guid } from "../uuidv4";
import { ConfigViewModelBase } from "../ConfigViewModelBase";

class FlatColorModel {
    constructor(id: KnockoutObservable<Guid>) {
        this.id = id;
        this.color = ko.observable<string>();
    }

    public id: KnockoutObservable<Guid>;
    public color: KnockoutObservable<string>;
}

class FlatColorSourceViewModel extends ConfigViewModelBase<FlatColorModel> {
    constructor(params: { id: KnockoutObservable<Guid> }) {
        super(new FlatColorModel(params.id), "FlatColor");
    }
}

export function RegisterFlatColorComponent() {
    AvailableSources.register("flat-color");
    ko.components.register("flat-color", {
        viewModel: FlatColorSourceViewModel,
        template: `
        <div>
            Color: <span data-bind="text: model.color"></span><br/>
            <input type="color" data-bind="value: model.color" />
        </div>`
    });
}