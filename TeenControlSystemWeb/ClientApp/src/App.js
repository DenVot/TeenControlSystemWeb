import React, {Component, useEffect, useState} from 'react';
import {Redirect, Route, Switch} from 'react-router-dom';
import {    Layout } from './components/Layout';
import {TestPage} from "./pages/TestPage";
import {Login} from "./pages/Login";
import {LoaderEventFrame} from "./components/LoaderEventFrame";

export default function App() {
    return (
        <Layout>
            <Switch>
                <Route path={"/login"}>
                    <Login/>
                </Route>
            </Switch>
            <LoaderEventFrame/>
        </Layout>
    );
}
