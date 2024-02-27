import { Route, Routes } from 'react-router-dom'
import React, {Component} from 'react'

import Home from './Pages/Home'
import NotFound from './Pages/NotFound'

class App extends Component {
  
  render() {
    return (
      <div className="App">
        <Routes>
            <Route exact path="/" element={<Home/>}/>
            <Route path='*' exact={true} element={<NotFound/>}/>
        </Routes>
      </div>
    );
  }
}

export default App;
