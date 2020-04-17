import React from "react";
import { ICard } from "../../model/icard";
import { Card } from "./card";
import { BoardStore } from "./board-store";
import { observer } from "mobx-react";
import { LinkedList } from "../../common/linked-list/linked-list";
import { PlaceType } from "../../constants/constants";

interface ICombinationProps {
    store: BoardStore;
    combinationId: number;
    combination: LinkedList<ICard>;
    place: PlaceType;
}

@observer
export class Combination extends React.Component<ICombinationProps> {

    public render() {
        return <div style={{ display: "flex" }} className="combination" >
            <Card moveCard={fromCoorinates => this.props.store.moveCard(fromCoorinates, {
                combinationId: this.props.combinationId,
                rank: 0,
                place: this.props.place,
            })} />
            {this.props.combination.map((card, rank) => <div key={rank} style={{ display: "flex" }}>
                <Card card={card} coordinates={{ combinationId: this.props.combinationId, rank, place: this.props.place }} />
                <Card moveCard={fromCoorinates => this.props.store.moveCard(fromCoorinates, {
                    combinationId: this.props.combinationId,
                    rank: rank + 1,
                    place: this.props.place,
                })} />
            </div>)}
        </div>;
    }
}