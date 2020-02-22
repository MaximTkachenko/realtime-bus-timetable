import React, { Component } from 'react';
import { Root } from './Metadata';

type RoutesScreenProps = { metadata: Root | null, server: string };
type RoutesScreenState = { };

class RoutesScreen extends Component<RoutesScreenProps, RoutesScreenState> {
    componentDidMount() {
        let svgContainerReady = new CustomEvent('routesReady', {'detail': this.props});
        document.dispatchEvent(svgContainerReady);
    }

    render () {    
        return ( 
            <div id="routes"></div>            
        );
    };
}

export default RoutesScreen;