import classNames from "classnames";
import {UserAuthInfo} from "../UserAuthInfo";
import {Redirect} from "react-router-dom";
import {BrandButton, BrandLoadingButton} from "../components/BrandButton";
import {BrandInput} from "../components/BrandInput";
import React, {useEffect, useState} from "react";
import initialize from "../providers/ApiProvider";
import LoadingProvider from "../providers/LoadingProvider";
import useDefStyle from "../style/jss/defaultStyle";
import {createUseStyles} from "react-jss";
import parents from "../images/ParentsImage.png";
import {getCachedUser, resetCache} from "../providers/ApiProvider";

const useLoginStyle = createUseStyles({
    loginBody: {
        position: "absolute",
        left: 0,
        right: 0,
        top: 0,
        bottom: 0,
        background: "linear-gradient(135deg, #2BD9FF, #BAF3FF)",
        alignItems: "center",
        display: "flex",
        justifyContent: "space-around"
    },
    loginForm: {
        width: 480,
        height: 480,
        display: "flex",
        flexDirection: "column",
        justifyContent: "space-around",
        alignItems: "center"
    },
    usernamePasswordCombo: {
        textAlign: "center",
        display: "flex",
        flexDirection: "column",
        width: "80%",
        "&input": {
            margin: 1
        }
    },
    isU: {
        color: "var(--default-blue)",
        fontSize: 72
    },
    yesIAmNo: {
        display: "flex",
        flexDirection: "column"
    }
});

export function Login() {
    const loginStyle = useLoginStyle();
    
    return <div className={loginStyle.loginBody}>
        <LoginForm/>
        <img src={parents} alt="Родители"/>
    </div>
}

function LoginForm() {
    const defStyle = useDefStyle();
    const loginStyle = useLoginStyle();
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState("");
    const [hasCachedLogin, setCachedLogin] = useState(false);
    const [redirect, setRedirect] = useState(false);
    const [isLoading, setLoading] = useState(false);
    
    useEffect(() => {
        async function defaultLogin() {
            try {
                UserAuthInfo.userInfo = await getCachedUser();
                setCachedLogin(true);
            } catch (e) {
                //Ignore
            }
        }
        
        defaultLogin();
    }, []);
    
    const onSubmitHandler = (e) => {
        e.preventDefault();
        async function login() {
            try {
                UserAuthInfo.userInfo = await initialize(username, password);
                setRedirect(true);
            } catch (e) {
                setError("Неправильный логин или пароль");   
            }
            setLoading(false);
        }
        
        if(username === "") {
            setError("Имя пользователя обязательно");
            return;
        }
        
        if(password === "") {
            setError("Пароль обязателен");
            return;
        }
        setLoading(true);
        login();
    }
    
    function usernameHandler(e) {
        setUsername(e.target.value);        
    }
    
    function passwordHandler(e) {
        setPassword(e.target.value);
    }
    
    if(redirect) {
        return <Redirect to="/home"/>
    }
    
    if(hasCachedLogin) {
        return <div className={classNames(defStyle.box, loginStyle.loginForm)}>
            <h1>Это Вы?</h1>
            <span className={loginStyle.isU}>{UserAuthInfo.userInfo.user.username}</span>
            <div className={loginStyle.yesIAmNo}>
                <BrandButton text="Войти" onClick={() => setRedirect(true)}/>
                <BrandButton text="Нет, это не я" onClick={() => {resetCache(); setRedirect(false); setCachedLogin(false);}} customColor="#FF543D"/>
            </div>
        </div>
    }
    
    return <form className={classNames(defStyle.box, loginStyle.loginForm)} onSubmit={onSubmitHandler}>
        <div className={loginStyle.usernamePasswordCombo}>
            <h1>Вход</h1>
            <span>{error}</span>
            <BrandInput type="text" placeholder="Логин" onChange={usernameHandler} />
            <BrandInput type="password" placeholder="Пароль" onChange={passwordHandler}/>    
        </div>
        {
            isLoading ? <BrandLoadingButton/> : <BrandButton isSubmit={true} text="Войти" /> 
        }
    </form>
}