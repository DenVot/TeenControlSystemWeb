import {createUseStyles} from "react-jss";
import classNames from "classnames";
import {useState} from "react";

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

export function BrandInput({type, placeholder, props, onChange, className, onSubmit, ref}) {
    const styles = useStyles();
    const [value, setValue] = useState("");
    
    function onKeyUp(e) {
        if(e.key === "Enter" && onSubmit) onSubmit(value);
    }
    
    function onChangeEv(e) {
        setValue(e.target.value);
        if(onChange)
            onChange(e);    
    }
    
    return <input ref={ref}
                  type={type}
                  onKeyUp={onKeyUp}
                  onChange={onChangeEv}
                  placeholder={placeholder}
                  className={classNames(styles.brandInput, className)} {...props} />
}