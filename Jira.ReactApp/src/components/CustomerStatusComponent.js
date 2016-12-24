import React, { Component } from 'react';
import Request from 'react-http-request';
import CustomerStatusCardComponent from './CustomerStatusCardComponent';
import CustomerLoaderCardComponent from './CustomerLoaderCardComponent';
import CustomerErrorCardComponent from './CustomerErrorCardComponent';
import map from 'lodash/map';

class CustomerStatusComponent extends Component {
    render() {
        let {customer, teamId, date, selectedSprint, hoursReserved} = this.props;
        let request = { 
            "teamId": teamId,
            "projectKeys": map(customer.projectKeys, 'key'), 
            "date": date.format(), 
            "sprint": { start: selectedSprint.start, end: selectedSprint.end }, 
            "hoursReserved": hoursReserved 
        };
        
        return(
            <Request
                url={process.env.REACT_APP_JIRA_API_CUSTOMER_STATUS_URL}
                method='post'
                accept='application/json'
                send={request}
                verbose={false}>
                {
                    ({error, result, loading}) => {
                        if (loading) {
                            return <CustomerLoaderCardComponent />;
                        } else if(error) {
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
