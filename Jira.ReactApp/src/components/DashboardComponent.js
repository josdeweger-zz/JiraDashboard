import React, { Component } from 'react';
import Request from 'react-http-request';
import { Config } from '../Config';
import CustomerStatusCardsComponent from './CustomerStatusCardsComponent';

class DashboardComponent extends Component {
    render() {
        return (
            <Request
                url={Config.jiraNodeServerSettingsUrl}
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
