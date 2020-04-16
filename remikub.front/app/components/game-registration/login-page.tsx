import React from "react";
import i18next from "i18next";
import { LoginStore } from "./login-store";
import { userContext } from "../../context/user-context";
import { gameRegistrationRoute } from "../app-router";
import { RouterStore } from "mobx-react-router";
import { observer, inject } from "mobx-react";
import { Button, Input, Dimmer, Segment, Header, Form } from "semantic-ui-react";

interface ILoginPageProps {
    routing: RouterStore;
}

@inject("routing")
@observer
export class LoginPage extends React.Component<ILoginPageProps> {

    private loginStore = new LoginStore();

    public render() {
        return <Dimmer.Dimmable as={Segment}>
            <Header as="h3">{i18next.t("login.titre")}</Header>
            <Form onSubmit={event => this.login(event)} action="">
                <Form.Field>
                    <Input label={i18next.t("login.login")}
                        value={this.loginStore.login}
                        onChange={event => this.loginStore.setLogin(event.target.value)}
                    />
                </Form.Field>
                <Button onClick={event => this.login(event)}>{i18next.t("login.submit")}</Button>
            </Form>
        </Dimmer.Dimmable>;
    }

    private login(event: React.FormEvent) {
        event.preventDefault();
        userContext.setUserInfo(this.loginStore.login);
        this.props.routing.push(gameRegistrationRoute);
    }
}