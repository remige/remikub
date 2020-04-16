import React from "react";
import { observer, inject } from "mobx-react";
import { RouterStore } from "mobx-react-router";
import { GameService } from "../../services/game-service";
import { GameRegistrationStore } from "./game-registration-store";
import i18next from "i18next";
import { userContext } from "../../context/user-context";
import { gamePlayRoute } from "../app-router";
import { Dimmer, Segment, Header, Form, Input, Button, List, Icon } from "semantic-ui-react";

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
            <Dimmer.Dimmable as={Segment}>
                <Header as="h3">{i18next.t("gameRegistration.titre")}</Header>
                <Form onSubmit={async event => await this.createGame(event)} action="">
                    <Form.Field>
                        <Input label={i18next.t("gameRegistration.newGame")}
                            value={this.store.newGame}
                            onChange={event => this.store.setNewGame(event.target.value)}
                        />
                    </Form.Field>
                    <Button onClick={async event => await this.createGame(event)}>{i18next.t("gameRegistration.submit")}</Button>
                </Form>
            </Dimmer.Dimmable>
            <List divided>
                {this.store.games.map(game => <List.Item key={game.id} as="a" onClick={event => this.meetGame(event, game.id)}>
                    <List.Header>{game.name}</List.Header>
                    <List.Content floated="right">
                        <div onClick={event => this.deleteGame(event, game.id)}><Icon name="delete" link /></div>
                    </List.Content>
                </List.Item>)}
            </List>
        </div >;
    }

    private async createGame(event: React.FormEvent) {
        event.preventDefault();
        await this.service.createGame(this.store.newGame);
        await this.refresh();
    }

    private async meetGame(event: React.FormEvent, id: string) {
        event.stopPropagation();
        await this.service.addUserToGame(userContext.userName, id);
        this.props.routing.push(`${gamePlayRoute}/${id}`);
    }

    private async deleteGame(event: React.FormEvent, id: string) {
        event.stopPropagation();
        await this.service.deleteGame(id);
        await this.refresh();
    }

    private async refresh() {
        this.store.setNewGame();
        this.store.setGames(await this.service.games());
    }
}
