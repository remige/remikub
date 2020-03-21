import { observable, computed, action } from "mobx";
import { ICard } from "../model/icard";
import { IPosition } from "../model/iposition";
import { filter } from "lodash-es";
export class BoardStore {

    @observable private _board: ICard[][] = [];
    @computed public get board() { return this._board; }
    @action public refreshBoard(board: ICard[][]) { this._board = board; }

    @action public moveCard(card: ICard, destination: IPosition, source?: IPosition) {
        if (source) {
            this._board[source.combinaisonId][source.cardId].toRemove = true;
        }

        if (this._board.length <= destination.combinaisonId) {
            this._board.splice(destination.combinaisonId, 0, []);
        }
        this._board[destination.combinaisonId].splice(destination.cardId, 0, card);

        const cardsToRemove: IPosition[] = [];
        this._board.forEach((combinaison, combinaisonId) => {
            combinaison.forEach((currentCard, cardId) => {
                if (currentCard.toRemove) {
                    cardsToRemove.push({ combinaisonId, cardId });
                }
            });
        });

        cardsToRemove.forEach(toRemove => this._board[toRemove.combinaisonId].splice(toRemove.cardId, 1));

        this._board = filter(this._board, x => x.length > 0);
    }
}
