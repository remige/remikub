export class FetcherResponse<T> {

    constructor(succeeded: boolean, value?: T) {
        this._value = value || null;
        this._succeeded = succeeded;
    }

    public static Failed<T>(errorCode: number, errorMessage: string) {
        const result = new FetcherResponse<T>(false);
        result._errorCode = errorCode;
        result._errorMessage = errorMessage;
        result._succeeded = false;
        return result;
    }

    private _value: T | null;
    public get value() { return this._value; }

    public _succeeded: boolean;
    public get succeeded() { return this._succeeded; }

    private _errorCode: number;
    public get errorCode() { return this._errorCode; }

    private _errorMessage: string;
    public get errorMessage() { return this._errorMessage; }
}
