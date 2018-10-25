import * as ko from "knockout";

export class AvailableEffects {
    public static list: KnockoutObservableArray<string> = ko.observableArray<string>();

    public static register(name: string){
        this.list.push(name);
    }
}