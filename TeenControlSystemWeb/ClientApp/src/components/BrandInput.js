import style from "../css/brand-input.module.css";

export function BrandInput({type, placeholder, props}) {
    return <input type={type} placeholder={placeholder} className={style.brandInput} {...props}/>
}