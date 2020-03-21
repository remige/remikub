import i18next from "i18next";

import { autorun } from "mobx";
import { LocaleType } from "../constants/constants";
import { translationFrFR } from "./fr-FR/index";
import { translationEnGB } from "./en-GB/index";

import "moment/locale/fr.js";
import "moment/locale/en-gb.js";
import { userContext } from "../context/user-context";

export class I18nManager {

    private isOnChangeLocaleRegistred = false;

    public init(onChangeLocale: (newLocale?: LocaleType) => Promise<void>) {
        return new Promise((resolve, reject) => {
            i18next.init({
                lng: userContext.locale,
                resources: {
                    "en-GB": { translation: this.load("en-GB") },
                    "fr-FR": { translation: this.load("fr-FR") },
                },
            }, err => {
                if (err) {
                    reject(err);
                }
                this.registerOnChangeLocale(onChangeLocale);
                resolve();
            });
        });
    }

    private registerOnChangeLocale(onChange: (newLocale?: LocaleType) => Promise<void>) {
        autorun(() => {
            if (userContext.locale && this.isOnChangeLocaleRegistred) {
                i18next.changeLanguage(userContext.locale, async () => await onChange(userContext.locale));
            }
            this.isOnChangeLocaleRegistred = true;
        });
    }

    private load(locale: LocaleType) {
        if (locale === "en-GB") {
            return translationEnGB;
        } else if (locale === "fr-FR") {
            return translationFrFR;
        }
        throw new Error(`This locale is not supported`);
    }
}

export const i18nManager = new I18nManager();
