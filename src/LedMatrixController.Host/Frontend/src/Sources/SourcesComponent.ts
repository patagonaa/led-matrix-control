import * as ko from "knockout";
import { Guid, uuidv4 } from "../uuidv4";
import { Observable, SubscriptionLike } from "rxjs";
import { RestDataService } from "../RestDataService";
import { AvailableSources } from "./AvailableSources";
import { SignalRDataServiceProvider } from "../SignalRDataService";

interface SourceListEntry {
    sourceName: KnockoutObservable<string>;
    configId: KnockoutObservable<Guid>;
}

export class SourcesConfigViewModel {
    public id = ko.observable<Guid>("00000000-0000-0000-0000-000000000000");
    public sources = ko.observableArray<SourceListEntry>();

    public availableSources = AvailableSources.list;
    public onSave: Observable<void>;
    public sourceToAdd = ko.observable<string>();

    private toDispose: (() => void)[] = [];
    private dataProvider: import("c:/Projects/led-matrix-control/src/LedMatrixController.Host/Frontend/src/SignalRDataService").SignalRDataService<any>;
    private subscription: SubscriptionLike;
    constructor(params: { onSave: Observable<void> }) {
        this.onSave = params.onSave;
        this.toDispose.push(this.onSave.subscribe(() => this.save()).unsubscribe);
        this.init();
    }

    private async init() {
        this.dataProvider = await SignalRDataServiceProvider.get<any>("SourcesList");
        await this.load(this.id());
        this.id.subscribe(newId => this.load(newId));
    }

    private async load(id: Guid | null): Promise<void> {
        if (id == null) {
            this.id(uuidv4());
            await this.save();
        } else {
            var model = await this.dataProvider.get(id);
            this.applyModel(model);
        }

        if (this.subscription != null) {
            this.subscription.unsubscribe();
        }

        var observable = this.dataProvider.observe(id);
        this.subscription = observable.subscribe(model => this.applyModel(model));
    }

    private applyModel(model: any) {
        var mapping = {
            'sources': {
                key: function (data: SourceListEntry) {
                    return ko.utils.unwrapObservable(data.configId);
                }
            }
        };

        ko.mapping.fromJS(model, mapping, this);
    }

    private async save(): Promise<void> {
        var model = {
            id: this.id(),
            sources: this.sources().map(s => ({configId: s.configId(), sourceName: s.sourceName()}))
        };
        await this.dataProvider.save(model);
    }

    public add() {
        this.sources.push({
            sourceName: ko.observable<string>(this.sourceToAdd()),
            configId: ko.observable<Guid>()
        });
    }
    //TODO: Remove

    public dispose() {
        this.toDispose.forEach(x => x());
    }
}

export function RegisterSourcesComponent() {
    ko.components.register("sources-config", {
        viewModel: SourcesConfigViewModel,
        template: `
        <select data-bind="value: sourceToAdd, options: availableSources"></select>
        <button data-bind="click: add">Add</button>
        <ul data-bind="foreach: sources">
            <li>
                <label data-bind="text: sourceName"></label> <label data-bind="text: configId"></label><br/>
                <div data-bind="component: {name: sourceName, params: {id: configId, onSave: $parent.onSave}}"></div>
                [Hier könnte ein Remove-Button sein]
            </li>
        </ul>
        `
    });
}