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
        await this.refresh();
    }

    public render() {
        return <div>
            <div>{i18next.t("board.currentUser", { user: this.store.currentUser })}</div>
            <div>{this.store.otherUsers.map(x => <span>{x}</span>)}</div>
            <button onClick={async () => await this.refresh()}>{i18next.t("board.reset")}</button>
            <button onClick={async () => await this.play()}>{i18next.t("board.submit")}</button>
            {this.store.board.map((combinaison, idx) => <Combination key={idx} combination={combinaison} combinationId={idx} store={this.store} place="board" />)}
            <Combination combination={new LinkedList<ICard>([])} combinationId={this.store.board.length} store={this.store} place="board" />

            <Hand store={this.store} service={this.service} gameId={this.props.match.params.gameId} drawCard={async () => await this.drawCard()} />
        </div>;
    }

    public async play() {
        await this.service.play(this.props.match.params.gameId, this.store.board.map(x => x.map(y => y)), this.store.hand.map(x => x));
        await this.refresh();
    }

    private async drawCard() {
        await this.service.drawCard(this.props.match.params.gameId);
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
