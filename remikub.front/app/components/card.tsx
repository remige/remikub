import React from "react";
import { ICard } from "../model/icard";
import "./style.scss";
import { observer } from "mobx-react";

interface ICardProps {
    card?: ICard;
    moveCard?: (card: ICard) => void;
}

@observer
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
        event.dataTransfer.setData("text/json", JSON.stringify(this.props.card));
    }

    private onDragOver(event: React.DragEvent<HTMLDivElement>) {
        event.preventDefault();
    }

    private onDrop(event: React.DragEvent<HTMLDivElement>) {
        event.preventDefault();
        const sourceCard = JSON.parse(event.dataTransfer.getData("text/json"))  as ICard;
        this.props.moveCard!(sourceCard);
    }
}
