import React, {useEffect, useState} from "react";
import {LoadingFrame} from "./LoadingFrame";
const LoadingProvider = require("../providers/LoadingProvider").default;

export function LoaderEventFrame () {
    const [isLoading, setLoading] = useState(false);
    
    useEffect(() =>{
        LoadingProvider.addLoadingListener(onLoadingStateChanged)
        alert("Added");
    }, []);
    
    function onLoadingStateChanged(loading) {
        alert("Load")
        alert(loading);
        setLoading(loading);
    }
    
    return isLoading ? <LoadingFrame/> : null;
}