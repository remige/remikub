import React from "react";
import { observer } from "mobx-react";
import { BoardStore } from "./board-store";
import { BoardService } from "../services/board-service";
import { ICard } from "../model/icard";
import { Card } from "./card";

@observer
export class Board extends React.Component {

    private service = new BoardService();
    private store = new BoardStore();

    public async componentDidMount() {
        this.store.refreshBoard(await this.service.board());
    }

    public render() {
        return <table>
            {this.store.board.map((combinaison, idx) => this.renderCombinaison(combinaison, idx))}
            {this.renderCombinaison([], this.store.board.length)}
        </table>;
    }

    private renderCombinaison(combinaison: ICard[], combinaisonId: number) {
        return <tr key={combinaisonId}>
            {combinaison.map((card, cardId) => <td key={cardId}><Card card={card} position={{combinaisonId, cardId}} /></td>)}
            <Card onCardPut={(card, source) => this.store.moveCard(card, {
                combinaisonId,
                cardId: combinaison.length
            }, source)} />
        </tr>;
    }
}
