import React, {useEffect, useState} from 'react';
import {Redirect, Route, Switch} from 'react-router-dom';
import { Layout } from './components/Layout';
import {Login} from "./pages/Login";
import {LoaderEventFrame} from "./components/LoaderEventFrame";
import {Home} from "./pages/Home";
import constants from "./constants";
import {getCachedUser} from "./providers/ApiProvider";
import {LoadingFrame} from "./components/LoadingFrame";

export default function App() {
    const [defInst, setInst] = useState(undefined);
    const [defInstProc, setInProc] = useState(true);
    const [redirect, setRedirect] = useState(false)

    useEffect(() => {
        async function fetchCache() {
            try {
                if (defInst == null && window.location.pathname !== constants.routes.login) {
                    setInst(await getCachedUser());
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
            <Switch>
                <Route path={"/login"}>
                    <Login/>
                </Route>
                <Route path={"/home"}>
                    <Home/>
                </Route>
            </Switch>
            <LoaderEventFrame/>
        </Layout>
    );
}
