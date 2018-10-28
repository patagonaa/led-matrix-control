import { Subject } from "rxjs";

export class MainViewModel{
    constructor(){
    }

    public onSave: Subject<void> = new Subject<void>();

    public save(){
        this.onSave.next();
    }
}