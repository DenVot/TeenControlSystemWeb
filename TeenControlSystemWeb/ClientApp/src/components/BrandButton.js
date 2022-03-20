import style from "../css/brand-button.module.css";

export function BrandButton({isSubmit}, props) {
    return isSubmit ? <input className={style.brandButton} type="submit" value={props.children} {...props}/> : 
        <button className={style.brandButton} {...props}>
            {props.children}
        </button> 
}