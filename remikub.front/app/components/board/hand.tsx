import React from "react";
import "./style.scss";
import { BoardStore } from "./board-store";
import { Combination } from "./combination";
import { observer } from "mobx-react";
import { GameService } from "../../services/game-service";
import i18next from "i18next";

interface IHandProps {
    store: BoardStore;
    service: GameService;
    gameId: string;
}

@observer
export class Hand extends React.Component<IHandProps> {

    public async componentDidMount() {
        await this.refresh();
    }

    public render() {
        return <div className="hand">
            <Combination combination={this.props.store.hand} combinationId={0} store={this.props.store} place="hand" />
            <button onClick={async () => await this.drawCard()}>{i18next.t("hand.drawCard")}</button>
        </div>;
    }

    private async drawCard() {
        await this.props.service.drawCard(this.props.gameId);
        await this.refresh();
    }

    private async refresh() {
        this.props.store.refreshHand(await this.props.service.hand(this.props.gameId));
    }
}