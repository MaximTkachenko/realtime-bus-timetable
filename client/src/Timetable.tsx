import React, { Component } from 'react';

type TimetableProps = {  };
type TimetableState = {  };

class Timetable extends Component<TimetableProps, TimetableState> {
    componentDidMount() {
        document.addEventListener('busStopSelected', (e: any) => {this.selectBusStop(e.detail);});
    }
  
    selectBusStop(busStopId: string) {
        this.setState({busStopId: busStopId});
        console.log(busStopId);
    }

    render(){    
        return <div/>;
    }
}

export default Timetable;
