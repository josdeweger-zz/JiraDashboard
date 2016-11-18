import React, { Component } from 'react';
import {Card, Message} from 'semantic-ui-react';

class CustomerErrorCardComponent extends Component {
    render() {
        console.error(this.props.error);

        return(
            <Card className="customer-error-card">
                <Message negative>
                    <Message.Header>Something went wrong!</Message.Header>
                </Message>
            </Card>
        )
    }
}

export default CustomerErrorCardComponent