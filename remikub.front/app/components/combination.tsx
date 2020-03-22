import React from "react";
import { ICard } from "../model/icard";
import { Card } from "./card";
import { BoardStore } from "./board-store";
import { observer } from "mobx-react";

interface ICombinationProps {
    store: BoardStore;
    combinationId: number;
    combination: ICard[];
}

@observer
export class Combination extends React.Component<ICombinationProps> {

    public render() {
        return <div style={{ display: "flex" }}>
            {this.props.combination.map(card => <div key={card.coordinates.rank} style={{ display: "flex" }}>
            <Card moveCard={c => this.props.store.moveCard(c, {
                combinationId: this.props.combinationId,
                rank: card.coordinates.rank
            })} />
            <Card card={card} />
            <Card moveCard={c => this.props.store.moveCard(c, {
                combinationId: this.props.combinationId,
                rank: card.coordinates.rank + 1
            })} />
        </div>)}
        </div>;
    }
}