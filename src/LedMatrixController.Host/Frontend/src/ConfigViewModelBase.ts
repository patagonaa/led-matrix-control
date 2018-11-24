import * as ko from "knockout";
import "knockout-mapping";
import { SignalRDataServiceProvider, SignalRDataService } from "./SignalRDataService";
import { Guid } from "./uuidv4";
import { SubscriptionLike } from "rxjs";

export abstract class ConfigViewModelBase<T extends { id: KnockoutObservable<Guid> }> {
    public model: T;
    dataProvider: SignalRDataService<any>;
    private modelUpdateSubscription: SubscriptionLike;

    constructor(model: T, endpoint: string) {
        this.model = model;
        this.init(endpoint);
    }

    private async init(endpoint: string) {
        this.dataProvider = await SignalRDataServiceProvider.get<any>(endpoint);
        await this.load(this.model.id());
        this.model.id.subscribe(newId => this.load(newId));
    }

    private async load(id: Guid): Promise<void> {
        var model = await this.dataProvider.get(id);
        this.setModel(model);

        if (this.modelUpdateSubscription != null) {
            this.modelUpdateSubscription.unsubscribe();
        }

        var observable = this.dataProvider.observe(id);
        this.modelUpdateSubscription = observable.subscribe(model => {
            this.setModel(model);
        });
    }

    protected modelSubscriptions: KnockoutSubscription[] = [];

    protected modelSubscribe(callback: () => void){
        for (const key in this.model) {
            if (this.model.hasOwnProperty(key)) {
                const observable = this.model[key];
                if(ko.isSubscribable(observable)){
                    this.modelSubscriptions.push(observable.subscribe(() => callback()));
                }
            }
        }
    }

    protected modelUnsubscribe(){
        var subscription: KnockoutSubscription;
        while(subscription = this.modelSubscriptions.pop()){
            subscription.dispose();
        }
    }

    private async save(): Promise<void> {
        let model = this.getModel();
        await this.dataProvider.save(model);
    }

    protected setModel(model: any) {
        this.modelUnsubscribe();
        ko.mapping.fromJS(model, {}, this.model);
        this.modelSubscribe(() => this.save());
    }

    protected getModel(): any {
        let model = this.model;
        return ko.mapping.toJS<any>(model);
    }
}