import React, { Component } from 'react';
import { Root } from './Metadata';

type RoutesScreenProps = { metadata: Root | null };
type RoutesScreenState = { };

class RoutesScreen extends Component<RoutesScreenProps, RoutesScreenState> {
    render () {  
        console.log(this.props.metadata);      
        return ( 
            <div>
                <div>success</div>
            </div>            
        );
    };
}

export default RoutesScreen;