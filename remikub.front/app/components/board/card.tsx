import React from "react";
import { ICard } from "../../model/icard";
import "./style.scss";
import { observer } from "mobx-react";
import { observable, action } from "mobx";
import { ICoordinates } from "../../model/icoordinates";

interface ICardProps {
    card?: ICard;
    coordinates?: ICoordinates;
    moveCard?: (coordinates: ICoordinates) => void;
}

@observer
export class Card extends React.Component<ICardProps> {

    public render() {
        if (this.props.card) {
            return <div className={`card filled ${this.props.card.color} ${this.isMouving && "onMovement"}`}
                draggable={true}
                onDragStart={event => this.onDragStart(event)}
                onDragEnd={() => this.setIsMoving(false)}>
                {!this.isMouving && <div className="card-value">{this.props.card.value}</div>}
            </div>;
        }
        return <div className={`card empty ${this.isOvered ? "" : "hidden"}`}
            onDragEnter={_ => this.setIsOvered(true)}
            onDragLeave={_ => this.setIsOvered(false)}
            onDragOver={event => this.onDragOver(event)}
            onDrop={event => this.onDrop(event)}>{this.isOvered && "+"}</div>;
    }

    private onDragStart(event: React.DragEvent<HTMLDivElement>) {
        this.setIsMoving(true);
        event.dataTransfer.setData("text/json", JSON.stringify(this.props.coordinates));
    }

    @observable private isMouving = false;
    @action private setIsMoving(isMouving: boolean) { this.isMouving = isMouving; }

    @observable private isOvered = false;
    @action private setIsOvered(isOvered: boolean) { this.isOvered = isOvered; }
    private onDragOver(event: React.DragEvent<HTMLDivElement>) {
        event.preventDefault();
        this.setIsOvered(true);
    }

    private onDrop(event: React.DragEvent<HTMLDivElement>) {
        this.setIsOvered(false);

        event.preventDefault();
        const fromCoordinate = JSON.parse(event.dataTransfer.getData("text/json")) as ICoordinates;
        this.props.moveCard!(fromCoordinate);
    }
}
