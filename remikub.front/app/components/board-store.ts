import { observable, computed, action } from "mobx";
import { ICard } from "../model/icard";
import { ICoordinates } from "../model/icoordinates";
import { orderBy, extend, filter } from "lodash-es";

export class BoardStore {

    @observable private _board: ICard[] = [];
    @action public refreshBoard(board: ICard[]) { this._board = board; }
    @computed public get board() {
        const board: ICard[][] = [];

        let currentCombinationId = -1;
        let currentCombination: ICard[] = [];
        orderBy(this._board, ["coordinates.combinationId", "coordinates.rank"]).forEach(x => {
            if (currentCombinationId !== x.coordinates.combinationId) {
                if (currentCombinationId !== -1) {
                    board.push(currentCombination);
                    currentCombination = [];
                }
                currentCombinationId = x.coordinates.combinationId;
            }
            currentCombination.push(x);
        });
        if (currentCombination.length > 0) {
            board.push(currentCombination);
        }

        return board;
    }

    @action public moveCard(card: ICard, destination: ICoordinates) {
        if (card.coordinates) {
            const combination = filter(this._board, x => x.coordinates.combinationId === card.coordinates.combinationId);
            combination.forEach(x => {
                if (x.coordinates.rank === card.coordinates.rank) {
                    x.coordinates.combinationId = destination.combinationId;
                    x.coordinates.rank = destination.rank;
                } else if (x.coordinates.rank > card.coordinates.rank) {
                    x.coordinates.rank--;
                }
            });
            if (combination.length === 1) {
                filter(this._board, x => x.coordinates.combinationId > card.coordinates.combinationId)
                    .forEach(x => x.coordinates.combinationId--);
            }
        } else {
            this._board.push(extend({}, card, { combinationId: destination.combinationId, rank: destination.rank }));
        }
    }
}
