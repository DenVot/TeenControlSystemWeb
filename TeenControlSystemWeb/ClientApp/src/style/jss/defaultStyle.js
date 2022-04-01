import {createUseStyles} from "react-jss";

export default createUseStyles({
    box: {
        background: "#FFF",
        borderRadius: "18px",
        position: "relative",
        padding: 30
    },
    centeredBox: {
        background: "#FFF",
        borderRadius: "18px",
        position: "relative",
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
        padding: 30
    },
    link: {
        color: "#10C2E9",
        fontWeight: "bold",
        textDecoration: "none",
        "&:hover": {
            color: "#10C2E9",
            textDecoration: "underline"
        }
    }
});