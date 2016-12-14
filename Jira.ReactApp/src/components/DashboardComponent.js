import React, { Component } from 'react';
import Request from 'react-http-request';
import CustomerStatusCardsComponent from './CustomerStatusCardsComponent';

class DashboardComponent extends Component {
    render() {
        let settingsUrl = process.env.REACT_APP_JIRA_NODE_SERVER_SETTINGS_URL;

        return (
            <Request
                url={settingsUrl}
                method='get'
                accept='application/json'
                verbose={true}>
                {
                    ({error, result, loading}) => {
                        if (loading) {
                            return <div>Loading...</div>;
                        } else if(error) {
                            return <div>Error: {{error}}</div>;
                        } else {
                            return <CustomerStatusCardsComponent settings={result.body} />
                        }
                    }
                }
            </Request>
        );
    }
}

export default DashboardComponent;
