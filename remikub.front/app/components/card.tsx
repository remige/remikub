import React from "react";
import { ICard } from "../model/icard";
import "./style.scss";
import { IPosition } from "../model/iposition";

interface ICardProps {
    card?: ICard;
    position?: IPosition;
    onCardPut?: (card: ICard, source?: IPosition) => void;
}

export class Card extends React.Component<ICardProps> {

    public render() {
        if (this.props.card) {
            return <div className={`card filled ${this.props.card.color}`}
                            draggable={true}
                            onDragStart={event => this.onDragStart(event)}>{this.props.card.number}</div>;
        }
        return <div className="card empty" onDragOver={event => this.onDragOver(event)}
                                            onDrop={event => this.onDrop(event)} >+</div>;
    }

    private onDragStart(event: React.DragEvent<HTMLDivElement>) {
        event.dataTransfer.setData("text/json", JSON.stringify({
            card: this.props.card,
            source: this.props.position
        }));
    }

    private onDragOver(event: React.DragEvent<HTMLDivElement>) {
        event.preventDefault();
    }

    private onDrop(event: React.DragEvent<HTMLDivElement>) {
        event.preventDefault();
        const dataTransfered = JSON.parse(event.dataTransfer.getData("text/json"));
        this.props.onCardPut!(dataTransfered.card as ICard, dataTransfered.source as IPosition);
    }
}
