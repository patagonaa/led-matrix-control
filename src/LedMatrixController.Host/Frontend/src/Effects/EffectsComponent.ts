import * as ko from "knockout";
import { Guid } from "../uuidv4";

export class EffectsConfigViewModel {
    public effects: KnockoutObservableArray<{ EffectName: KnockoutObservable<string>, EffectConfigId: KnockoutObservable<Guid> }> = ko.observableArray();
    constructor() {
        this.effects.push({ EffectName: ko.observable<string>("simple-effect"), EffectConfigId: ko.observable<Guid>() });
    }

    //TODO: Save
    //TODO: Add
}

export function RegisterEffectsComponent() {
    ko.components.register("effects-config", {
        viewModel: EffectsConfigViewModel,
        template: `
        <ul>
            <li data-bind="foreach: effects">
                <effect-config params="{EffectName: EffectName, EffectConfigId: EffectConfigId}"></effect-config>
            </li>
        </ul>
        `
    });
}