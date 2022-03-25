import {Loader} from "./Loader";
import {createUseStyles} from "react-jss";

const useStyles = createUseStyles({
    loadingFrame: {
        position: "absolute",
        width: "100%",
        height: "100%",
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
        background: "rgba(139, 139, 139, 0.3)",
        top: 0,
        left: 0
    }
});

export function LoadingFrame() {
    const styles = useStyles();
    
    return <div className={styles.loadingFrame}>
        <Loader/>
    </div>
}
