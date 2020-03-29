import { IFetcherError } from "./ifetcher-error";

export class FetcherResponse<T> {

    constructor(value?: T) {
        this._value = value || null;
    }

    public static Failed<T>(status: number, error: IFetcherError) {
        const result = new FetcherResponse<T>();
        result._status = status;
        result._error = error;
        return result;
    }

    private _value: T | null;
    public get value() { return this._value; }

    public get succeeded() { return this._error === undefined; }

    private _status: number;
    public get status() { return this._status; }

    private _error: IFetcherError;
    public get error() { return this._error; }
}
