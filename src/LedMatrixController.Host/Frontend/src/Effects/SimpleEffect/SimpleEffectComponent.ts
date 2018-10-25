import { AvailableEffects } from "../AvailableEffects";
import * as ko from "knockout";
import "knockout-mapping";
import { AvailableSources } from "../../Sources/AvailableSources";
import { DataService } from "../../DataService";
import { uuidv4, Guid } from "../../uuidv4";

class SimpleEffectViewModel {
    private endpointName = "simpleEffect";

    public Id: KnockoutObservable<Guid> = ko.observable<Guid>();
    public SourceName: KnockoutObservable<string> = ko.observable<string>();
    public SourceConfigId: KnockoutObservable<Guid> = ko.observable<Guid>();

    public availableSources = AvailableSources.list;
    constructor(params: { id: KnockoutObservable<Guid> }) {
        this.loadConfig(params.id()).then(() => {
            this.Id.subscribe(newId => this.loadConfig(newId));
        });
    }

    private async loadConfig(id: Guid | null): Promise<void> {
        if(id == null)
        {
            this.Id(uuidv4());
            return;
        }

        var model = await DataService.get(this.endpointName, id);
        ko.mapping.fromJS(model, this);
    }

    // TODO: call
    private async saveConfig(): Promise<void> {
        let model = ko.mapping.toJS(this);
        await DataService.put(this.endpointName, this.Id(), model);
    }
}

export function RegisterSimpleEffectComponent() {
    AvailableEffects.register('simple-effect');

    ko.components.register("simple-effect", {
        viewModel: SimpleEffectViewModel,
        template: `
        <div>Source: <select data-bind="value: SourceName, options: availableSources"></select></div>
        <div data-bind="if: SourceName">
            <div data-bind="component: {name: SourceName, params: {id: SourceConfigId}}"></div>
        </div>
        `
    });
}