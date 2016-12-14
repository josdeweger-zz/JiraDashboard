import React, { Component } from 'react';
import moment from 'moment';
import HeaderComponent from './HeaderComponent';
import CustomerStatusComponent from './CustomerStatusComponent';
import {Card, Segment} from 'semantic-ui-react';
import find from 'lodash/find';
import map from 'lodash/map';

class CustomerStatusCardsComponent extends Component {
    constructor(props) {
        super(props);

        this.state = {date: moment()};

        this.handleDateChanged = this.handleDateChanged.bind(this);
    }

    handleDateChanged(date) {
        this.setState({date: date});
    }

    render() {
        let {settings} = this.props;
        let date = this.state.date;
        let firstSprintStart = moment(settings.sprints[0].start, "YYYY-MM-DD");
        let dateRange = {min: firstSprintStart, max: moment()};

        let sprints = map(settings.sprints, function(sprint) {
            sprint.start = moment(sprint.start, "YYYY-MM-DD");
            sprint.end = moment(sprint.end, "YYYY-MM-DD");

            return sprint;
        });

        let selectedSprint = find(sprints, function(sprint) {
            return sprint.start.startOf('day') <= date && sprint.end.endOf('day') >= date;
        });
        
        return (             
            <div>
                <HeaderComponent 
                    date={date}
                    dateRange={dateRange}
                    selectedSprint={selectedSprint}
                    handleDateChanged={this.handleDateChanged} />
                <Segment>
                    <Card.Group>
                        {
                            settings.customers.map(function(customer, index) {
                                let reservation = find(selectedSprint.reservations, function(reservation) {
                                    return reservation.customerId === customer.customerId;
                                });

                                if(reservation) {
                                    return <CustomerStatusComponent 
                                            teamId={settings.teamId}
                                            key={index} 
                                            customer={customer} 
                                            date={date} 
                                            selectedSprint={selectedSprint}
                                            hoursReserved={reservation.hoursReserved} />
                                } else {
                                    return '';
                                }
                            })
                        }
                    </Card.Group>
                </Segment>
            </div>
        );
    }
}

export default CustomerStatusCardsComponent;
