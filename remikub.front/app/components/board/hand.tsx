import React from "react";
import "./style.scss";
import { BoardStore } from "./board-store";
import { Combination } from "./combination";
import { observer } from "mobx-react";
import { GameService } from "../../services/game-service";

interface IHandProps {
    store: BoardStore;
    service: GameService;
    gameId: string;
}

@observer
export class Hand extends React.Component<IHandProps> {

    public render() {
        return <div className="hand">
            <Combination combination={this.props.store.hand} combinationId={0} store={this.props.store} place="hand" />
        </div>;
    }
}