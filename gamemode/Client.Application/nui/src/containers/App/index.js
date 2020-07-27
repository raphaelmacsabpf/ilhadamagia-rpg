import React, { Component } from 'react';
import './styles.css';
import Nui from '../../util/Nui';
import AccountList from '../AccountList';

class App extends Component {
  constructor(props) {
    super(props);
    this.state = {
      viewSelectAccountVisible: false
    }
  }
  componentWillMount() {
    window.addEventListener('message', this.handleEvent);
  }

  componentWillUnmount() {
    window.removeEventListener('message', this.handleEvent);
  }

  handleEvent = event => {
    // Handle event.data
    console.log(JSON.stringify(event.data));
    if(event.data.type == "OPEN_VIEW") {
      this.openView(event.data.viewName);
    }
    else if(event.data.type == "CLOSE_VIEW") {
      this.closeView(event.data.viewName);
    }
  };
  openView(viewName) {
    console.log(`Oppening view ${viewName}`); // TODO: Remover esse log
    if(viewName == 'SELECT_ACCOUNT') {
      this.setState({ viewSelectAccountVisible: true});
    }
  }

  closeView(viewName) {
    console.log(`Closing view ${viewName}`); // TODO: Remover esse log
    if(viewName == 'SELECT_ACCOUNT') {
      this.setState({ viewSelectAccountVisible: false});
    }
  }

  render() {
    return (
      <div>
        <h1>Ilha da Magia RPG</h1>
        { this.state.viewSelectAccountVisible && (<AccountList/>)} 
        
      </div>
    )
  }
}

export default App;