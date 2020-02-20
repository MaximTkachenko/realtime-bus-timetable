import React from 'react';

const WelcomeScreen = (props: any) => {
    return (
        <div>
            <input type="text" onChange={props.onChange}></input>
            <button onClick={props.onClick}>go</button>
        </div>
    );
}

export default WelcomeScreen;