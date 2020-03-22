import { CardColor } from "../constants/constants";
import { ICoordinates } from "./icoordinates";

export interface ICard {
    number: 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 | 10 | 11 | 12 | 13;
    color: CardColor;
    coordinates: ICoordinates;
}
