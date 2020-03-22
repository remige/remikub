import { CardColor, CardNumber } from "../constants/constants";
import { ICoordinates } from "./icoordinates";

export interface ICard {
    value: CardNumber;
    color: CardColor;
    coordinates?: ICoordinates;
}
