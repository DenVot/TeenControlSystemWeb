export default {
    routes: {
        login: "/login",
        home: "/home"
    },
    events: {
        loadingStateChanged: "LOADING_STATE_CHANGED",
        raiseMsg: "RAISE_MSG",
        raiseSensDialog: "RAISE_SENS_DIALOG",
        sensDialogResult: "SENS_DIALOG_RESULT"
    },
    sensEvents: {
        sensorDelete: "SENSOR_DELETE"
    }
};
