import React from 'react';
import 'whatwg-fetch';
import { Router, Route, IndexRedirect } from 'react-router';
import Auth from './Auth.js';

import AppComponent from './components/AppComponent';
import DashboardComponent from './components/DashboardComponent';
import SettingsComponent from './components/SettingsComponent';
import AboutComponent from './components/AboutComponent';
import LoginComponent from './components/LoginComponent';
import NotFoundComponent from './components/NotFoundComponent';

const Routes = (props) => (
    <Router {...props}>
        <Route path='/' component={AppComponent}>
            <IndexRedirect to='/dashboard' />
            <Route path='/dashboard' component={DashboardComponent} onEnter={Auth.requireAuth} />
            <Route path='/settings' component={SettingsComponent} onEnter={Auth.requireAuth} />
            <Route path='/about' component={AboutComponent} onEnter={Auth.requireAuth} />
        </Route>
        <Route path='/login' component={LoginComponent} />
        <Route path='*' component={NotFoundComponent} />
    </Router>
);

export default Routes;