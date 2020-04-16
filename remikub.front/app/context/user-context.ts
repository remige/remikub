import { LocaleType } from "./../constants/constants";
import { observable, computed, action } from "mobx";
import * as moment from "moment";

export class UserContext {

    @observable private _locale: LocaleType = "fr-FR";
    @observable private _userName: string | null = window.localStorage.getItem(userKey);

    @computed public get locale() { return this._locale; }
    @computed public get userName() { return this._userName; }

    @action public updateLocale(locale: LocaleType) {
        moment.locale(locale);
        this._locale = locale;
    }
    @action
    public setUserInfo(userName: string | null) {
        this._userName = userName;
        if (userName) {
            window.localStorage.setItem(userKey, userName);
        } else {
            window.localStorage.removeItem(userKey);
        }
    }
}

const userKey = "userName";

export const userContext = new UserContext();
