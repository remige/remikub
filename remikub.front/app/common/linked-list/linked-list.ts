import { LinkedListRow } from "./linked-list-node";
import { observable, computed } from "mobx";

export class LinkedList<TData> {

    public constructor(list: TData[] = []) {
        list.forEach(row => this.addLast(row));
    }

    @observable private _count = 0;
    @computed public get count() { return this._count; }

    @observable private _first: LinkedListRow<TData> | undefined;
    @computed public get first() { return this._first; }

    @observable private _last: LinkedListRow<TData> | undefined;
    @computed public get last() { return this._last; }

    public map<U>(callbackfn: (value: TData, index: number) => U) {
        const result: U[] = [];
        let idx = 0;
        let currentRow = this._first;
        while (currentRow) {
            result.push(callbackfn(currentRow.data, idx));
            idx++;
            currentRow = currentRow.next;
        }
        return result;
    }

    public addLast(row: TData) {
        const newNode = new LinkedListRow<TData>(row, undefined);
        if (this._last) {
            this._last.changeNext(newNode);
        } else {
            this._first = newNode;
        }
        this._last = newNode;
        this._count++;
    }

    public addAt(row: TData, position: number) {
        if (position > this.count) {
            throw new Error(`${position} is an invalid position. List has only ${this.count} elements`);
        }

        if (position === this.count) {
            return this.addLast(row);
        }
        if (position === 0) {
            // Add element at the beginning of the list
            const newNode = new LinkedListRow<TData>(row, this.first);
            newNode.changeNext(this._first!);
            this._first = newNode;
        } else {
            const node = this.elementAt(position - 1);
            const newNode = new LinkedListRow<TData>(row, node.next);
            node.changeNext(newNode);
        }

        this._count++;
    }

    public remove(position: number) {
        if (position >= this.count) {
            throw new Error(`${position} is an invalid position. List has only ${this.count} elements`);
        }

        let data;

        if (position === 0) {
            data = this._first!.data;
            this._first = this._first?.next;
        } else if (position === this.count - 1) {
            data = this._last!.data;
            const previous = this.elementAt(position - 1);
            previous.changeNext(undefined);
            this._last = previous;
        } else {
            const previous = this.elementAt(position - 1);
            data = previous.next!.data;
            previous.changeNext(previous.next?.next);
        }

        this._count--;
        return data;
    }

    public move(from: number, to: number) {
        if (from === to) {
            return;
        }

        const data = this.remove(from);
        this.addAt(data, from < to ? to - 1 : to);
    }

    public elementAt(position: number) {
        if (position >= this.count) {
            throw new Error(`${position} is an invalid position. List has only ${this.count} elements`);
        }

        let idx = 0;
        let currentNode = this._first!; // Cannot be null because of the previous consition : (position >= this.count)
        while (idx < position) {
            idx++;
            currentNode = currentNode.next!;
        }
        return currentNode;
    }
}
