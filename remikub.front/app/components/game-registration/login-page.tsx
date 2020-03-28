import React from "react";
import i18next from "i18next";
import { LoginStore } from "./login-store";
import { userContext } from "../../context/user-context";
import { gameRegistrationRoute } from "../app-router";
import { RouterStore } from "mobx-react-router";
import { observer, inject } from "mobx-react";

interface ILoginPageProps {
    routing: RouterStore;
}

@inject("routing")
@observer
export class LoginPage extends React.Component<ILoginPageProps> {

    private loginStore = new LoginStore();

    public render() {
        return <form onSubmit={event => this.login(event)} action="">
            <label htmlFor="login">{i18next.t("login.login")}</label>
            <input type="text" name="login" id="login"
                value={this.loginStore.login}
                onChange={event => this.loginStore.setLogin(event.target.value)} />
        </form>;
    }

    private login(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();
        userContext.setUserInfo(this.loginStore.login);
        this.props.routing.push(gameRegistrationRoute);
    }
}