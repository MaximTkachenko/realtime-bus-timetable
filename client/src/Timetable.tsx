import React, { Component } from 'react';

type TimetableProps = { server: string };
type TimetableState = { busStopId: string, items: TimetableItem[], intervalId: NodeJS.Timeout | null };
type TimetableItem = { routeId: string, msBeforeArrival: number };

class Timetable extends Component<TimetableProps, TimetableState> {
    constructor(props: TimetableProps){
      super(props);
      this.state = {busStopId: '', items: [], intervalId: null};
    }

    componentDidMount() {
        document.addEventListener('busStopSelected', (e: any) => {this.selectBusStop(e.detail);});
    }
  
    selectBusStop(busStopId: string) {
        this.setState({busStopId: busStopId});
        if(this.state.intervalId != null){
            clearInterval(this.state.intervalId);
        }

        const intervalId = setInterval(async () => {
            fetch(this.props.server + '/' + this.state.busStopId + '/timetable')
            .then(response => response.json())
            .then(response =>
            this.setState({ 
                items: response
            }))
            .catch(error => this.setState({ 
                items: []
            }));
        }, 500);

        this.setState({intervalId: intervalId});
    }

    render(){    
        const listItems = this.state.items.map((item) =>
            <li key={item.routeId}>{item.routeId}: {item.msBeforeArrival}</li> 
        );
        return (
        <div>
            <b>{this.state.busStopId}</b>
            <ul>
            {listItems}
            </ul>
        </div>
        );
    }
}

export default Timetable;
