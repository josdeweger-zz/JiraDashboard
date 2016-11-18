import React, { Component } from 'react';
import {Loader, Card} from 'semantic-ui-react';

class CustomerLoaderCardComponent extends Component {
    render() {
        return(
            <Card className="customer-loader-card">
                <div className='ui active dimmer'>
                    <Loader />
                </div>
            </Card>
        )
    }
}

export default CustomerLoaderCardComponent