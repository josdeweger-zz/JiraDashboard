import React from 'react';
import ReactDOM from 'react-dom';
//import AppComponent from './components/AppComponent';
import { browserHistory } from 'react-router';
import Routes from './Routes';
import './css/index.css';

ReactDOM.render(
  <Routes history={browserHistory} />,
  document.getElementById('root')
);
