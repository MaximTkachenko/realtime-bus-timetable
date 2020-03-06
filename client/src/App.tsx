import React, { Component } from 'react';
import RoutesScreen from './RoutesScreen';
import { Root } from './Metadata';
import Timetable from './Timetable';
import * as rb from 'react-bootstrap';
import 'react-bootstrap/';

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
      <rb.Container ><rb.Row  className="justify-content-md-center">      <rb.Col  md="auto">
            <rb.InputGroup className="mb-3">
              <rb.FormControl
                placeholder="Host"
                aria-label="Host"
                aria-describedby="basic-addon2" onChange={this.updateHost}
              />
              <rb.InputGroup.Append>
                <rb.Button onClick={this.handleClick}>Go</rb.Button>
              </rb.InputGroup.Append>
            </rb.InputGroup></rb.Col></rb.Row>      
            </rb.Container>;
    }
    else{
      screen = (
        <div>
            <RoutesScreen metadata={this.state.metadata} server={this.state.server} />
            <Timetable server={this.state.server} /></div>
            );
    }

    return screen;
  }
}

export default App;
