import EventEmitter from "events";
import constants from "../../constants";
import {Sensor} from "../../types/ApiTypes";

interface SensorDialogResult {
    updateType: string,
    search: string | null,
    filterOption: string | null,
    orderOption: string | null,
    sensorId: number | null,
    newSensorName: string | null
}

export class SensorDialog extends EventEmitter {
    openDialog(sensor: Sensor) {
        this.emit(constants.events.raiseSensDialog, sensor);
    }

    setDialogResult(result: SensorDialogResult) {
        this.emit(constants.events.sensDialogResult, result);
    }

    addDialogResultListener(callback: (e: SensorDialogResult) => void) {
        this.addListener(constants.events.sensDialogResult, callback);
    }

    removeDialogResultListener(callback: (e: SensorDialogResult) => void) {
        this.removeListener(constants.events.sensDialogResult, callback);
    }

    addOpenDialogListener(callback: (e: Sensor) => void) {
        this.addListener(constants.events.raiseSensDialog, callback);
    }

    removeOpenDialogListener(callback: (e: Sensor) => void) {
        this.removeListener(constants.events.raiseSensDialog, callback);
    }
}

export default new SensorDialog();