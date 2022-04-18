import getJwtExpireDate from "../jwtDeserializer";
import {UserAuthInfo} from "../UserAuthInfo";
import {User, Session, UserInfo, Sensor} from "../types/ApiTypes";

export class ApiProvider {
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
            "Authorization": "Bearer " + this.token,
            "Content-Type": "application/json"
        };
    }
    
    async getActiveSessions() : Promise<Session[]> {
        let response: Response = await fetch("/api/sessions/get-active-sessions", {
            headers: this.configureHeaders(),
            method: "GET"
        });
        
        await ApiProvider.ensureSuccessReponse(response);
        
        return response.json();
    }
    
    async getAllUsers() : Promise<User[]> {
        let response: Response = await fetch("/api/users/get-all-users", {
            headers: this.configureHeaders(),
            method: "GET"
        });

        await ApiProvider.ensureSuccessReponse(response);

        return response.json();
    }
    
    addSensor(mac: string, name: string, order: number | null) {
        fetch("/api/sensors/add-sensor", {
            headers: this.configureHeaders(),
            method: "POST",
            body: JSON.stringify({
                order: order,
                name: name,
                mac: mac
            })
        }).then(response => ApiProvider.ensureSuccessReponse(response));
    }
    
    async getAllSensors() : Promise<Sensor[]> {
        let response: Response = await fetch("/api/sensors/get-all-sensors", {
            headers: this.configureHeaders(),
            method: "GET"
        });

        await ApiProvider.ensureSuccessReponse(response);

        return response.json();
    }
    
    editSensorName(id: number, name: string) {
        fetch("/api/sensors/edit-sensor-name", {
            headers: this.configureHeaders(),
            method: "PUT",
            body: JSON.stringify({
                sensorId: id,
                newName: name
            })
        }).then(response => ApiProvider.ensureSuccessReponse(response));
    }
    
    editSensorOrder(id: number, order: number) {
        fetch("/api/sensors/edit-sensor-order", {
            headers: this.configureHeaders(),
            method: "PUT",
            body: JSON.stringify({
                sensorId: id,
                order: order
            })
        }).then(response => ApiProvider.ensureSuccessReponse(response));
    }
    
    deleteSensor(id: number) {
        fetch("/api/sensors/remove-sensor?id=" + id, {
            headers: this.configureHeaders(),
            method: "DELETE"
        }).then(response => ApiProvider.ensureSuccessReponse(response));
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
            "Content-Type": "application/json"
        },
        method: "POST"
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

export async function getCachedUser(): Promise<UserInfo> {
    let tokenFromStorage: string | null = window.localStorage.getItem('auth_token');
    let expireDate: Date | null = new Date(getJwtExpireDate(tokenFromStorage) * 1000);
    let now: Date = new Date();
    
    if(tokenFromStorage != null && tokenFromStorage != "" && expireDate != null && expireDate > now) {
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

export function resetCache() {
    window.localStorage.setItem("auth_token", "null");
}
