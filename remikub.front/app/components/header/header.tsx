import React from "react";
import { Sticky, Header as HeaderSui } from "semantic-ui-react";

interface IHeaderProps {
    context: object | React.Ref<HTMLElement>;
}

export class Header extends React.Component<IHeaderProps> {

    public render() {
        return <Sticky context={this.props.context}>
            <HeaderSui as="h3">Stuck Content</HeaderSui>
        </Sticky>;
    }
}