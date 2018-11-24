import * as ko from "knockout";
import { Guid } from "../uuidv4";

export interface SourceListEntry {
    sourceName: KnockoutObservable<string>;
    configId: KnockoutObservable<Guid>;
}

export class SourcesConfigModel {
    constructor(id: KnockoutObservable<Guid>){
        this.id = id;
        this.sources = ko.observableArray<SourceListEntry>();
    }

    id: KnockoutObservable<Guid>;
    sources: KnockoutObservableArray<SourceListEntry>;
}