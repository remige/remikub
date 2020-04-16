import React from "react";
import { Sticky, Header, Dimmer, Segment, Icon } from "semantic-ui-react";
import i18next from "i18next";
import { userContext } from "../../context/user-context";
import "./style.scss";

interface IHeaderProps {
    context: object | React.Ref<HTMLElement>;
}

export class AppHeader extends React.Component<IHeaderProps> {

    public render() {
        return <Sticky context={this.props.context}>
            <Dimmer.Dimmable as={Segment} className="header">
                <Header as="h3">{i18next.t("header.title")}</Header>
                <div className="user-information">
                    <Header as="h5">{userContext.userName}</Header>
                    {userContext.userName &&
                        <Icon name="log out" link onClick={() => userContext.setUserInfo(null)} />}
                </div>
            </Dimmer.Dimmable>
        </Sticky>;
    }
}