import React, {useEffect, useState} from 'react';
import {Link, Redirect, Route, Switch} from 'react-router-dom';
import { Layout } from './components/Layout';
import {Login} from "./pages/Login";
import {LoaderEventFrame} from "./components/LoaderEventFrame";
import {Home} from "./pages/Home";
import constants from "./constants";
import {getCachedUser} from "./providers/ApiProvider";
import {LoadingFrame} from "./components/LoadingFrame";
import {Sensors} from "./pages/Sensors";
import {UserAuthInfo} from "./UserAuthInfo";
import HomeIcon from "./images/home.svg";
import Sensor from "./images/sensor.svg";
import Users from "./images/users.svg";
import {createUseStyles} from "react-jss";

const useStyles = createUseStyles({
    headerStyle: {
        position: "absolute",
        width: "auto",
        right: 0,
        top: 0,
        zIndex: 10000,
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
    navImage: {
        margin: 10
    },
    avatar: {
        borderRadius: "50%",
        margin: 10
    },
});

export default function App() {
    const [defInst, setInst] = useState(undefined);
    const [defInstProc, setInProc] = useState(true);
    const [redirect, setRedirect] = useState(false);
    const [authDataRec, setAuthDataRec] = useState(false);
    
    useEffect(() => {
        async function fetchCache() {
            try {
                if (defInst == null && window.location.pathname !== constants.routes.login) {
                    setInst(await getCachedUser());
                    setAuthDataRec(true);
                }
            } catch (e) {
                setRedirect(true);
            }
            setInProc(false);
        }

        fetchCache();
    }, [])

    if(redirect && window.location.pathname !== constants.routes.login) {
        return <Redirect to="/login"/>
    }

    if(defInstProc) {
        return <LoadingFrame/>
    }
    
    return (
        <Layout>
            <Route path={["/home", "/sensors"]}>
                <Header/>
            </Route>
            <Switch>
                <Route path={"/login"}>
                    <Login/>
                </Route>
                <Route path={"/home"}>
                    <Home/>
                </Route>
                <Route path={"/sensors"}>
                    <Sensors/>
                </Route>
                <Route path={"*"}>
                    <Redirect to="/login"/>
                </Route>
            </Switch>
            <LoaderEventFrame/>
        </Layout>
    );
}

function Header() {
    const user = UserAuthInfo.userInfo.user;
    const styles = useStyles();
    const icoWidth = 30;

    return <div className={styles.headerStyle}>
        <nav>
            <Link to="/home">
                <img className={styles.navImage} src={HomeIcon} alt="Домой" width={icoWidth}/>
            </Link>
            <Link to="/sensors">
                <img src={Sensor} alt="Маячки" width={icoWidth} className={styles.navImage}/>
            </Link>
            <Link to="/users">
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