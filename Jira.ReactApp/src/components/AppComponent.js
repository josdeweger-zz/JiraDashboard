import React, { Component } from 'react';
import MenuComponent from './MenuComponent';
import '../css/App.css';

class AppComponent extends Component {
    render() {
        return (
            <div>
                <MenuComponent />
                {this.props.children}
            </div>
        );
    }
}

export default AppComponent;
