import logo from './logo.svg';
import './App.css';
import {Route, Switch} from 'react-router-dom'
import { Component } from 'react';

class App extends Component {
  
  render() {
    return (
      <div className="App">
        <Switch>
            <Route exact path="/" component={Home}/>
            <Route component={NotFound}/>
        </Switch>
      </div>
    );
  }
}

export default App;
