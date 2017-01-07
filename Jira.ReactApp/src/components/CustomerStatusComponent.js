import React, { Component } from 'react';
import Request from 'react-http-request';
import CustomerStatusCardComponent from './CustomerStatusCardComponent';
import CustomerLoaderCardComponent from './CustomerLoaderCardComponent';
import CustomerErrorCardComponent from './CustomerErrorCardComponent';
import map from 'lodash/map';
import Auth from '../Auth';

class CustomerStatusComponent extends Component {
    render() {
        let {customer, teamId, date, selectedSprint, hoursReserved} = this.props;
        let request = { 
            'session': Auth.getSessionCookie(),
            'teamId': teamId,
            'projectKeys': map(customer.projectKeys, 'key'), 
            'date': date.format(), 
            'sprint': { start: selectedSprint.start, end: selectedSprint.end }, 
            'hoursReserved': hoursReserved 
        };
        
        return(
            <Request
                url={process.env.REACT_APP_JIRA_API_URL + '/customer/status'}
                method='post'
                accept='application/json'
                send={request}
                type='application/json'
                verbose={true}>
                {
                    ({error, result, loading}) => {
                        if (loading) {
                            return <CustomerLoaderCardComponent />;
                        } else if(error) {
                            console.log(error);
                            return <CustomerErrorCardComponent error={error} />;
                        } else {
                            return <CustomerStatusCardComponent customer={customer} customerStatus={result.body} />;
                        }
                    }
                }
            </Request>
        )
    }
}

export default CustomerStatusComponent;
