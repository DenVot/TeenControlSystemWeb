import {createUseStyles} from "react-jss";

const useStyles = createUseStyles({
    brandInput: {
        background: "transparent",
        border: "2px solid var(--default-blue)",
        color: "#000",
        padding: "10px",
        borderRadius: "7px",
        margin: 5,
        transition: "border-color 0.3s",
        outline: "none",
        "&:focus": {
            borderColor: "var(--default-blue-dark)"
        }
    }
});

export function BrandInput({type, placeholder, props, onChange}) {
    const styles = useStyles();
    
    return <input type={type} placeholder={placeholder} className={styles.brandInput} {...props} onChange={onChange}/>
}