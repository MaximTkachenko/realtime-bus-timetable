import React, { Component } from 'react';
import { Root } from './Metadata';
import RoutesRenderer from './RoutesRenderer';

type RoutesScreenProps = { metadata: Root, server: string };
type RoutesScreenState = { };

export default class RoutesScreen extends Component<RoutesScreenProps, RoutesScreenState> {
    componentDidMount() {        
        new RoutesRenderer(this.props.metadata, this.props.server);
    }

    render () {    
        return ( 
            <div id="routes"></div>            
        );
    };
}