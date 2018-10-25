import * as ko from "knockout";

export class AvailableSources {
    public static list: KnockoutObservableArray<string> = ko.observableArray();

    public static register(name: string){
        this.list.push(name);
    }
}