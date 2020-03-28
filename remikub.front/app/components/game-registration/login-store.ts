import { observable, computed, action } from "mobx";

export class LoginStore {
    @observable private _login: string = "";
    @computed public get login() { return this._login; }
    @action public setLogin(login: string) { this._login = login; }
}