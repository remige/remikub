import { observable, action, computed } from "mobx";

export class LinkedListRow<TData> {

    public constructor(data: TData, next: LinkedListRow<TData> | undefined) {
        this._data = data;
        this._next = next;
    }

    @observable private _data: TData;
    @computed public get data() { return this._data; }

    @observable private _next: LinkedListRow<TData> | undefined;
    @computed public get next() { return this._next; }

    @action public changeNext(next: LinkedListRow<TData> | undefined) { this._next = next; }
}