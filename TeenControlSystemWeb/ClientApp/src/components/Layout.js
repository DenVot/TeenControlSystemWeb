import React from 'react';
import { Container } from 'reactstrap';
import "../style/css/brand.css";
import {createUseStyles} from "react-jss";
import {UserAuthInfo} from "../UserAuthInfo";

const useStyles = createUseStyles({
    maxLayout: {
        width: "100%", 
        height: "100%"
    },
});

export function Layout(props) {
    const styles = useStyles();
    
    return (
        <Container className={styles.maxLayout}>
            {props.children}
        </Container>
    );
}
