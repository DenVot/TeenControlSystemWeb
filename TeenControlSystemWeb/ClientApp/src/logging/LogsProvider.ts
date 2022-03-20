import constants from "../constants";

const EventEmitter = require("events");

function generateUUID() {
    let d = new Date().getTime();
    let d2 = ((typeof performance !== 'undefined') && performance.now && (performance.now() * 1000)) || 0;
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
        let r = Math.random() * 16;
        if(d > 0){
            r = (d + r)%16 | 0;
            d = Math.floor(d/16);
        } else {
            r = (d2 + r)%16 | 0;
            d2 = Math.floor(d2/16);
        }
        return (c === 'x' ? r : (r & 0x3 | 0x8)).toString(16);
    });
}

interface LogInfo {
    text: string,
    type: LogType
}

enum LogType {
    Error,
    Info,
    Warning,
    Success
}

export class LogsProvider extends EventEmitter {
    constructor() {
        super();
        this.queue = [];
    }
    
    queue:Log[];
    
    private _raise(info: LogInfo) {
        this.queue.push(LogsProvider._convertToLog(info));
        this.emit(constants.events.raiseMsg, this.queue);
    }
    
    info(text: string) {
        this._raise({
            text: text,
            type: LogType.Info
        });
    }
    
    warn(text: string) {
        this._raise({
            text: text,
            type: LogType.Warning
        });
    }
    
    error(text: string) {
        this._raise({
            text: text,
            type: LogType.Error 
        });
    }
    
    success(text: string) {
        this._raise({
            text: text,
            type: LogType.Success
        });
    }
    
    private static _convertToLog(info: LogInfo) : Log {
        return {
            id: generateUUID(),
            logType: info.type,
            text: info.text
        };
    }
    
    addUpdateListener(callback: void) {
        this.addListener("raise", callback);
    }
    
    removeUpdateListener(callback: void) {
        this.removeListener("raise", callback);
    }
}

class Log {
    constructor(id: string, logType: LogType, text: string) {
        this.id = id;
        this.logType = logType;
        this.text = text;
    }
    
    id: string
    logType: LogType
    text: string
}

export default new LogsProvider();