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
        this.setState({ items: [], busStopId: '' })
        if(this.state.intervalId != null){
            clearInterval(this.state.intervalId);
        }

        const intervalId = setInterval(async () => {
            fetch(this.props.server + '/' + busStopId + '/timetable')
                .then(response => response.json())
                .then(response => this.setState({ items: response, busStopId: busStopId }))
            .catch(error => this.setState({ 
                items: []
            }));
        }, 500);

        this.setState({intervalId: intervalId});
    }

    render(){    
        const listItems = this.state.items.map((item) =>            
            <div key={item.routeId} className='alert alert-primary alert-link' role="alert">
                {item.routeId}: {item.msBeforeArrival}
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

export default Timetable;
