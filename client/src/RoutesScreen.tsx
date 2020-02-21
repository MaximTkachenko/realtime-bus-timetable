import React, { Component } from 'react';
import { Root } from './Metadata';

type RoutesScreenProps = { metadata: Root | null };
type RoutesScreenState = { };

class RoutesScreen extends Component<RoutesScreenProps, RoutesScreenState> {
    componentDidMount() {
        let ev = new CustomEvent('routesReady', {'detail': this.props.metadata});
        document.dispatchEvent(ev);
    }

    render () {    
        return ( 
            <div id="routes"></div>            
        );
    };
}

export default RoutesScreen;