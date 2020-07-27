import React from 'react';
import './styles.css';
import Nui from '../../util/Nui';

class App extends React.Component {
  componentWillMount() {
    window.addEventListener('message', this.handleEvent);
  }

  componentWillUnmount() {
    window.removeEventListener('message', this.handleEvent);
  }

  handleEvent = event => {
    // Handle event.data
  };

  render() {
    return (<h1>Ilha da Magia RPG</h1>)
  }
}

export default App;
