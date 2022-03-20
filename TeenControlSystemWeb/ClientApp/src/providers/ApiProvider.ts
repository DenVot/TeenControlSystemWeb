import getJwtExpireDate from "../jwtDeserializer";
import {UserAuthInfo} from "../UserAuthInfo";

interface Sensor {
    id: number,
    name: string,
    mac: string,
    activeSession: Session | null,
    online: boolean
}

interface Session {
    id: number,
    name: string,
    owner: User | null,
    startAt: Date,
    startedAt: Date | null,
    endedAt: Date | null,
    sensors: Sensor[]
}

interface User {
    id: number,
    username: string,
    isAdmin: boolean,
    activeSession: Session | null
}

export interface UserInfo {
    provider: ApiProvider,
    user: User
}

class ApiProvider {
    private readonly token: string;
    
    constructor(token: string) {
        this.token = token;
    }
    
    async getContextUser() : Promise<User> {
        let response: Response = await fetch("/api/users/get-context-user", {
            headers: this.configureHeaders(),
            method: "GET"
        });
        
        await ApiProvider.ensureSuccessReponse(response);
        
        return response.json();
    }
    
    configureHeaders() : any {
        return {
            "Authorization": this.token
        };
    }
    
    private static async ensureSuccessReponse(response: Response) {
        if(!response.ok) {
            throw new Error(await response.json());
        }
    }
}

export default async function initialize(username: string, password: string): Promise<UserInfo> {
    let result: Response = await fetch("/api/auth/login", {
        body: JSON.stringify({
            username: username,
            password: password
        }),
        headers: {
            "Content-Type": "application-json"
        }
    });
    
    if(!result.ok) {
        throw new Error(await result.json());
    }
    
    let userInfo = await result.json();
    
    let token = userInfo.token;
    
    window.localStorage.setItem("auth_token", token);
    let res = {
        provider: new ApiProvider(token),
        user: userInfo.user
    }; 
    
    UserAuthInfo.userInfo = res;
    
    return res;
}

export async function tryGetDefaultInstance(): Promise<UserInfo> {
    let tokenFromStorage: string | null = window.localStorage.getItem('auth_token');
    let expireDate: Date | null = getJwtExpireDate(tokenFromStorage);

    if(tokenFromStorage != null && expireDate != null && expireDate > new Date()) {
        let api = new ApiProvider(tokenFromStorage);

        let user = await api.getContextUser();
        let res = {
            user: user,
            provider: api
        }; 
        
        UserAuthInfo.userInfo = res;
        
        return res;
    }
    
    throw new Error();
}
