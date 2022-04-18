import {BrandInput} from "../components/BrandInput";
import React, {useEffect, useRef, useState} from "react";
import {createUseStyles} from "react-jss";
import sensorIcon from "../images/sensor-icon.png";
import useDefStyles from "../style/jss/defaultStyle";
import SensorDialog from "../providers/sensorPage/SensorDialog"
import classNames from "classnames";
import constants from "../constants";
import {LoadingFrame} from "../components/LoadingFrame";
import {UserAuthInfo} from "../UserAuthInfo";
import {BrandButton} from "../components/BrandButton";
const EventEmitter = require("events");

const useStyles = createUseStyles({
    mainHeader: {
       textAlign: "center"
    },
    searchInput: {
        width: "80%"
    },
    sensorContainer: {
        width: 300,
        height: 400,
        margin: 25,
        zIndex: 10000,
        transition: "transform 0.3s",
        cursor: "pointer",
        "&::before": {
            content: '""',
            position: "absolute",
            bottom: 0,
            left: 0,
            height: 20,
            width: 300,
            background: "#EDEDED",
            zIndex: 2,
            borderRadius: [0,0,18,18]
        },
        "&:hover": {
            transform: "translateY(-10px)"
        }
    },
    activeSensorContainer: {
        "&::before": {
            background: "#78F67D"
        }
    },
    disabledSensorContainer: {
        "&::before": {
            background: "#FC8D8D"
        }
    },
    creatingSensorContainer: {
        cursor: "default",
        "&:hover": {
            transform: "translateY(0)"
        },
        "&::before": {
            background: "var(--default-blue)"
        }
    },
    sensorsList: {
        display: "flex",
        flexDirection: "row",
        justifyContent: "space-around",
        flexWrap: "wrap",
        position: "relative"
    },
    sensorContainerInner: {
        display: "flex",
        flexDirection: "column",
        justifyContent: "center",
        alignItems: "center",
        fontSize: 24,
        paddingBottom: 20,
        height: 350
    },
    filterCombo: {
        display: "flex",
        flexDirection: "row",
        alignItems: "center",
        justifyContent: "center",
        "& select": {
            background: "transparent",
            color: "var(--default-blue)",
            border: "2px solid var(--default-blue)",
            borderRadius: 5,
            padding: 5,
            outline: "none"
        }
    },
    sensorDialog: {
        position: "absolute",
        left: 0,
        top: 0,
        width: "100%",
        height: "100%",
        zIndex: 1000000000000,
        background: "rgba(78,78,78,0.45)",
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
    },
    sensorDialogInner: {
        display: "flex",
        flexDirection: "row",
        justifyContent: "space-around",
        alignItems: "center",
        fontSize: 24,
        width: "50%"
    },
    positiveText: {
        color: "#78F67D",
        fontWeight: "bold"
    },
    negativeText: {
        color: "#FC8D8D",
        fontWeight: "bold"
    },
    deleteButtonFirstStage: {
        background: "var(--red)",
        position: "absolute",
        width: 200,
        height: 50,
        borderRadius: 18,
        left: -5,
        top: -5,
        color: "#FFF",
        display: "flex",
        justifyContent: "center",
        alignItems: "center"
    },
    deleteButton: {
        border: "5px solid var(--red)",
        borderRadius: 18, 
        position: "relative",
        width: 200,
        height: 50,
        padding: 10,
        display: "flex",
        alignItems: "center",
        cursor: "pointer"
    },
    mainContainer: {
        paddingTop: 30
    },
    addSenButton: {
        position: "fixed",
        right: 25,
        bottom: 25,
        width: 75,
        height: 75,
        borderRadius: "50%",
        backgroundColor: "var(--default-blue)",
        color: "#FFF",
        fontSize: 36,
        border: "none",
        outline: "none",
        zIndex: 10000000000000000
    },
    addSensInput: {
        width: "98%"
    }
});

