import React, {useEffect, useState} from "react";
import {LoadingFrame} from "./LoadingFrame";
const LoadingProvider = require("../providers/LoadingProvider").default;

export function LoaderEventFrame () {
    const [isLoading, setLoading] = useState(false);
    
    useEffect(() =>{
        LoadingProvider.addLoadingListener(onLoadingStateChanged)
    }, []);
    
    function onLoadingStateChanged(loading) {
        setLoading(loading);
    }
    
    return isLoading ? <LoadingFrame/> : null;
}