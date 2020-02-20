import React, { Component } from 'react';
import './App.css';
import WelcomeScreen from './WelcomeScreen';
import RoutesScreen from './RoutesScreen';

type AppProps = {  };
type AppState = { connected: boolean, server: string, error: boolean, metadata: any };

class App extends Component<AppProps, AppState> {
  constructor(props: AppProps){
    super(props);
    this.state = {connected: false, server: '', error: false, metadata: null};
    this.handleClick = this.handleClick.bind(this);
    this.updateHost = this.updateHost.bind(this);
  }

  handleClick(){
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
    this.setState({connected: false, server: e.target.value, error: false, metadata: null});
  }
  
  render(){    
    const connected = this.state.connected;
    let screen;

    if(!connected){
      screen = 
      <div className="App">
        <header className="App-header">
        <WelcomeScreen onClick={this.handleClick} onChange={this.updateHost} />
        </header>
      </div>;
    }
    else{
      screen = <RoutesScreen/>;
    }

    return screen;
  }
}

export default App;
