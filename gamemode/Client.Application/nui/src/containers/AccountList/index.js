import React, { Component } from 'react';
import './styles.css';
import Nui from '../../util/Nui';

class AccountList extends Component {
    constructor(props) {
        super(props);
        this.state = {
            accounts: []
        };
    }

    updateAccounts(newAccountList) {
        let mappedAccountList = newAccountList.map((element) => {
            return element.Username;
        });
        this.setState({ accounts: mappedAccountList });
    }

    handleAccountSelect(account) {
        Nui.send('NUI_ENDPOINT', { type: 'RESPONSE_ACCOUNT_SELECTED', account });
    }

    renderAccount(account) {
        let functionHandle = this.handleAccountSelect;
        return (
            <div className="account" key={account}>
                <li>{account}</li>
                <button onClick={(e) => functionHandle(account)}>Selecionar</button>
            </div>
        )
    }

    render() {
        return (
            <div className="container">
                <h1>Escolha sua conta</h1>
                <ul>
                    {this.state.accounts.map(this.renderAccount.bind(this))}
                </ul>
                <button onClick={(e) => this.handleAccountSelect(e)}>Selecionards</button>
            </div>
        )
    }
}

export default AccountList;