export function Sensors() {
    const styles = useStyles();
    const [sens, setSens] = useState(null);
    
    function onFilterChanged(e) {
        switch (e.updateType) {
            case "SEARCH_CHANGED":
                let filtered = sens.filter(element => element.name.toLowerCase().startsWith(e.search.toLowerCase()));
                let arr = filtered;
                
                for (let i in sens) {
                    let el = sens[i]
                    
                    if(!filtered.includes(el)) {
                        arr.push(el);
                    }
                }
                
                setSens(arr);
                break;
        }
    }
    
    function onSensorDelete(id) {
        let arr = []

        for(let i = 0; i < sens.length; i++) {
            let el = sens[i];

            if(el.id === id) continue

            arr.push(el)
        }

        setSens(arr);
    }
    
    function onSensorRenamed(e) {
        let arr = []

        for(let i = 0; i < sens.length; i++) {
            let el = sens[i];

            if(el.id === e.id) {
                el.name = e.name;
            }

            arr.push(el)
        }

        setSens(arr);
    }
    
    function onSensorOrderChanged(e) {
        let arr = []

        if(sens.some(el => el.order == e.order)) return;
        
        for(let i = 0; i < sens.length; i++) {
            let el = sens[i];

            if(el.id === e.id) {
                el.order = e.order;
            }

            arr.push(el)
        }

        setSens(arr);
    }
    
    function onSensAdded(sensorName) {
        let maxId = -1
        let maxOrder = -1;
        for(let i in sens) {
            if(sens[i].id > maxId) maxId = sens[i].id;
            if(sens[i].order > maxOrder) maxOrder = sens[i].order;
        }
        
        let arr = [...sens];
        
        let sensor = {
            id: maxId + 1,
            name: sensorName,
            order: maxOrder + 1,
            online: null,
            activeSession: null
        }

        arr.push(sensor);
        
        setSens(arr);
    }
    
    useEffect(() => {
        async function fetchSensors() {
            setSens(await UserAuthInfo.userInfo.provider.getAllSensors());
        }

        fetchSensors()
    }, []);
    
    if(sens === null) return <LoadingFrame/>
    
    return <div className={styles.mainContainer}>
        <h1 className={styles.mainHeader}>Маячки</h1>
        <FilterFrame onFilterChanged={onFilterChanged}/>
        <SensorsList sensors={sens}/>
        <AddSensorButton onSensAdded={onSensAdded} sensors={sens}/>
        <SensorDialogFrame onSensorDelete={onSensorDelete} onSensorRenamed={onSensorRenamed} onSensorOrderChanged={onSensorOrderChanged}/>
    </div>
}

function FilterFrame({onFilterChanged}) {
    const styles = useStyles();
    const [searchValue, setSearchValue] = useState("");
    const [filterOption, setFilterOption] = useState("by-activity");
    const [orderOption, setOrderOption] = useState("order-by");
    
    function configureResponse(updateType, searchVal, filterVal, orderVal) {
        return {
            updateType: updateType,
            search: searchVal,
            filterOption: filterVal,
            orderOption: orderVal
        };
    }
    
    function onInputValueChanged(e) {
        const value = e.target.value;
        
        setSearchValue(value);
        onFilterChanged(configureResponse("SEARCH_CHANGED", value, filterOption, orderOption));
    }
    
    function filterOptionChanged(e) {
        const value = e.target.value;
        
        setFilterOption(value);
        onFilterChanged(configureResponse("FILTER_CHANGED", searchValue, value, orderOption));
    }
    
    function orderOptionChanged(e) {
        const value = e.target.value;
        
        setOrderOption(value);
        onFilterChanged(configureResponse("ORDER_CHANGED", searchValue, filterOption, value))
    }
    
    return <div className={styles.filterCombo}>
        <BrandInput className={styles.searchInput} placeholder="Поиск" type="search" onChange={onInputValueChanged}/>
    </div>;
}

