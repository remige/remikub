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
        await this.store.refreshBoard(await this.service.board(this.props.match.params.gameId));
    }

    public render() {
        return <div>
            <button onClick={async () => await this.play()}>{i18next.t("board.submit")}</button>
            {this.store.board.map((combinaison, idx) => <Combination key={idx} combination={combinaison} combinationId={idx} store={this.store} place="board" />)}
            <Combination combination={new LinkedList<ICard>([])} combinationId={this.store.board.length} store={this.store} place="board" />

            <Hand store={this.store} service={this.service} gameId={this.props.match.params.gameId} />
        </div>;
    }

    public async play() {
        await this.service.play(this.props.match.params.gameId, this.store.board.map(x => x.map(y => y)), this.store.hand.map(x => x));
    }
}