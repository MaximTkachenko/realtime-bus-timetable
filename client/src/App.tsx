import React, { Component } from 'react';
import RoutesScreen from './RoutesScreen';
import { Root } from './Metadata';
import Timetable from './Timetable';
import * as rb from 'react-bootstrap';

type AppProps = {  };
type AppState = { connected: boolean, server: string, error: boolean, metadata: Root | null };

class App extends Component<AppProps, AppState> {
  constructor(props: AppProps){
    super(props);
    this.state = {connected: false, server: '', error: false, metadata: null};
    this.handleClick = this.handleClick.bind(this);
    this.updateHost = this.updateHost.bind(this);
    this.hostKeyPress = this.hostKeyPress.bind(this);
  }

  handleClick(){
    fetch(this.state.server + '/metadata')
    .then(response => response.json())
    .then(response =>
      this.setState({ 
        connected: true,
        error: false,
        metadata: response
      }))
    .catch(error => this.setState({ 
      connected: false,
      error: true,
      metadata: null
    }));
  }
  
  updateHost(e: any){
    this.setState({server: e.target.value});
  }

  hostKeyPress(e: any){
    if(e.charCode === 13){
      this.handleClick();
    }
  }
  
  render(){    
    const connected = this.state.connected;
    let screen;

    if(!connected){
      const style = {
        paddingTop: '10%'
      };

      screen = 
        <rb.Container style={style}>
          <rb.Row className="justify-content-md-center">      
            <rb.Col md="auto">
              <rb.InputGroup className="mb-3">
                <rb.FormControl placeholder="Host" aria-label="Host" aria-describedby="basic-addon2" onKeyPress={this.hostKeyPress} onChange={this.updateHost}/>
                <rb.InputGroup.Append>
                  <rb.Button onClick={this.handleClick}>Go</rb.Button>
                </rb.InputGroup.Append>
              </rb.InputGroup>
            </rb.Col>
          </rb.Row>      
        </rb.Container>;
    }
    else {
      screen =         
        <rb.Container fluid>
          <rb.Row>      
            <rb.Col md="auto">
              <RoutesScreen metadata={this.state.metadata} server={this.state.server} />
            </rb.Col>            
            <rb.Col md="auto">
              <Timetable server={this.state.server} />
            </rb.Col>            
          </rb.Row>      
        </rb.Container>;
    }

    return screen;
  }
}

export default App;
