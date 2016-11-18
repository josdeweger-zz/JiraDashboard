import React, { Component } from 'react';
import { Menu } from 'semantic-ui-react';
    
class MenuItemComponent extends Component {
    static contextTypes = {
        router: React.PropTypes.object
    }

    render() {
        //const isActive = this.context.router.isActive(this.props.to, this.props.onlyActiveOnIndex);

        return (
            <Menu.Item {...this.props} active={this.context.router.isActive(this.props.to)} />
        )
    }
}

export default MenuItemComponent;