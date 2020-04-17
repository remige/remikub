import React from "react";
import { Modal } from "semantic-ui-react";
import { BoardStore } from "./board-store";
import i18next from "i18next";
import { observer } from "mobx-react";
import "./style.scss";

interface IVictoryProps {
    store: BoardStore;
}

@observer
export class Victory extends React.Component<IVictoryProps> {

    public render() {
        return <Modal open={!!this.props.store.winner} size="tiny" className="victory">
            <Modal.Header>{i18next.t("victory.title", { winner: this.props.store.winner })}</Modal.Header>
            <Modal.Content><img src="https://media.giphy.com/media/1dMNqVx9Kb12EBjFrc/giphy.gif" /></Modal.Content>
        </Modal>;
    }
}