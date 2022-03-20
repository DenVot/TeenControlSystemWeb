import constants from "../constants";

const EventEmitter = require("events");

class LoadingProvider extends EventEmitter {
    public loading: boolean;
    
    constructor() {
        super();
        this.loading = false;
    }
    
    setLoading(state: boolean) {
        this.loading = state;
        this.emit(constants.events.loadingStateChanged, this.loading);
    }
    
    addLoadingListener(callback: void) {
        this.addListener(constants.events.loadingStateChanged, callback);
    }
    
    removeLoadingListener(callback: void) {
        this.removeListener(callback);
    }
}

export default new LoadingProvider();