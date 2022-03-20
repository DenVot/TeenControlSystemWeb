function getJwtExpireDate(jwt) {
    let result = jwt.split(".");
    
    if(result.length !== 3) {
        return null;
    }
    
    let json = atob(result[1]);
    
    return new Date(json.exp);
}

export default getJwtExpireDate;