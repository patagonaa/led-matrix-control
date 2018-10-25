import * as ko from "knockout";
import { AvailableEffects } from "./AvailableEffects";
import { Guid } from "../uuidv4";

export class EffectConfigViewModel {
    public EffectName: KnockoutObservable<string>;
    public EffectConfigId: KnockoutObservable<Guid>;

    public availableEffects = AvailableEffects.list;

    constructor(params: { EffectName: KnockoutObservable<string>, EffectConfigId: KnockoutObservable<Guid> }) {
        this.EffectName = params.EffectName;
        this.EffectConfigId = params.EffectConfigId;
    }
}

export function RegisterEffectComponent() {
    ko.components.register("effect-config", {
        viewModel: EffectConfigViewModel,
        template: `
            <div>Effect: <select data-bind="value: EffectName, options: availableEffects"></select></div>
            <div data-bind="if: EffectName">
                <div data-bind="component: {name: EffectName, params: {id: EffectConfigId}}"></div>
            </div>
        `
    });
}