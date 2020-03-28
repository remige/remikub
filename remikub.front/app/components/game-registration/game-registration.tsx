import React from "react";
import { observer, inject } from "mobx-react";
import { RouterStore } from "mobx-react-router";
import { GameService } from "../../services/game-service";
import { GameRegistrationStore } from "./game-registration-store";
import i18next from "i18next";
import { userContext } from "../../context/user-context";
import { gamePlayRoute } from "../app-router";

interface IGameRegistration {
    routing: RouterStore;
}

@inject("routing")
@observer
export class GameRegistration extends React.Component<IGameRegistration> {

    private store = new GameRegistrationStore();
    private service = new GameService();

    public async componentDidMount() {
        await this.refresh();
    }

    public render() {
        return <div>
            <form onSubmit={event => this.createGame(event)} action="">
                <label htmlFor="newGame">{i18next.t("gameRegistration.newGame")}</label>
                <input type="text" name="newGame" id="newGame"
                    value={this.store.newGame}
                    onChange={event => this.store.setNewGame(event.target.value)} />
            </form>
            <ul>
                {this.store.games.map(game => <li key={game.id}>
                    <div>{game.name}</div>
                    <div><button onClick={_ => this.meetGame(game.id)}>{i18next.t("gameRegistration.meetGame")}</button></div>
                </li>)}
            </ul>
        </div>;
    }

    private async createGame(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();
        await this.service.createGame(this.store.newGame);
        await this.refresh();
    }

    private async meetGame(id: string) {
        await this.service.addUserToGame(userContext.userName, id);
        this.props.routing.push(`${gamePlayRoute}/${id}`);
    }

    private async refresh() {
        this.store.setGames(await this.service.games());
        this.store.setNewGame();
    }
}
