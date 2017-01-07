import React, { Component } from 'react';
import { Button, Container, Form, Grid, Header, Input, Message, Segment } from 'semantic-ui-react';
import { browserHistory } from 'react-router';
import Auth from '../Auth';

const startPage = '/dashboard';

class LoginComponent extends Component {
    constructor(props) {
        super(props);

        this.state = {
            showLoginFailedMessage: false,
            loginFailedMessage: ''
        };

        this.login = this.login.bind(this);
    }

    login(e, formData) {
        let self = this;

        Auth.authenticate(e, formData).then(function(loginStatus) {
            if(loginStatus.loggedIn === true) {
                self.setState({
                    showLoginFailedMessage: false,
                    loginFailedMessage: ''
                });
                browserHistory.push(startPage);
            } else {
                self.setState({
                    showLoginFailedMessage: true,
                    loginFailedMessage: loginStatus.message
                });
            }
        });
    }

    render() {
        return (
            <div>
                <Container textAlign='center'>
                    <Grid columns='equal'>
                        <Grid.Column></Grid.Column>
                        <Grid.Column width={8}>
                            <Segment textAlign='left'>
                                <Segment basic={true} textAlign='center'>
                                    <Header>Login with your Jira account</Header>
                                </Segment>
                                <Form onSubmit={this.login}>
                                    <Form.Field>
                                        <label>Username</label>
                                        <Input name="username" placeholder='Username' />
                                    </Form.Field>
                                    <Form.Field>
                                        <label>Password</label>
                                        <Input name="password" type="password" placeholder='Password' />
                                    </Form.Field>
                                    <Segment basic={true} textAlign='right'>
                                        <Button type='submit' primary>Login</Button>
                                    </Segment>
                                    <Segment basic={true} textAlign='center' style={this.state.showLoginFailedMessage === true ? {} : { display: 'none' }}>
                                        <Message negative>
                                            <Message.Header>{this.state.loginFailedMessage}</Message.Header>
                                        </Message>
                                    </Segment>
                                </Form>
                            </Segment>
                        </Grid.Column>
                        <Grid.Column></Grid.Column>
                    </Grid>
                </Container>
            </div>
        );
    }
}

export default LoginComponent;