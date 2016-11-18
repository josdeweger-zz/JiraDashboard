import React, { Component } from 'react';
import {Segment, Container} from 'semantic-ui-react';
import DatePicker from 'react-datepicker';
import '../../node_modules/react-datepicker/dist/react-datepicker.min.css';

class HeaderControls extends Component {

    render() {
        let {date, handleDateChanged, dateRange, selectedSprint} = this.props;

        return (
            <div>
                <Container className="center aligned header">
                    <Segment.Group horizontal>
                        <Segment raised style={{width: 50 + '%'}}>
                            <div className="ui input datepicker">
                                <DatePicker
                                    placeholderText="Click to select date"
                                    minDate={dateRange.min}
                                    maxDate={dateRange.max}
                                    dateFormat="DD-MM-YYYY"
                                    todayButton={'Today'}
                                    selected={date}
                                    onChange={handleDateChanged} />
                            </div>
                        </Segment>
                        <Segment raised padded style={{width: 50 + '%'}}>
                            <h2>Sprint: {selectedSprint.start.format("DD-MM-YYYY")}  -  {selectedSprint.end.format("DD-MM-YYYY")}</h2>
                        </Segment>
                    </Segment.Group>
                </Container>
            </div>
        );
    }
}

export default HeaderControls;
