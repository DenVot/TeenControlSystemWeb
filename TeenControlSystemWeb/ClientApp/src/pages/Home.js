import {createUseStyles} from "react-jss";
import {useEffect, useState} from "react";
import {UserAuthInfo} from "../UserAuthInfo";
import useDefStyle from "../style/jss/defaultStyle";
import {ErrorIcon} from "../components/ErrorIcon";
import {Link} from "react-router-dom";
import {Loader} from "../components/Loader";
import {Map, Placemark, YMaps} from "react-yandex-maps";
import AdminBadge from "../images/admin-badge.svg";
import Working from "../images/working.svg";

Array.__proto__.last = function() {
    return this[this.length() - 1];
}

const noInfoErr = "NO_INFO_FROM_SERVER";

const useStyles = createUseStyles({
    homeFrame: {
        width: "100%",
        height: "100%",
        display: "flex",
        flexDirection: "row",
        justifyContent: "space-around"
    },
    inlineLinkAndNameOfSection: {
        display: "flex",
        flexDirection: "row",
        alignItems: "center",
        flexWrap: "wrap",
        "& span": {
            fontSize: 32
        },
        "& a": {
            fontSize: 25
        }
    },
    sessionInfo: {
        display: "flex",
        justifyContent: "space-between",
        alignItems: "center"
    },
    sessionPeopleCountLabel: {
        color: "var(--gray)",
        fontSize: 14
    },
    sessionNameLabel: {
        fontSize: 24
    },
    noActiveSessionsLabel: {
        color: "var(--default-blue)"
    },
    map: {
        width: "50%",
        height: "100%"
    },
    userContainer: {
        display: "flex",
        flexDirection: "row",
        alignItems: "center",
        justifyContent: "space-between",
        "& img": {
            margin: 10
        },
        "& span": {
            fontSize: 24
        }
    },
    mb: {
        marginBottom: 30
    }
})

export function Home() {
    const styles = useStyles();
    const [sessions, setSessions] = useState(null);

    useEffect(() => {
        async function fetchSessions() {
            try {
                const fetched = await UserAuthInfo.userInfo.provider.getActiveSessions();
                setSessions(fetched);
            } catch (e) {
                setSessions(noInfoErr);
            }
        }

        fetchSessions();
    }, []) //Sessions fetching
    
    return <div className={styles.homeFrame}>
        <div>
            <Sessions sessions={sessions}/>
            {UserAuthInfo.userInfo.user.isAdmin && <UsersInfo/>}
        </div>
        <div>
            <div className={styles.inlineLinkAndNameOfSection}>
                <span>Группы на карте </span>
            </div>
            <GroupsOnMap sessions={sessions}/>    
        </div>
    </div>
}

function Sessions({sessions}) {
    const defStyles = useDefStyle();
    const styles = useStyles();
    
    
    if(sessions == null) {
        return <div className={defStyles.centeredBox}>
            <Loader/>
        </div>
    }
    
    if(sessions === noInfoErr) {
        return <div className={defStyles.centeredBox}>
            <ErrorIcon/>
        </div>
    }
    
    return <div className={styles.mb}>
        <div className={styles.inlineLinkAndNameOfSection}>
            <p>
                <span>Активные сессии </span>
                <Link className={defStyles.link} to="/create-session">Добавить</Link>
            </p>
        </div>
        <div className={defStyles.box}>
            <div>
                {sessions.length > 0 ? sessions.map(e => <SessionInfo sessionName={e.name} countOfPeople={e.sensors.length}/>) :
                    <span className={styles.noActiveSessionsLabel}>Нет активных сессий</span>}
            </div>
        </div>
        
    </div>;
}

function SessionInfo({sessionName, countOfPeople}) {
    const styles = useStyles();
    
    return <div className={styles.sessionInfo}>
        <span className={styles.sessionNameLabel}>{sessionName}</span>
        <span className={styles.sessionPeopleCountLabel}>{countOfPeople} чел</span>
    </div>
}

function GroupsOnMap({sessions}) {
    const defStyles = useDefStyle();
    const styles = useStyles();
    
    if(sessions == null) {
        return <div className={defStyles.centeredBox}>
            <Loader/>
        </div>
    }

    if(sessions === noInfoErr) {
        return <div className={defStyles.centeredBox}>
            <ErrorIcon/>
        </div>
    }
    
    return <div className={defStyles.box}>
        <YMaps>
            <Map
                width={500}
                height={800}
                defaultState={{
                    center: [55.751574, 37.573856],
                    zoom: 5,
                }}
            >
                {sessions.map(e => e.points.length > 0 ? <Placemark
                    geometry={[e.points[e.points.length - 1].longitude,
                        e.points[e.points.length - 1].latitude]}
                    options={{
                        iconLayout: 'default#image',
                        iconImageHref: "/api/media/def-avatar?id=" + e.owner.avatarId,
                        iconImageSize: [32, 32],
                        iconImageOffset: [-5, -38],
                    }}/> : null)}
            </Map>
        </YMaps>
    </div>
}

function UsersInfo() {
    const [users, setUsers] = useState(null);
    const defStyles = useDefStyle();
    const styles = useStyles();
    
    useEffect(() => {
        async function fetchUsers() {
            try {
                setUsers(await UserAuthInfo.userInfo.provider.getAllUsers())
            } catch (e) {
                setUsers(noInfoErr);
            }
        }
        
        fetchUsers()
    }, [])

    if(users == null) {
        return <div className={defStyles.centeredBox}>
            <Loader/>
        </div>
    }

    if(users === noInfoErr) {
        return <div className={defStyles.centeredBox}>
            <ErrorIcon/>
        </div>
    }
    
    return<div>
        <div className={styles.inlineLinkAndNameOfSection}>
            <p>
                <span>Ваши сотрудники </span>
                <Link className={defStyles.link} to="/create-session">Добавить</Link>
            </p>
        </div>
        <div className={defStyles.box}>
            {users.map(e => <UserInfo key={e.id} user={e}/>)}
        </div>
    </div>
}

function UserInfo({user}) {
    const styles = useStyles();
    
    return <div className={styles.userContainer}>
        <span>{user.username}</span>
        <div>
            <img src={AdminBadge} alt="Администратор" style={{opacity: user.isAdmin ? 1 : 0}}/>
            <img src={Working} alt="Занят(-а)" style={{opacity: user.activeSession != null ? 1 : 0}}/>    
        </div>
    </div>
}