import React, { Component } from 'react';
import './App.css';
import RoutesScreen from './RoutesScreen';
import { Root } from './Metadata';
import Button from '@material-ui/core/Button';
import TextField from '@material-ui/core/TextField';

type AppProps = {  };
type AppState = { connected: boolean, server: string, error: boolean, metadata: Root | null };

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
        <div>
            <TextField id="host" label="Host" onChange={this.updateHost} />
            <Button variant="contained" color="primary" onClick={this.handleClick}>go</Button>
        </div>
        </header>
      </div>;
    }
    else{
      screen = <RoutesScreen metadata={this.state.metadata} server={this.state.server} />;
    }

    return screen;
  }
}

export default App;
