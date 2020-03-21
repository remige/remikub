import { i18nManager } from "./i18n/i18n-manager";
import "@babel/polyfill";
import "whatwg-fetch";

import { LocaleType } from "./constants/constants";
import { configure } from "mobx";
import ReactDOM from 'react-dom';
import React from 'react';
import { Board } from './components/board';

export const AppContainerId = "app-content";

class AppInitializer {

  public async initAsync() {
    configure({ enforceActions: "observed" });

    await i18nManager.init(async (newLocale?: LocaleType) => await this.renderApp(newLocale));

    if (document.readyState === "complete") {
      await this.renderApp();
    } else {
      document.addEventListener("DOMContentLoaded", async () => await this.renderApp());
    }
  }

  private async renderApp(_newLocale?: LocaleType) {

    ReactDOM.render(
      React.createElement(Board),
      document.getElementById(AppContainerId));
  }
}

const initializer = new AppInitializer();
initializer.initAsync();
