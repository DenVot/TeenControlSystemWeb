import {BrandInput} from "../components/BrandInput";

const EventEmitter = require("events");
const constants = require("../constants");
const {useState} = require("react");
import React, {useEffect} from "react";

function Sensors() {
    const [sens, setSens] = useState(null);
    
    useEffect(() => {
        new SensorDialog().addDialogResultListener(onSensDialogResultReceived);    
    }, []);
    
    function onFilterChanged(e) {
            
    }
    
    function onSensDialogResultReceived(e) {
        
    }
    
    function onSensAdded(sensor) {
        sens.push(sensor);
        setSens(sens);
    }
    
    return <div>
        <h1>Маячки</h1>
        <AddSensorButton onSensAdded={}/>
        <FilterFrame onFilterChanged={onFilterChanged}/>
        <SensorsList/>
    </div>
}

function FilterFrame({onFilterChanged}) {
    function onInputValueChanged(e) {
        const value = e.target.value;
        
        let ev = {
          updateType: "SEARCH_CHANGE",
          search: value,
          filter: null  
        };
        
        onFilterChanged(ev);
    }
    
    return <div>
        <BrandInput placeholder="Поиск" type="search" onChange={onInputValueChanged}/>
        <div>
            <span>Фильтровать по</span>
            
        </div>
    </div>;
}

function AddSensorButton({onSensAdded}) {
    return <></>;
}

function SensorsList({sensors}) {
    return <div>
        {sensors.map(e => <SensorFrame sensor={e} key={e.id}/>)}
    </div>
}

function SensorFrame({sensor}) {
    return <></>;
}

function SensorDialogFrame() {
    const [dialogSensor, setDialogSensor] = useState(null);
    new SensorDialog().addOpenDialogListener((e) => setDialogSensor(e));
    
    if(dialogSensor == null) return null;
    
    return <>{/*TODO Write some code*/}</>;
}

class SensorDialog extends EventEmitter {
    constructor() {
        super();
        this.sensor = null;
    }
    
    openDialog(sensor) {
        this.emit(constants.events.raiseSensDialog, sensor);
    }
    
    addDialogResultListener(callback) {
        this.addListener(constants.events.sensDialogResult, callback);
    }
    
    removeDialogResultListener(callback) {
        this.removeListener(constants.events.sensDialogResult, callback);
    }
    
    addOpenDialogListener(callback) {
        this.addListener(constants.events.raiseSensDialog, callback);
    }
    
    removeOpenDialogListener(callback) {
        this.removeListener(constants.events.raiseSensDialog, callback);
    }
}