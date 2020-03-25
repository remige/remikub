import { observable, computed, action } from "mobx";
import { ICard } from "../model/icard";
import { ICoordinates } from "../model/icoordinates";
import { LinkedList } from "../common/linked-list/linked-list";

export class BoardStore {
    @observable private _hand: ICard[] = [{ value: 1, color: "Red" }, { value: 1, color: "Red" }, { value: 3, color: "Blue" }];
    @computed public get hand() { return this._hand; }

    @observable private _board: LinkedList<ICard>[] = [];
    @computed public get board() { return this._board; }
    @action public refreshBoard(board: ICard[][]) {
        this._board = board.map(x => {
            const combination = new LinkedList<ICard>();
            x.forEach(y => combination.addLast(y));
            return combination;
        });
    }

    @action public moveCard(from: ICoordinates | undefined, to: ICoordinates) {
        if (!from) {
            // TODO : move from the hand
        } else if (from.combinationId === to.combinationId) {
            this._board[from.combinationId].move(from.rank, to.rank);
        } else {
            const card = this._board[from.combinationId].remove(from.rank);
            this._board[to.combinationId].addAt(card, to.rank);
        }
    }
}
