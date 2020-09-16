import React, { Component } from 'react';
import './styles.css';
import Nui from '../../util/Nui';

class CreateAccount extends Component {
    constructor(props) {
        super(props);
        this.state = {

        };
    }

    handleAccountCreate(account) {
      Nui.send('NUI_ENDPOINT', { type: 'RESPONSE_ACCOUNT_CREATE', account });
    }

    render () {
      return (
        <div className="container">
          <h1>Criando sua conta</h1>
          <form>
            <input type="text" placeholder="Nick" />
            <input type="password" placeholder="Senha" />
          </form>
        </div>
      )
    }
}

export default CreateAccount;