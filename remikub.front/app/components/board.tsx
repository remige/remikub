import React from "react";
import { observer } from "mobx-react";
import { BoardStore } from "./board-store";
import { BoardService } from "../services/board-service";
import { Combination } from "./combination";

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
            <Combination combination={[]} combinationId={this.store.board.length} store={this.store} />
        </div>;
    }
}
