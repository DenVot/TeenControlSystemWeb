import getJwtExpireDate from "./jwtDeserializer";
const assert = require('assert')

it("JWT date extracting", () => {
    let result = getJwtExpireDate("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjAiLCJuYmYiOjE2NDY0NzM0NDksImV4cCI6MTY0NzY4MzA0OSwiaWF0IjoxNjQ2NDczNDQ5fQ.Pq2MWKEjJh3tEyFp-ARB0BuCIXpsSyb8HW1orTL2C4o");
    
    assert(3, result.getMonth());
})