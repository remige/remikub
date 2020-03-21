import { observable, computed, action } from "mobx";
import { ICard } from "../model/icard";
import { IPosition } from "../model/iposition";
import { orderBy, extend, filter } from "lodash-es";

export class BoardStore {

    @observable private _board: ICard[] = [];
    @action public refreshBoard(board: ICard[]) { this._board = board; }
    @computed public get board() {
        const board: ICard[][] = [];

        let currentCombinationId = -1;
        let currentCombination: ICard[] = [];
        orderBy(this._board, ["combinationId", "rank"]).forEach(x => {
            if (currentCombinationId !== x.combinationId) {
                if (currentCombinationId !== -1) {
                    board.push(currentCombination);
                    currentCombination = [];
                }
                currentCombinationId = x.combinationId;
            }
            currentCombination.push(x);
        });
        if (currentCombination.length > 0) {
            board.push(currentCombination);
        }

        return board;
    }

    @action public moveCard(card: ICard, destination: IPosition, source?: IPosition) {
        if (source) {
            const combination = filter(this._board, x => x.combinationId === source.combinationId);
            combination.forEach(x => {
                if (x.rank === source.rank) {
                    x.combinationId = destination.combinationId;
                    x.rank = destination.rank;
                } else if (x.rank > source.rank) {
                    x.rank--;
                }
            });
            if (combination.length === 1) {
                filter(this._board, x => x.combinationId > source.combinationId)
                    .forEach(x => x.combinationId--);
            }
        } else {
            this._board.push(extend({}, card, { combinationId: destination.combinationId, rank: destination.rank }));
        }
    }
}
