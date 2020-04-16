import React, { createRef } from "react";
import {
    Router,
    Switch,
    Route,
    Redirect
} from "react-router-dom";
import { userContext } from "../context/user-context";
import { LoginPage } from "./game-registration/login-page";
import { createHashHistory } from "history";
import { RouterStore, syncHistoryWithStore } from "mobx-react-router";
import { Provider, observer } from "mobx-react";
import { GameRegistration } from "./game-registration/game-registration";
import { Board } from "./board/board";
import { Ref } from "semantic-ui-react";
import { Header } from "./header/header";
export const gameRegistrationRoute = "/game-registration";
export const gamePlayRoute = "/game-play";

const routingStore = new RouterStore();
const history = syncHistoryWithStore(createHashHistory(), routingStore);

const stores = {
    routing: routingStore,
};

@observer
export class AppRouter extends React.Component {
    private contextRef = createRef();


    public render() {
        return <Provider {...stores}>
            <Router history={history}  >
                {this.renderApp()}
            </Router>
        </Provider>;
    }

    private renderApp() {
        return <div>
            <Ref innerRef={this.contextRef}>
                <Header context={this.contextRef} />
            </Ref>
            <div>
                {this.renderContent()}
            </div>
        </div>;
    }

    private renderContent() {
        if (!userContext.userName) {
            return <LoginPage routing={routingStore} />;
        }

        return <Switch>
            <Route path={gameRegistrationRoute} component={GameRegistration} />
            <Route path={`${gamePlayRoute}/:gameId`} component={Board} />
            <Redirect to={gameRegistrationRoute} />
        </Switch>;
    }
}