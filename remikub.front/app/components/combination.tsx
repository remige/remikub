import React from "react";
import { ICard } from "../model/icard";
import { Card } from "./card";
import { BoardStore } from "./board-store";
import { observer } from "mobx-react";
import { LinkedList } from "../common/linked-list/linked-list";

interface ICombinationProps {
    store: BoardStore;
    combinationId: number;
    combination: LinkedList<ICard>;
}

@observer
export class Combination extends React.Component<ICombinationProps> {

    public render() {
        return <div style={{ display: "flex" }}>
            {this.props.combination.map((card, rank) => <div key={rank} style={{ display: "flex" }}>
                {rank === 0 &&
                    <Card moveCard={fromCoorinates => this.props.store.moveCard(fromCoorinates, {
                        combinationId: this.props.combinationId,
                        rank
                    })} />}
                <Card card={card} coordinates={{ combinationId: this.props.combinationId, rank }} />
                <Card moveCard={fromCoorinates => this.props.store.moveCard(fromCoorinates, {
                    combinationId: this.props.combinationId,
                    rank: rank + 1
                })} />
            </div>)}
        </div>;
    }
}