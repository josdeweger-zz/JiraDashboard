import React, { Component } from 'react';
import {Header} from 'semantic-ui-react';

class AboutComponent extends Component {
    render() {
        return (
            <div>
                <Header>
                    About the Jira Dashboard
                </Header>
                <p>
                    The Jira Dashboard helps teams to get insight into hours spent on projects. 
                </p>
            </div>
        );
    }
}

export default AboutComponent;