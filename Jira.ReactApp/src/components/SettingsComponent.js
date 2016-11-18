import React, { Component } from 'react';
import {Header, Form, Button} from 'semantic-ui-react';
import Config from '../../public/Config.json';

class SettingsComponent extends Component {
    render() {
        return (
            <div className='settings'>
                <Header>
                    Settings
                </Header>
                <Form.TextArea defaultValue={JSON.stringify(Config, null, 2) } />
                <Button primary type='submit' floated='right'>Save Settings</Button>
            </div>
        );
    }
}

export default SettingsComponent;