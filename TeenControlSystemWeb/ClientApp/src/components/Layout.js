import React, {Component, useEffect, useState} from 'react';
import { Container } from 'reactstrap';
import {tryGetDefaultInstance} from "../providers/ApiProvider";
import constants from "../constants";
import {Redirect} from "react-router-dom";

export function Layout(props) {
    const [defInst, setInst] = useState(undefined);
    
    useEffect( async () => {
        try {
            setInst(await tryGetDefaultInstance());
        } catch (e) {
            setInst(null);
        }
    }, []);
    
    if(defInst == null && window.location.pathname !== constants.routes.login) {
        return <Redirect to={constants.routes.login}/>;
    }
    
    return (
        <div>
            <Container>
                {props.children}
            </Container>
        </div>
    );
}