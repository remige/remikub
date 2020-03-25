import React from "react";
import { ICard } from "../model/icard";
import "./style.scss";
import { observer } from "mobx-react";
import { observable, action } from "mobx";
import { ICoordinates } from "../model/icoordinates";

interface ICardProps {
    card?: ICard;
    coordinates?: ICoordinates;
    moveCard?: (coordinates?: ICoordinates) => void;
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
        if (this.props.coordinates) {
            event.dataTransfer.setData("text/json", JSON.stringify(this.props.coordinates));
        }
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
        const fromCoordinate = (event.dataTransfer.items.length > 0 ?
            JSON.parse(event.dataTransfer.getData("text/json")) as ICoordinates : undefined);
        this.props.moveCard!(fromCoordinate);
    }
}
