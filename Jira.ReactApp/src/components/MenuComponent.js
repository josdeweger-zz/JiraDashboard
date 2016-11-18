import React, { Component } from 'react';
import { Link } from 'react-router';
import { Menu } from 'semantic-ui-react';
import MenuItemComponent from './MenuItemComponent';
    
class MenuComponent extends Component {
    render() {
        return (
            <Menu pointing secondary>
                <MenuItemComponent as={Link} icon='dashboard' name='dashboard' to="/dashboard" />
                <MenuItemComponent as={Link} icon='settings' name='settings' to="/settings" />
                <MenuItemComponent as={Link} icon='info circle' name='about' to="/about" />
            </Menu>
        )
    }
}

export default MenuComponent;