function AddSensorButton({onSensAdded, sensors}) {
    const [isCreatingDialogInProcess, setDialogProcess] = useState(false);
    const styles = useStyles();
    
    if(isCreatingDialogInProcess) return <AddSensorDialog sensors={sensors} 
                                                        onSkipped={() => setDialogProcess(false)}
                                                          onSensCreated={(e) => { onSensAdded(e); setDialogProcess(false);}}/>
    
    return <button className={styles.addSenButton} onClick={() => setDialogProcess(true)}>
        +
    </button>;
}

function SensorsList({sensors}) {
    const styles = useStyles();
    
    return <div className={styles.sensorsList}>
        {sensors.map(e => <SensorFrame sensor={e} key={e.id.toString()}/>)}
    </div>
}

function SensorFrame({sensor}) {
    const defStyles = useDefStyles();
    const styles = useStyles();
    
    return <div onClick={() => SensorDialog.openDialog(sensor)} className={classNames(styles.sensorContainer,
        defStyles.box,
        sensor.online == null ? null : 
            sensor.online ? styles.activeSensorContainer :
                styles.disabledSensorContainer)}>
        <div className={styles.sensorContainerInner}>
            <span>{sensor.activeSession?.name ?? null}</span>
            <img src={sensorIcon} alt="Маячок" width={230}/>
            <span>{sensor.name} ({sensor.order})</span>    
        </div>
    </div>;
}

function SensorDialogFrame({onSensorDelete, onSensorRenamed, onSensorOrderChanged}) {
    const [dialogSensor, setDialogSensor] = useState(null);
    const styles = useStyles();
    const defStyles = useDefStyles();
    const [isNameEditingMode, setNameEditingMode] = useState(false);
    const [isOrderEditingMode, setOrderEditingMode] = useState(false);
    const mainFrameRef = useRef(null);
    
    useEffect(() => {
        SensorDialog.addOpenDialogListener(onDialogRequested);    
    }, []);

    function onMainFrameClick(e) {
        let target = e.target;
        let isMainClick = false;
        
        while(target !== null) {
            if(target === mainFrameRef.current) {
                isMainClick = true;
                break;
            }
            target = target.parentElement
        }
        if(!isMainClick) {
            setDialogSensor(null);
            setNameEditingMode(false);    
        }
    }
    
    function onDialogRequested(e) {
        setDialogSensor(e);
    }
    
    function onDelete() {
        onSensorDelete(dialogSensor.id);
        UserAuthInfo.userInfo.provider.deleteSensor(dialogSensor.id);
        setDialogSensor(null);
    }
    
    function onNameSubmit(value) {
        setNameEditingMode(false)
        
        if(value !== "" && value !== dialogSensor.name && onSensorRenamed) {
            onSensorRenamed({
                id: dialogSensor.id,
                name: value
            });
            UserAuthInfo.userInfo.provider.editSensorName(dialogSensor.id, value);
        }
        setDialogSensor(null);
    }
    
    function onOrderSubmit(value) {
        setOrderEditingMode(false)

        if(value !== "" && value !== dialogSensor.order && onSensorOrderChanged && value > 0) {
            onSensorOrderChanged({
                id: dialogSensor.id,
                order: value
            });
            UserAuthInfo.userInfo.provider.editSensorOrder(dialogSensor.id, value)
        }
        setDialogSensor(null);
    }
    
    if(dialogSensor === null) return null;
    
    return <div onClick={onMainFrameClick} className={styles.sensorDialog}>
        <div ref={mainFrameRef} className={classNames(defStyles.box, styles.sensorDialogInner)}>
            <img src={sensorIcon} alt="Изображение маячка"/>
            <div>
                <div>
                    <span>Статус: </span>{dialogSensor.online == null ? <span>не занят</span> :<> {dialogSensor.online ?
                    <span className={styles.positiveText}>онлайн</span> :
                    <span className={styles.negativeText}>офлайн</span>} <span>на мероприятии «{dialogSensor.activeSession.name}»</span></>}
                </div>
                <div>
                    <span>Владелец: </span>
                    {!isNameEditingMode ? <span onDoubleClick={() => setNameEditingMode(true)}>{dialogSensor.name}</span> :
                        <BrandInput onSubmit={onNameSubmit} placeholder={dialogSensor.name}/> }
                </div>
                <div>
                    <span>Порядковый номер: </span>
                    {!isOrderEditingMode ? <span onDoubleClick={() => setOrderEditingMode(true)}>{dialogSensor.order}</span> :
                        <BrandInput type="number" onSubmit={onOrderSubmit} placeholder={dialogSensor.order}/> }
                </div>
                <DeleteButton onDelete={onDelete}/>
            </div>
        </div>
    </div>;
}

