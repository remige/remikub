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
        return <tr>
            {this.props.combination.map(card=> <td key={card.coordinates.rank}><Card card={card} /></td>)}
            <td key={this.props.combination.length}>
            <Card moveCard={card => this.props.store.moveCard(card, {
                combinationId: this.props.combinationId,
                rank: this.props.combination.length
            })} />
            </td>
        </tr>;
    }
}