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
            {this.store.board. map((combinaison, idx) => this.renderCombinaison(combinaison, idx))}
            {this.renderCombinaison([], this.store.board.length)}
        </table>;
    }

    private renderCombinaison(combination: ICard[], combinationId: number) {
        return <tr key={combinationId}>
            {combination.map((card, rank) => <td key={rank}><Card card={card} position={{combinationId, rank}} /></td>)}
            <td key={combination.length}>
            <Card onCardPut={(card, source) => this.store.moveCard(card, {
                combinationId,
                rank: combination.length
            }, source)} />
            </td>
        </tr>;
    }
}
