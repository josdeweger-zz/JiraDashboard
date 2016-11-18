import React, { Component } from 'react';
import {Card, Message} from 'semantic-ui-react';

class CustomerErrorCardComponent extends Component {
    render() {
        return(
            <Card className="customer-error-card">
                <Message negative>
                    <Message.Header>Something went wrong!</Message.Header>
                    <p>Error: {this.props.error}</p>
                </Message>
            </Card>
        )
    }
}

export default CustomerErrorCardComponent