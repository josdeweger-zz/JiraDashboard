import React, { Component } from 'react';
import {Header} from 'semantic-ui-react';

class NotFoundComponent extends Component {
    render() {
        return (
            <div>
                <Header>
                    Ai. Not found. Too bad. Better luck next time (on a different page, 'cause no one is gonna fix this one...).
                </Header>
            </div>
        );
    }
}

export default NotFoundComponent;