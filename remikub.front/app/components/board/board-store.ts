import { observable, computed, action } from "mobx";
import { ICard } from "../../model/icard";
import { ICoordinates } from "../../model/icoordinates";
import { LinkedList } from "../../common/linked-list/linked-list";
import { userContext } from "../../context/user-context";

export class BoardStore {
    @observable private _currentUser: string;
    @computed public get currentUser() { return this._currentUser; }

    @observable private _users: string[] = [];
    @computed public get users() { return this._users; }

    @observable private _hand: LinkedList<ICard> = new LinkedList<ICard>();
    @computed public get hand() { return this._hand; }

    @observable private _board: LinkedList<ICard>[] = [];
    @computed public get board() { return this._board; }

    @observable private _moved: boolean;
    @computed public get moved() { return this._moved; }

    @computed public get isUserTurn() { return this._currentUser === userContext.userName; }

    @action public refreshBoard(board: ICard[][], currentUser: string, users: string[]) {
        this._board = board.map(x => new LinkedList<ICard>(x));
        this._currentUser = currentUser;
        this._users = users;
        this._moved = false;
    }

    @action public refreshHand(hand: ICard[]) {
        this._hand = new LinkedList<ICard>(hand);
    }

    @action public moveCard(from: ICoordinates, to: ICoordinates) {
        if (from.place === "board" && to.place === "hand") {
            throw new Error("Cannot move cad from the bord to your hand");
        }

        if (from.place === "hand" && to.place === "hand") {
            return this._hand.move(from.rank, to.rank);
        }

        this._moved = true;
        if (from.place === "hand" && to.place === "board") {
            const card = this._hand.remove(from.rank);
            this.addAt(card, to);
        } else if (from.combinationId === to.combinationId) {
            this._board[from.combinationId].move(from.rank, to.rank);
        } else {
            const card = this._board[from.combinationId].remove(from.rank);
            this.addAt(card, to);
        }
    }

    private addAt(card: ICard, to: ICoordinates) {
        if (this._board.length === to.combinationId) {
            this._board.push(new LinkedList<ICard>([card]));
        } else {
            this._board[to.combinationId].addAt(card, to.rank);
        }
    }
}
