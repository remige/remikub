import React from "react";
import { ICard } from "../model/icard";
import "./style.scss";
import { observer } from "mobx-react";
import { observable, action } from "mobx";

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
                onDragStart={event => this.onDragStart(event)}>{this.props.card.value}</div>;
        }
        return <div className={`card empty ${this.isOvered ? "" : "hidden"}`}
            onDragEnter={_ => this.setIsOvered(true)}
            onDragLeave={_ => this.setIsOvered(false)}
            onDragOver={event => this.onDragOver(event)}
            onDrop={event => this.onDrop(event)}>{this.isOvered && "+"}</div>;
    }

    private onDragStart(event: React.DragEvent<HTMLDivElement>) {
        event.dataTransfer.setData("text/json", JSON.stringify(this.props.card));
    }

    @observable private isOvered = false;
    @action private setIsOvered(isOvered: boolean) { this.isOvered = isOvered; }
    private onDragOver(event: React.DragEvent<HTMLDivElement>) {
        event.preventDefault();
        this.setIsOvered(true);
    }

    private onDrop(event: React.DragEvent<HTMLDivElement>) {
        this.setIsOvered(false);

        event.preventDefault();
        const sourceCard = JSON.parse(event.dataTransfer.getData("text/json")) as ICard;
        this.props.moveCard!(sourceCard);
    }
}
