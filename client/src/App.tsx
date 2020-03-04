import React, { Component } from 'react';
import RoutesScreen from './RoutesScreen';
import { Root } from './Metadata';
import Button from '@material-ui/core/Button';
import TextField from '@material-ui/core/TextField';
import FormControl from '@material-ui/core/FormControl';
import Grid from '@material-ui/core/Grid';
import Container from '@material-ui/core/Container';
import Timetable from './Timetable';

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
          <Container maxWidth="sm">
            <Grid container spacing={0} direction="row" justify="center" alignItems="center" style={{ minHeight: '100vh' }}>
              <Grid item xs style={{ textAlign: 'center' }}>
                <FormControl>
                  <TextField id="host" placeholder="Host" onChange={this.updateHost} />
                  <Button variant="contained" color="primary" onClick={this.handleClick}>go</Button>
                </FormControl>
              </Grid>
            </Grid>
          </Container>;
    }
    else{
      screen = (
        <Grid container direction="row" justify="flex-start" alignItems="flex-start" spacing={1}>
          <Grid item xs={9}>
            <RoutesScreen metadata={this.state.metadata} server={this.state.server} />
          </Grid>
          <Grid item xs={3}>
            <Timetable server={this.state.server} />
          </Grid>
        </Grid>);
    }

    return screen;
  }
}

export default App;
