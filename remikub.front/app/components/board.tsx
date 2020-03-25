import React from "react";
import { observer } from "mobx-react";
import { BoardStore } from "./board-store";
import { BoardService } from "../services/board-service";
import { Combination } from "./combination";
import { Hand } from "./hand";
import { LinkedList } from "../common/linked-list/linked-list";
import { ICard } from "../model/icard";

@observer
export class Board extends React.Component {

    private service = new BoardService();
    private store = new BoardStore();

    public async componentDidMount() {
        this.store.refreshBoard(await this.service.board());
    }

    public render() {
        return <div>
            {this.store.board.map((combinaison, idx) => <Combination key={idx} combination={combinaison} combinationId={idx} store={this.store} />)}
            <Combination combination={new LinkedList<ICard>()} combinationId={this.store.board.length} store={this.store} />

            <Hand store={this.store} />
        </div>;
    }
}
