import React from "react";
import { Card } from "./card";
import "./style.scss";
import { BoardStore } from "./board-store";

interface IHandProps {
    store: BoardStore;
}

export class Hand extends React.Component<IHandProps> {

    public render() {
        return <div className="hand">
            {this.props.store.hand.map((x, idx) => <Card card={x} key={idx} />)}
        </div>;
    }
}