function AddSensorDialog({onSensCreated, onSkipped, sensors}) {
    const styles = useStyles()
    const defStyles = useDefStyles()
    const mainFrameRef = useRef();
    const [error, setError] = useState("");
    const [mac, setMac] = useState("");
    const [name, setName] = useState("");
    
    function onMainFrameClick(e) {
        let target = e.target;
        let isMainClick = false;

        while(target !== null) {
            if(target === mainFrameRef.current) {
                isMainClick = true;
                break;
            }
            target = target.parentElement
        }
        
        if(!isMainClick) {
            onSkipped();
        }
    }
    
    function onSubmit(e) {
        if(e.preventDefault) e.preventDefault();
        const macRegex = /[\\dA-F][\\dA-F]((:[\\dA-F][\\dA-F]){5})/;
        
        let macUpper = mac.toUpperCase();
        
        if(macUpper === "") {
            setError("MAC адрес обязателен");
            return;
        }
        
        if(name === "") {
            setError("Имя маячка обязательно");
            return;
        }
        
        if(!macRegex.test(macUpper)) {
            setError("MAC адрес маячка должен быть в формате XX:XX:XX:XX:XX:XX");
            return;
        } 
        
        if(sensors.some(sensor => sensor.mac === macUpper)) {
            setError(`Маячок с MAC адресом ${macUpper} уже существует`);
            return;
        }
        
        UserAuthInfo.userInfo.provider.addSensor(macUpper, name, null);
        
        onSensCreated(name);
    }
    
    return <div ref={mainFrameRef} onClick={onMainFrameClick} className={styles.sensorDialog}> 
        <div className={classNames(styles.sensorContainer, defStyles.box, styles.creatingSensorContainer)}>
            <form className={styles.sensorContainerInner} onSubmit={onSubmit}>
                <span>{error}</span>
                <BrandInput onChange={(e) => setName(e.target.value)} 
                    className={styles.addSensInput} placeholder="Название маячка" onSubmit={onSubmit}/>
                <BrandInput onChange={(e) => {setMac(e.target.value)}}
                    placeholder="00:00:00:00:00:00" className={styles.addSensInput} onSubmit={onSubmit}/>
                <BrandButton isSubmit={true} text="Создать"/>
            </form>
        </div>
    </div>;
}

function DeleteButton({onDelete}) {
    const styles = useStyles()
    const delButRef = useRef(null);
    const [isOpened, setOpened] = useState(false);
    
    function onClick() {
        if(isOpened) {
            delButRef.current.animate([
                { transform: 'translateX(0px)' },
            ], {duration: 500, fill: "forwards"})
            setOpened(false);
            if(onDelete)
                onDelete();
            
            return;
        }
        
        delButRef.current.animate([
            { transform: 'translateX(0px)' },
            { transform: 'translateX(190px)' }
        ], {duration: 500, fill: "forwards"})
        setTimeout(() => {
            delButRef.current.animate([
                { transform: 'translateX(190px)' },
                { transform: 'translateX(0px)' }
            ], {duration: 10000, fill: "forwards"});
            
            setOpened(true);
            setTimeout(() => {
                setOpened(false);
            }, 9000);
        }, 500);
    }
    
    return <div onClick={onClick} className={styles.deleteButton}>
        <div className={styles.deleteButtonFirstStage} ref={delButRef}>
            <span>Удалить</span>
        </div>
        <div>
            <span>Вы уверены?</span>
        </div>
    </div>
}
