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
import { userContext } from "../../context/user-context";

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
        this.store.refreshBoard(
            await this.service.board(this.props.match.params.gameId),
            await this.service.hand(this.props.match.params.gameId, userContext.userName));
    }

    public render() {
        return <div>
            {this.store.board.map((combinaison, idx) => <Combination key={idx} combination={combinaison} combinationId={idx} store={this.store} place="board" />)}
            <Combination combination={new LinkedList<ICard>([])} combinationId={this.store.board.length} store={this.store} place="board" />

            <Hand store={this.store} />
        </div>;
    }
}
