function getJwtExpireDate(jwt) {
    let result = jwt.split(".");
    
    if(result.length !== 3) {
        return null;
    }
    
    let json = JSON.parse(atob(result[1]));
    
    return json.exp;
}

export default getJwtExpireDate;