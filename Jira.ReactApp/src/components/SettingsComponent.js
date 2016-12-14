import React, { Component } from 'react';
import Request from 'react-http-request';
import {Header, Form, Button, Modal, TextArea} from 'semantic-ui-react';
import 'whatwg-fetch';

class SettingsComponent extends Component {
    constructor(props) {
        super(props);

        this.state = { 
            isModalOpen: false, 
            modalMessage: '' 
        };

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

        fetch(process.env.REACT_APP_JIRA_NODE_SERVER_SETTINGS_URL, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: serializedForm.settings
        })
        .then(function(result) {
            self.modalOpen('The settings were successfully saved!');
        })
        .catch(function(ex) {
            console.log('Exception: ', ex)
            self.modalOpen('Woops, something went wrong saving the settings. Check the exception: ' + JSON.stringify(ex));
        });
    }

    render() {
        let {isModalOpen, modalMessage} = this.state;
        let jiraNodeServerSettingsUrl = process.env.REACT_APP_JIRA_NODE_SERVER_SETTINGS_URL;

        return (
            <div className='settings'>
                <Header>
                    Settings
                </Header>
                <Form onSubmit={this.saveJsonSettings}>
                    <Form.Field>
                        <Request
                            url={jiraNodeServerSettingsUrl}
                            method='get'
                            accept='application/json'
                            verbose={true}>
                            {
                                ({error, result, loading}) => {
                                    if (loading) {
                                        return <div>Loading...</div>;
                                    } else if(error) {
                                        console.error(error);
                                    } else {
                                        return <TextArea name="settings" defaultValue={JSON.stringify(result.body, null, 2) } />;
                                    }
                                }
                            }
                        </Request>
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