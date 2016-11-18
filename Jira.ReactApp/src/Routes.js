import React from 'react';
import { Router, Route, IndexRedirect } from 'react-router';

import AppComponent from './components/AppComponent';
import DashboardComponent from './components/DashboardComponent';
import SettingsComponent from './components/SettingsComponent';
import AboutComponent from './components/AboutComponent';
import NotFoundComponent from './components/NotFoundComponent';

const Routes = (props) => (
  <Router {...props}>
    <Route path="/" component={AppComponent}>
        <IndexRedirect to="/dashboard" />
        <Route path='/dashboard' component={DashboardComponent}/>
        <Route path="/settings" component={SettingsComponent}/>
        <Route path="/about" component={AboutComponent}/>
    </Route>
    <Route path="*" component={NotFoundComponent} />
  </Router>
);

export default Routes;