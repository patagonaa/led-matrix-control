import { AvailableSources } from "./AvailableSources";
import * as ko from "knockout";
import { Guid, uuidv4 } from "../uuidv4";
import { SignalRDataServiceProvider, SignalRDataService } from "../SignalRDataService";
import { Observable, SubscriptionLike } from "rxjs";

class FlatColorSourceViewModel {
    public id: KnockoutObservable<Guid>;
    public color: KnockoutObservable<string> = ko.observable<string>();
    private dataProvider: SignalRDataService<any>;
    private subscription: SubscriptionLike;

    constructor(params: { id: KnockoutObservable<Guid>, onSave: Observable<void> }) {
        params.onSave.subscribe(() => this.save());
        this.id = params.id;
        this.init();
    }

    private async init() {
        this.dataProvider = await SignalRDataServiceProvider.get<any>("FlatColor");
        await this.load(this.id());
        this.id.subscribe(newId => this.load(newId));

        this.color.subscribe(newColor => this.save()); //TODO: automatically sync
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
        ko.mapping.fromJS(model, {}, this);
    }

    private async save(): Promise<void> {
        let model = {
            id: this.id(),
            color: this.color()
        };
        await this.dataProvider.save(model);
    }
}

export function RegisterFlatColorComponent() {
    AvailableSources.register("flat-color");
    ko.components.register("flat-color", {
        viewModel: FlatColorSourceViewModel,
        template: `
        <div>
            Color: <span data-bind="text: color"></span><br/>
            <input type="color" data-bind="value: color" />
        </div>`
    });
}