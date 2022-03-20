import LogsProvider from "./LogsProvider";
import {useState} from "react";

function LogsContainer() {
    const [logs, setLogs] = useState([]);
    
    LogsProvider.addUpdateListener((logsUpdate) => {
        setLogs(logsUpdate);
    })
    
    //TODO End this feature
}