import * as ko from "knockout";
import { Guid, uuidv4 } from "../uuidv4";
import { AvailableSources } from "./AvailableSources";
import { ConfigViewModelBase } from "../ConfigViewModelBase";

interface SourceListEntry {
    sourceName: KnockoutObservable<string>;
    configId: KnockoutObservable<Guid>;
}

class SourcesConfigModel {
    constructor(id: KnockoutObservable<Guid>){
        this.id = id;
        this.sources = ko.observableArray<SourceListEntry>();
    }

    id: KnockoutObservable<Guid>;
    sources: KnockoutObservableArray<SourceListEntry>;
}

export class SourcesConfigViewModel extends ConfigViewModelBase<SourcesConfigModel> {

    public availableSources = AvailableSources.list;
    public sourceToAdd = ko.observable<string>();

    constructor(params: {}) {
        super(new SourcesConfigModel(ko.observable<Guid>("00000000-0000-0000-0000-000000000000")), "SourcesList");
    }

    protected fromJS(model: any) {
        var mapping = {
            'sources': {
                key: function (data: SourceListEntry) {
                    return ko.utils.unwrapObservable(data.configId);
                }
            }
        };
        ko.mapping.fromJS(model, mapping, this.model);
    }

    public add() {
        this.model.sources.push({
            sourceName: ko.observable(this.sourceToAdd()),
            configId: ko.observable(uuidv4())
        });
    }

    public remove(item: SourceListEntry) {
        this.model.sources.remove(item);
    }
}

export function RegisterSourcesComponent() {
    ko.components.register("sources-config", {
        viewModel: SourcesConfigViewModel,
        template: `
        <select data-bind="value: sourceToAdd, options: availableSources"></select>
        <button data-bind="click: add">Add</button>
        <ul data-bind="foreach: model.sources">
            <li>
                <label data-bind="text: sourceName"></label> <label data-bind="text: configId"></label><br/>
                <div data-bind="component: {name: sourceName, params: {id: configId}}"></div>
                <button data-bind="click: $parent.remove.bind($parent, $data)">remove</button>
            </li>
        </ul>
        `
    });
}