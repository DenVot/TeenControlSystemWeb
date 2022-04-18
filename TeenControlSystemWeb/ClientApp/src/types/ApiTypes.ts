import {ApiProvider} from "../providers/ApiProvider";

export interface Sensor {
    id: number,
    name: string,
    mac: string,
    activeSession: Session | null,
    online: boolean,
    order: number
}

export interface Session {
    id: number,
    name: string,
    owner: User | null,
    startAt: Date,
    startedAt: Date | null,
    endedAt: Date | null,
    sensors: Sensor[]
}

export interface User {
    id: number,
    username: string,
    isAdmin: boolean,
    activeSession: Session | null,
    avatarId: number
}

export interface UserInfo {
    provider: ApiProvider,
    user: User
}