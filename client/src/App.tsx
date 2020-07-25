import React, { Component } from 'react';
import RoutesScreen from './RoutesScreen';
import { Root } from './Metadata';
import Timetable from './Timetable';
import * as rb from 'react-bootstrap';

type AppProps = {  };
type AppState = { connected: boolean, server: string, error: boolean, metadata: Root | null, connecting: boolean };

export default class App extends Component<AppProps, AppState> {
  constructor(props: AppProps){
    super(props);
    this.state = {connected: false, server: '', error: false, metadata: null, connecting: false};
    this.connectToServer = this.connectToServer.bind(this);
    this.updateHost = this.updateHost.bind(this);
  }

  connectToServer(e: any){
    this.setState({connecting: true, error: false});
    e.preventDefault();
    
    fetch(this.state.server + '/metadata')
    .then(response => response.json())
    .then(response =>
      this.setState({ 
        connected: true,
        error: false,
        metadata: response,
        connecting: false
      }))
    .catch(error => {
      this.setState({ 
        connected: false,
        error: true,
        metadata: null,
        connecting: false
      })
    });
  }
  
  updateHost(e: any){
    this.setState({server: e.target.value});
  }
  
  render(){  
    if(this.state.connected && this.state.metadata != null){
      const style = {
        paddingLeft: '5px'
      };

      return (       
        <rb.Container fluid style={style}>
          <rb.Row>      
            <rb.Col md="auto">
              <RoutesScreen metadata={this.state.metadata} server={this.state.server} />
            </rb.Col>
            <rb.Col md="auto">
              <Timetable server={this.state.server} metadata={this.state.metadata} />
            </rb.Col>            
          </rb.Row>          
        </rb.Container>
      );
    }

    const style = {
      paddingTop: '10%'
    };

    return (
      <rb.Container style={style}>
        <rb.Row className="justify-content-md-center">      
          <rb.Col md="auto">
            <form onSubmit={this.connectToServer}>
              <rb.InputGroup className="mb-3">
                <rb.FormControl placeholder="Host" aria-label="Host" aria-describedby="basic-addon2" onChange={this.updateHost}/>
                <rb.InputGroup.Append>
                  <rb.Button type="submit" disabled={this.state.connecting}>{this.state.connecting ? "Connecting..." : "Go"}</rb.Button>
                </rb.InputGroup.Append>
              </rb.InputGroup>
            </form>
          </rb.Col>
        </rb.Row>  
        <rb.Row className="justify-content-md-center">      
          <rb.Col md="auto">
            <rb.Alert variant="danger" hidden={!this.state.error}>Unable to connect to server.</rb.Alert>
          </rb.Col>
        </rb.Row>     
      </rb.Container>
    );
  }
}