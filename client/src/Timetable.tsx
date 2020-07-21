import React, { Component } from 'react';
import { Root } from './Metadata';
import * as rb from 'react-bootstrap';

type TimetableProps = { server: string, metadata: Root };
type TimetableState = { busStopId: string, items: TimetableItem[], intervalId: NodeJS.Timeout | null, error: boolean, getMode: number };
type TimetableItem = { routeId: string, msBeforeArrival: number, direction: number };

export default class Timetable extends Component<TimetableProps, TimetableState> {
    constructor(props: TimetableProps){
      super(props);
      this.state = {busStopId: '', items: [], intervalId: null, error: false, getMode: 1};

      this.selectBusStop = this.selectBusStop.bind(this);
      this.setGetMode = this.setGetMode.bind(this);
    }

    componentDidMount() {
        document.addEventListener('busStopSelected', (e: any) => {this.selectBusStop(e.detail);});
    }
  
    selectBusStop(busStopId: string) {
        this.setState({ items: [], busStopId: '' })
        if(this.state.intervalId != null){
            clearInterval(this.state.intervalId);
        }

        const intervalId = setInterval(async () => {
            const getUrl = this.state.getMode === 1
                ? this.props.server + '/' + busStopId + '/timetable'
                : this.props.server + '/' + busStopId + '/cache/timetable';

            fetch(getUrl)
                .then(response => response.json())
                .then(response => this.setState({ items: response, busStopId: busStopId, error: false }))
            .catch(error => this.setState({items: [], error: true}));
        }, this.props.metadata.updateIntervalMs);

        this.setState({intervalId: intervalId});
    }

    setGetMode(getMode: number){
        this.setState({getMode: getMode});   
    }

    render(){        
        const getModeSelector = (<rb.ToggleButtonGroup type="radio" name="options" defaultValue={1} onChange={this.setGetMode}>
            <rb.ToggleButton value={1}>From grain</rb.ToggleButton>
            <rb.ToggleButton value={2}>From cache</rb.ToggleButton>
        </rb.ToggleButtonGroup>);
        
        if(this.state.error){
            return (
                <div>
                    {getModeSelector}
                    <h4>{this.state.busStopId}</h4>
                    <rb.Alert key='error' variant='danger'>Unable to connect to server.</rb.Alert>
                </div>
            );
        }
        
        const listItems = this.state.items.map((item) =>            
            <rb.Alert key={item.routeId} variant='primary'>
                [{item.direction === 1 ? "THERE" : "BACK"}] {item.routeId}: {item.msBeforeArrival <= this.props.metadata.nowThresholdSec ? 'now' : `${item.msBeforeArrival}sec`}
            </rb.Alert>
        );
        return (
            <div>
                {getModeSelector}
                <h4>{this.state.busStopId}</h4>
                {listItems}
            </div>
        );
    }
}