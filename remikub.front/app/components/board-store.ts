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
        orderBy(this._board, ["combinationId", "position"]).forEach(x => {
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
            const combination = filter(this._board, x => x.combinationId === source.combinaisonId);
            combination.forEach(x => {
                if (x.position === source.cardId) {
                    x.combinationId = destination.combinaisonId;
                    x.position = destination.cardId;
                } else if (x.position > source.cardId) {
                    x.position--;
                }
            });
            if (combination.length === 1) {
                filter(this._board, x => x.combinationId > source.combinaisonId)
                    .forEach(x => x.combinationId--);
            }
        } else {
            this._board.push(extend({}, card, { combinaisonId: destination.combinaisonId, position: destination.cardId }));
        }
    }
}
