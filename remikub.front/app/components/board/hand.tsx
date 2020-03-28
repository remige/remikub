import React from "react";
import "./style.scss";
import { BoardStore } from "./board-store";
import { Combination } from "./combination";

interface IHandProps {
    store: BoardStore;
}

export class Hand extends React.Component<IHandProps> {

    public render() {
        return <div className="hand">
            <Combination combination={this.props.store.hand} combinationId={0} store={this.props.store} place="hand" />
        </div>;
    }
}