import React, { Component } from 'react';
import MenuComponent from './MenuComponent';
import '../css/App.css';

class AppComponent extends Component {
    render() {
        console.log(this.props.sessionId);

        return (
            <div>
                <MenuComponent />
                {this.props.children}
            </div>
        );
    }
}

export default AppComponent;
