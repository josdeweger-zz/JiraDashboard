import React, { Component } from 'react';
import {Header, Form, Button, Modal, TextArea} from 'semantic-ui-react';
import Config from '../../public/Config.json';
import 'whatwg-fetch';

class SettingsComponent extends Component {
    constructor(props) {
        super(props);

        this.state = { isModalOpen: false, modalMessage: '' };

        this.modalOpen = this.modalOpen.bind(this);
        this.modalClose = this.modalClose.bind(this);
        this.saveJsonSettings = this.saveJsonSettings.bind(this);
    }


    modalOpen = (modalMessage) => this.setState({ 
        isModalOpen: true, 
        modalMessage: modalMessage 
    });

    modalClose = () => this.setState({ isModalOpen: false });

    saveJsonSettings = (e, serializedForm) => {
        console.log(e);
        e.preventDefault();
        let self = this;

        fetch('http://localhost:3001/settings', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: serializedForm.settings
        })
        .then(function(result) {
            console.log('result', result.body);
            self.modalOpen('The settings were successfully saved!');
        })
        .catch(function(ex) {
            console.log('Exception: ', ex)
            self.modalOpen('Woops, something went wrong. Check the exception: ' + JSON.stringify(ex));
        });
    }

    render() {
        let {isModalOpen, modalMessage} = this.state;

        return (
            <div className='settings'>
                <Header>
                    Settings
                </Header>
                <Form onSubmit={this.saveJsonSettings}>
                    <Form.Field>
                        <TextArea name="settings" defaultValue={JSON.stringify(Config, null, 2) } />
                    </Form.Field>
                    <Button primary type='submit' floated='right'>Save Settings</Button>
                </Form>
                <Modal open={isModalOpen} onClose={this.modalClose}>
                    <Modal.Header>
                        Save Settings
                    </Modal.Header>
                    <Modal.Content>
                        <p>{modalMessage}</p>
                    </Modal.Content>
                    <Modal.Actions>
                        <Button positive labelPosition='right' icon='checkmark' content='OK' onClick={this.modalClose} />
                    </Modal.Actions>
                </Modal>
            </div>
        );
    }
}

export default SettingsComponent;