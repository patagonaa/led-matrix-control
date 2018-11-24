import * as ko from "knockout";
import { Guid, uuidv4 } from "../uuidv4";
import { ConfigViewModelBase } from "../ConfigViewModelBase";
import { SignalRDataServiceProvider } from "../SignalRDataService";

enum QueueElementType{
    Static,
    LinearMixer
}

class QueueElementConfig {
    id: KnockoutObservable<Guid>;
    sourceName: KnockoutObservable<string>;
    sourceConfigId: KnockoutObservable<Guid>;
    type: KnockoutObservable<QueueElementType>;
    duration: KnockoutObservable<number>;
}

class QueueConfigModel {
    constructor(id: KnockoutObservable<Guid>) {
        this.id = id;
        this.queueElements = ko.observableArray<QueueElementConfig>([]);
    }

    id: KnockoutObservable<Guid>;
    queueElements: KnockoutObservableArray<QueueElementConfig>;
}

class SourceListEntry {
    sourceName: string;
    configId: Guid;
}

export class QueueConfigViewModel extends ConfigViewModelBase<QueueConfigModel> {

    public availableSourceConfigs: KnockoutObservableArray<SourceListEntry> = ko.observableArray([]);
    public sourceToAdd = ko.observable<SourceListEntry>();

    constructor(params: {}) {
        super(new QueueConfigModel(ko.observable<Guid>("00000000-0000-0000-0000-000000000000")), "Queue");
        SignalRDataServiceProvider.get<{ id: string, sources: SourceListEntry[] }>("SourcesList")
            .then(async x => {
                this.availableSourceConfigs((await x.get("00000000-0000-0000-0000-000000000000")).sources);
                await x.observe("00000000-0000-0000-0000-000000000000")
                    .subscribe(config => this.availableSourceConfigs(config.sources));
            });
    }

    protected fromJS(model: any) {
        var mapping = {
            'queueElements': {
                key: function (data: QueueElementConfig) {
                    return ko.utils.unwrapObservable(data.id);
                }
            }
        };
        ko.mapping.fromJS(model, mapping, this.model);
    }

    public add() {
        var source = this.sourceToAdd();
        this.model.queueElements.push({
            id: ko.observable(uuidv4()),
            sourceName: ko.observable(source.sourceName),
            sourceConfigId: ko.observable(source.configId),
            type: ko.observable(QueueElementType.LinearMixer),
            duration: ko.observable(1000)
        });
    }

    public remove(item: QueueElementConfig) {
        this.model.queueElements.remove(item);
    }
}

export function RegisterPlaybackQueueComponent() {
    ko.components.register("playback-queue", {
        viewModel: QueueConfigViewModel,
        template: `
        <select data-bind="value: sourceToAdd, options: availableSourceConfigs, optionsText: 'configId'"></select>
        <button data-bind="click: add">Add</button>
        <ul data-bind="foreach: model.queueElements">
            <li>
                <label data-bind="text: sourceName"></label> <label data-bind="text: sourceConfigId"></label> Duration: <label data-bind="text: duration"></label>ms<br/>
                <button data-bind="click: $parent.remove.bind($parent, $data)">remove</button>
            </li>
        </ul>
        `
    });
}