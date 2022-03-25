import {createUseStyles} from "react-jss";
import classNames from "classnames";
import {styles} from "prompts/lib/util/style";

const useStyles = createUseStyles({
    brandButton: {
       background: "var(--default-blue)",
       color: "#FFF",
       padding: {
           left: "50px",
           right: "50px",
           top: "10px",
           bottom: "10px"
       },
       border: "none",
       outline: "none",
       borderRadius: "8px",
       transition: "background-color 0.3s",
       "&:hover": {
           background: "var(--default-blue-dark)"
       },
       fontSize: 24,
       margin: 10
    },
    brandLoading: {
       display: "flex",
       flexDirection: "row",
       alignItems: "center"
    },
    buble: {
        width: 10,
        height: 10,
        margin: 10,
        borderRadius: "50%",
        background: "#FFF",
        animation: "$bubleAnimation 0.5s infinite"
    },
    "@keyframes bubleAnimation": {
        "0%": {
            transform: "scale(1)"
        },
        "50%": {
            transform: "scale(2)"
        },
        "100%": {
            transform: "scale(1)"
        }
    },
    firstBuble: {
       animationDelay: "0s"
    },
    secoundBuble: {
        animationDelay: "0.1s"
    },
    thirdBuble: {
        animationDelay: "0.2s"
    },
});

export function BrandButton({isSubmit, text, className, props, customColor, onClick}) {
    const styles = useStyles();
    
    return isSubmit ? <input className={classNames(styles.brandButton, className)}
                             type="submit" value={text}
                             {...props}
                             style={customColor !== undefined ? {backgroundColor: customColor} : null}
                            onClick={onClick}/> : 
        <button className={classNames(styles.brandButton, className)}
                {...props}
                style={customColor !== undefined ? {backgroundColor: customColor} : null}
                onClick={onClick}>
            {text}
        </button> 
}

export function BrandLoadingButton() {
    const styles = useStyles();
    
    return <div className={classNames(styles.brandButton, styles.brandLoading)}>
        <div className={classNames(styles.buble, styles.firstBuble)}/>
        <div className={classNames(styles.buble, styles.secoundBuble)}/>
        <div className={classNames(styles.buble, styles.thirdBuble)}/>
    </div>
}