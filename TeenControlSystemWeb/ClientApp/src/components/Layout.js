import React, {Component, useEffect, useState} from 'react';
import { Container } from 'reactstrap';
import {getCachedUser} from "../providers/ApiProvider";
import constants from "../constants";
import {Redirect} from "react-router-dom";
import "../style/css/brand.css";

export function Layout(props) {
    const [defInst, setInst] = useState(undefined);
    
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