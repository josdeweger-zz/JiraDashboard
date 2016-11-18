import React, { Component } from 'react';
import { Link } from 'react-router';
import { Menu } from 'semantic-ui-react';
    
class MenuComponent extends Component {
    state = { activeItem: 'dashboard' };

    handleItemClick = (e, { name }) => {
        console.log(name);
        this.setState({ activeItem: name });
    }

    render() {
        const { activeItem } = this.state;

        return (
            <Menu pointing secondary>
                <Menu.Item as={Link} icon='dashboard' name='dashboard' to="/" active={activeItem === 'dashboard'} onClick={this.handleItemClick} />
                <Menu.Item as={Link} icon='settings' name='settings' to="/settings" active={activeItem === 'settings'} onClick={this.handleItemClick} />
                <Menu.Item as={Link} icon='info circle' name='about' to="/about" active={activeItem === 'about'} onClick={this.handleItemClick} />
            </Menu>
        )
    }
}

export default MenuComponent;