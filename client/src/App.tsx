import React, { Component } from 'react';
import './App.css';

type AppProps = {  };
type AppState = { connected: boolean, server: string, error: boolean, metadata: any };

class App extends Component<AppProps, AppState> {
  constructor(props: AppProps){
    super(props);
    this.state = {connected: false, server: '', error: false, metadata: null};
  }

  handleClick(){
    console.log(this.state);
       fetch(this.state.server + '/metadata')
        .then(response => response.json())
        .then(response =>
          this.setState({ 
          connected: true,
          server: this.state.server,
          error: false,
          metadata: response
        }))
        .catch(error => this.setState({ 
          connected: false,
          server: this.state.server,
          error: true,
          metadata: null
        }));
  }
  
  updateHost(e: any){
    this.state = {connected: false, server: e.target.value, error: false, metadata: null};
  }
  
  render(){    
    return (
      <div className="App">
        <header className="App-header">
        <input type="text" onChange={this.updateHost.bind(this)}></input>
        <button onClick={this.handleClick.bind(this)}>go</button>
        </header>
      </div>
    );
  }
}

export default App;
