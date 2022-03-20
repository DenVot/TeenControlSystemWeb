import {Loader} from "./Loader";
import style from "../css/loading-frame.module.css";

export function LoadingFrame() {
    return <div className={style.loadingFrame}>
        <Loader/>
    </div>
}
