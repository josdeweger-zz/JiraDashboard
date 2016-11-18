import React, { Component } from 'react';
import {Doughnut as DoughnutChart} from 'react-chartjs';
import {Card, Table, Label} from 'semantic-ui-react';

let chartColorArray = [];
chartColorArray["positive"] = { color:  "#00cc00", highlight: "#00CC66" };
chartColorArray["neutral"] = { color:  "#00cc00", highlight: "#00CC66" };
chartColorArray["negative"] = { color:  "#F7464A", highlight: "#FF5A5E" };

class CustomerStatusCardComponent extends Component {
    getChartData(customerStatus, loggedHoursValue) {
        let percentageDone = customerStatus.percentage > 100 
                                ? 100 
                                : customerStatus.percentage;
        let percentageToDo = 100 - percentageDone;
        
        return [
            {
                value: percentageDone,
                color: chartColorArray[loggedHoursValue].color,
                highlight: chartColorArray[loggedHoursValue].highlight,
                label: "% done"
            },
            {
                value: percentageToDo,
                color: "#DDD",
                highlight: "#EEE",
                label: "% of hours left"
            }
        ];
    }

    render() {
        let {customerStatus, customer} = this.props;
        let loggedHoursValue = customerStatus.loggedHoursValue === 2 ? "negative" : "positive";
        let chartData = this.getChartData(customerStatus, loggedHoursValue);

        return(
            <Card className="customer-status-card">
                <div className="chart center aligned">
                    <div className="percentage">
                        <h2>{customerStatus.percentage} %</h2>
                    </div>
                    <DoughnutChart data={chartData} redraw />
                </div>
                <Card.Content>
                    <Card.Header>
                        {customer.name}
                    </Card.Header>
                    <Card.Description>
                        <Table celled className="center aligned">
                            <Table.Header>
                                <Table.Row>
                                    <Table.HeaderCell>Reserved</Table.HeaderCell>
                                    <Table.HeaderCell>Expected</Table.HeaderCell>
                                    <Table.HeaderCell>Logged</Table.HeaderCell>
                                </Table.Row>
                            </Table.Header>
                            <Table.Body>
                                <Table.Row className={loggedHoursValue}>
                                    <Table.Cell>
                                        <Label><h3>{customerStatus.hoursReserved}</h3></Label>
                                    </Table.Cell>
                                    <Table.Cell>
                                        <Label><h3>{customerStatus.hoursExpected}</h3></Label>
                                    </Table.Cell>
                                    <Table.Cell>
                                        <Label><h3>{customerStatus.totalHours}</h3></Label>
                                    </Table.Cell>
                                </Table.Row>
                            </Table.Body>
                        </Table>
                    </Card.Description>
                </Card.Content>
            </Card>
        )
    }
}

export default CustomerStatusCardComponent