import { observable, computed, action } from "mobx";
import { ICard } from "../model/icard";
import { ICoordinates } from "../model/icoordinates";
import { LinkedList } from "../common/linked-list/linked-list";

export class BoardStore {
    @observable private _hand: LinkedList<ICard> = new LinkedList<ICard>();
    @computed public get hand() { return this._hand; }

    @observable private _board: LinkedList<ICard>[] = [];
    @computed public get board() { return this._board; }

    @action public refreshBoard(board: ICard[][], hand: ICard[]) {
        this._board = board.map(x => new LinkedList<ICard>(x));
        this._hand = new LinkedList<ICard>(hand);
    }

    @action public moveCard(from: ICoordinates, to: ICoordinates) {
        if (from.place === "board" && to.place === "hand") {
            throw new Error("Cannot move cad frm the bord to your hand");
        }

        if (from.place === "hand" && to.place === "hand") {
            this._hand.move(from.rank, to.rank);
        } else if (from.place === "hand" && to.place === "board") {
            const card = this._hand.remove(from.rank);
            this._board[to.combinationId].addAt(card, to.rank);
        } else if (from.combinationId === to.combinationId) {
            this._board[from.combinationId].move(from.rank, to.rank);
        } else {
            const card = this._board[from.combinationId].remove(from.rank);
            this._board[to.combinationId].addAt(card, to.rank);
        }
    }
}
