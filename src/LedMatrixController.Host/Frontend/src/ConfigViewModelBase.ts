import * as ko from "knockout";
import "knockout-mapping";
import { SignalRDataServiceProvider, SignalRDataService } from "./SignalRDataService";
import { Guid } from "./uuidv4";
import { SubscriptionLike } from "rxjs";

export abstract class ConfigViewModelBase<T extends { id: KnockoutObservable<Guid> }> {
    public model: T;
    dataProvider: SignalRDataService<any>;
    private subscription: SubscriptionLike;

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
        this.modelUnsubscribe();
        var model = await this.dataProvider.get(id);
        this.fromJS(model);

        if (this.subscription != null) {
            this.subscription.unsubscribe();
        }

        var observable = this.dataProvider.observe(id);
        this.subscription = observable.subscribe(model => {
            this.modelUnsubscribe();
            this.fromJS(model);
            this.modelSubscribe(() => this.save());
        });

        this.modelSubscribe(() => this.save());
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
        let model = this.toJS();
        await this.dataProvider.save(model);
    }

    protected fromJS(model: any) {
        ko.mapping.fromJS(model, {}, this.model);
    }

    protected toJS(): any {
        let model = this.model;
        return ko.mapping.toJS<any>(model);
    }
}