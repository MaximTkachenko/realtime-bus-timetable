import React, { Component } from 'react';

type TimetableProps = { server: string, nowThresholdSec: number };
type TimetableState = { busStopId: string, items: TimetableItem[], intervalId: NodeJS.Timeout | null, error: boolean };
type TimetableItem = { routeId: string, msBeforeArrival: number };

export default class Timetable extends Component<TimetableProps, TimetableState> {
    constructor(props: TimetableProps){
      super(props);
      this.state = {busStopId: '', items: [], intervalId: null, error: false};
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
            fetch(this.props.server + '/' + busStopId + '/timetable')
                .then(response => response.json())
                .then(response => this.setState({ items: response, busStopId: busStopId, error: false }))
            .catch(error => this.setState({items: [], error: true}));
        }, 500);

        this.setState({intervalId: intervalId});
    }

    render(){
        if(this.state.error){
            return (
                <div>
                    <h4>{this.state.busStopId}</h4>
                    <div key='error' className='alert alert-danger alert-link' role="alert">Unable to connect to server.</div>
                </div>
            );
        }
        
        const listItems = this.state.items.map((item) =>            
            <div key={item.routeId} className='alert alert-primary alert-link' role="alert">
                {item.routeId}: {item.msBeforeArrival <= this.props.nowThresholdSec ? 'now' : `${item.msBeforeArrival}sec`}
            </div>
        );
        return (
            <div>
                <h4>{this.state.busStopId}</h4>
                {listItems}
            </div>
        );
    }
}