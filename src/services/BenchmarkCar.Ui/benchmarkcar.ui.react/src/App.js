import { Route, Routes } from 'react-router-dom'
import React, {Component} from 'react'

import Home from './Pages/Home'
import NotFound from './Pages/NotFound'
import NavBarLayout from './components/NavBarLayout'

class App extends Component {
  
  render() {
    return (
      <div className="App h-100">
        <Routes>
            <Route exact path="/" element={<NavBarLayout><Home/></NavBarLayout>}/>
            <Route path='*' exact={true} element={<NotFound/>}/>
        </Routes>
      </div>
    );
  }
}

export default App;
