import { observable, computed, action } from "mobx";
import { ICard } from "../model/icard";
import { ICoordinates } from "../model/icoordinates";
import { orderBy, extend, filter, find } from "lodash-es";

export class BoardStore {
    @observable private _hand: ICard[] = [{ value: 1, color: "Red" }, { value: 1, color: "Red" }, { value: 3, color: "Blue" }];
    @computed public get hand() { return this._hand; }

    @observable private _board: ICard[] = [];
    @action public refreshBoard(board: ICard[]) { this._board = board; }
    @computed public get board() {
        const board: ICard[][] = [];

        let currentCombinationId = -1;
        let currentCombination: ICard[] = [];
        orderBy(this._board, ["coordinates.combinationId", "coordinates.rank"]).forEach(x => {
            if (currentCombinationId !== x.coordinates!.combinationId) {
                if (currentCombinationId !== -1) {
                    board.push(currentCombination);
                    currentCombination = [];
                }
                currentCombinationId = x.coordinates!.combinationId;
            }
            currentCombination.push(x);
        });
        if (currentCombination.length > 0) {
            board.push(currentCombination);
        }

        return board;
    }

    @action public moveCard(card: ICard, destination: ICoordinates) {
        if (!card.coordinates) {
            // Add card
            const cardToAdd = extend({}, card, { coordinates: { combinationId: destination.combinationId, rank: destination.rank } });
            this._board.push(cardToAdd);
            const idxToRemove = this._hand.findIndex(x => x.value === card.value && x.color === card.color);
            this._hand.splice(idxToRemove, 1);
        } else if (card.coordinates!.combinationId !== destination.combinationId) {
            // Move on different line
            const combinationSource = this.getCombination(card.coordinates!.combinationId);
            const combinationDestination = this.getCombination(destination.combinationId);
            combinationDestination.forEach(x => {
                if (x.coordinates!.rank >= destination.rank) {
                    x.coordinates!.rank++;
                }
            });
            combinationSource.forEach(x => {
                if (x.coordinates!.rank === card.coordinates!.rank) {
                    // The card is moved
                    x.coordinates!.combinationId = destination.combinationId;
                    x.coordinates!.rank = destination.rank;
                } else if (x.coordinates!.rank > card.coordinates!.rank) {
                    // All the combination cards after are moved to the left
                    x.coordinates!.rank--;
                }
            });
            if (combinationSource.length === 1) {
                // If the moved card is the last one, the combination will be empty, it will be removed
                // => all the next combination need to be moved to the top
                filter(this._board, x => x.coordinates!.combinationId > card.coordinates!.combinationId)
                    .forEach(x => x.coordinates!.combinationId--);
            }
        } else if (card.coordinates!.rank < destination.rank) {
            // Move on the same line from the left to the right
            const combination = this.getCombination(destination.combinationId);
            const toMove = find(combination, x => x.coordinates!.rank === card.coordinates!.rank);
            const toShift = filter(combination, x => x.coordinates!.rank > card.coordinates!.rank && x.coordinates!.rank < destination.rank);
            toMove!.coordinates!.rank = destination.rank - 1;
            toShift.forEach(x => {
                x.coordinates!.rank--;
            });
        } else if (card.coordinates!.rank > destination.rank) {
            // Move on the same line from the right to the left
            const combination = this.getCombination(destination.combinationId);
            const toMove = find(combination, x => x.coordinates!.rank === card.coordinates!.rank);
            const toShift = filter(combination, x => x.coordinates!.rank >= destination.rank && x.coordinates!.rank < card.coordinates!.rank);
            toMove!.coordinates!.rank = destination.rank;
            toShift.forEach(x => {
                x.coordinates!.rank++;
            });
        }
    }

    private getCombination(combinationId: number) {
        return filter(this._board, x => x.coordinates!.combinationId === combinationId);
    }
}
