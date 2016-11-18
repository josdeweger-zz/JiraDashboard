import React from 'react';
import { Router, Route, IndexRoute } from 'react-router';

import AppComponent from './components/AppComponent';
import DashboardComponent from './components/DashboardComponent';
import SettingsComponent from './components/SettingsComponent';
import AboutComponent from './components/AboutComponent';
import NotFoundComponent from './components/NotFoundComponent';

const Routes = (props) => (
  <Router {...props}>
    <Route path="/" component={AppComponent}>
        <IndexRoute component={DashboardComponent}/>
        <Route path="/settings" component={SettingsComponent}/>
        <Route path="/about" component={AboutComponent}/>
    </Route>
    <Route path="*" component={NotFoundComponent} />
  </Router>
);

export default Routes;