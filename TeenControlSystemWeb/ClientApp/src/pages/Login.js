import {UserAuthInfo} from "../UserAuthInfo";
import {Redirect} from "react-router-dom";
import {BrandButton} from "../components/BrandButton";
import {BrandInput} from "../components/BrandInput";
import React, {Component, useRef} from "react";
import initialize from "../providers/ApiProvider";
import LoadingProvider from "../providers/LoadingProvider";

export function Login() {
    if(UserAuthInfo.userInfo != null) {
        return <Redirect to="/home"/>
    }
    
    return <LoginForm/>
}

function LoginForm() {
    const loginRef = useRef();
    const passwordRef = useRef();
    
    const onSubmitHandler = (e) => {
        e.preventDefault();
        async function login() {
            /*const username = loginRef.current.value;
            const password = passwordRef.current.value;*/
            LoadingProvider.setLoading(true);    
        }
        login();
        /*await initialize(username, password);
        
        LoadingProvider.setLoading(false);*/
    }
    
    return <form onSubmit={onSubmitHandler}>
        <div>
            <BrandInput type="text" placeholder="Логин" ref={loginRef}/>
            <BrandInput type="password" placeholder="Пароль" ref={passwordRef}/>    
        </div>
        <div>
            <BrandButton isSubmit={true}>Войти</BrandButton>
        </div>
    </form>
}