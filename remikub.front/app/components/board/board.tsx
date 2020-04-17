import React from "react";
import { observer, inject } from "mobx-react";
import { BoardStore } from "./board-store";
import { GameService } from "../../services/game-service";
import { Combination } from "./combination";
import { Hand } from "./hand";
import { LinkedList } from "../../common/linked-list/linked-list";
import { ICard } from "../../model/icard";
import { match } from "react-router";
import { RouterStore } from "mobx-react-router";
import i18next from "i18next";
import { signalrClient } from "../../signalR/signalr-client";
import { IUserHasPlayed } from "../../model/iuser-has-played";
import { Label, Icon, Popup } from "semantic-ui-react";
import { IUserHasWon } from "../../model/iuser-has-won";
import { Victory } from "./victory";

interface IBoardProps {
    routing: RouterStore;
    match: match<{ gameId: string }>;
}

@inject("routing")
@observer
export class Board extends React.Component<IBoardProps> {

    private service = new GameService();
    private store = new BoardStore();

    public async componentDidMount() {
        signalrClient.register("UserHasPlayed", (message: IUserHasPlayed) => {
            if (message.gameId === this.props.match.params.gameId) {
                this.refresh();
            }
        });
        signalrClient.register("UserHasWon", (message: IUserHasWon) => {
            if (message.gameId === this.props.match.params.gameId) {
                this.store.declareWinner(message.user);
            }
        });
        await this.refresh();
    }

    public async componentWillUnmount() {
        signalrClient.unregister("UserHasPlayed");
        signalrClient.unregister("UserHasWon");
    }

    public render() {
        return <div className="board">
            <div className="board-header">
                <div>
                    {this.store.users.map(x => <Label key={x} color={x === this.store.currentUser ? "red" : undefined} >{x}</Label>)}
                </div>
                {this.store.isUserTurn && <div>
                    <Popup content={i18next.t("board.reset")} trigger={
                        <Icon link size="huge" color="red" name="cancel" onClick={async () => await this.refresh()}></Icon>} />
                    <Popup content={i18next.t("board.submit")} trigger={
                        <Icon link={this.store.moved}
                            disabled={!this.store.moved}
                            size="huge" color="green" name="check" onClick={async () => await this.play()}></Icon>} />
                    <Popup content={i18next.t("board.drawCard")} trigger={
                        <Icon link={!this.store.moved}
                            disabled={this.store.moved}
                            size="huge" color="green" name="plus" onClick={async () => await this.drawCard()}></Icon>} />
                </div>}
            </div>
            {this.store.board.map((combinaison, idx) => <Combination key={idx} combination={combinaison} combinationId={idx} store={this.store} place="board" />)}
            <Combination combination={new LinkedList<ICard>([])} combinationId={this.store.board.length} store={this.store} place="board" />

            <Hand store={this.store} service={this.service} gameId={this.props.match.params.gameId} />

            <Victory store={this.store} />;
        </div>;
    }

    public async play() {
        await this.service.play(this.props.match.params.gameId, this.store.board.map(x => x.map(y => y)), this.store.hand.map(x => x));
        await this.refresh();
    }

    private async drawCard() {
        await this.service.drawCard(this.props.match.params.gameId, this.store.hand.map(x => x));
        await this.refresh();
    }

    public async refresh() {
        await this.store.refreshBoard(
            await this.service.board(this.props.match.params.gameId),
            await this.service.currentUser(this.props.match.params.gameId),
            await this.service.users(this.props.match.params.gameId),
        );
        await this.store.refreshHand(await this.service.hand(this.props.match.params.gameId));
    }
}
