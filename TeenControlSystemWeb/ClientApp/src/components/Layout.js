import React, {Component, useEffect, useState} from 'react';
import { Container } from 'reactstrap';
import {getCachedUser} from "../providers/ApiProvider";
import constants from "../constants";
import {Link, Redirect, Route} from "react-router-dom";
import "../style/css/brand.css";
import {LoadingFrame} from "./LoadingFrame";
import {UserAuthInfo} from "../UserAuthInfo";
import {createUseStyles} from "react-jss";
import Sensor from "../images/sensor.svg";
import Users from "../images/users.svg";
import Home from "../images/home.svg";

const useStyles = createUseStyles({
   headerStyle: {
       display: "flex",
       flexDirection: "row",
       justifyContent: "flex-end",
       alignItems: "center",
       "& a": {
           color: "#000",
           textDecoration: "none",
           fontSize: 24,
           fontWeight: "bold",
           "&:hover": {
               textDecoration: "underline"
           }
       },
       height: "10%"
   },
    avatar: {
        borderRadius: "50%",
        margin: 10
    },
    maxLayout: {
        width: "100%", 
        height: "90%"
    },
    navImage: {
       margin: 10
    }
});

export function Layout(props) {
    const styles = useStyles();
    
    return (
        <>
            <Route path={constants.routes.home}>
                <Header/>    
            </Route>
            <Container className={styles.maxLayout}>
                {props.children}    
            </Container>
        </>
    );
}

function Header() {
    const user = UserAuthInfo.userInfo.user;
    const styles = useStyles();
    const icoWidth = 30;
    
    return <div className={styles.headerStyle}>
        <nav>
            <Link to="/home">
                <img className={styles.navImage} src={Home} alt="Домой" width={icoWidth}/>
                <img src={Sensor} alt="Маячки" width={icoWidth} className={styles.navImage}/>
                {user.isAdmin && <img src={Users} alt="Пользователи" width={icoWidth} className={styles.navImage}/>}
            </Link>
        </nav>
        <div>
            <Link to="/profile">
                {user.username}
            </Link>
            <img className={styles.avatar} src={"/api/media/def-avatar?id=" + user.avatarId} alt="Подождите..."/>
        </div>
    </div>
}