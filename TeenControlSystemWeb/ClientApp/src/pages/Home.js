import {createUseStyles} from "react-jss";
import React, {useEffect, useRef, useState} from "react";
import {UserAuthInfo} from "../UserAuthInfo";
import useDefStyle from "../style/jss/defaultStyle";
import {ErrorIcon} from "../components/ErrorIcon";
import {Link} from "react-router-dom";
import {Loader} from "../components/Loader";
import {Map, Placemark, YMaps} from "react-yandex-maps";
import AdminBadge from "../images/admin-badge.svg";
import Working from "../images/working.svg";
import Sensor from "../images/sensor.svg";
import Users from "../images/users.svg";
import HomeIcon from "../images/home.svg";
import classNames from "classnames";

Array.__proto__.last = function() {
    return this[this.length() - 1];
}

const noInfoErr = "NO_INFO_FROM_SERVER";

const useStyles = createUseStyles({
    homeFrame: {
        width: "100%",
        height: "100%"
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
        },
        width: "100%"
    },
    mb: {
        marginBottom: 30
    },
    mainInfoFrame: {
        position: "absolute",
        width: "30%",
        height: "100%",
        left: 0,
        top: 0,
        zIndex: 100000,
        background: "rgba(245, 245, 245, 0.59)",
        backdropFilter: "blur(4px)",
        padding: 50,
        transition: "transform 1s cubic-bezier(0, 0.66, 0.65, 1)"
    },
    headerStyle: {
        position: "absolute",
        width: "100%",
        left: 0,
        top: 0,
        zIndex: 10000,
        display: "flex",
        flexDirection: "row",
        justifyContent: "flex-end",
        alignItems: "center",
        "& a": {
            color: "#000",
            textDecoration: "none",
            fontSize: 24,
            fontWeight: "bold",
            "&:hover": {
                textDecoration: "underline"
            }
        },
        height: "10%"
    },
    navImage: {
        margin: 10
    },
    avatar: {
        borderRadius: "50%",
        margin: 10
    },
    lowDunk: {
        position: "absolute",
        width: 50,
        height: 50,
        bottom: 60,
        left: "calc(50% - 25px)",
        borderRadius: "50%",
        background: "var(--default-blue)",
        display: "none",
        zIndex: 10000000000000,
        outline: "none",
        border: "none",
        color: "#FFF",
        fontSize: 24
    },
    "@media (max-width: 720px)": {
        lowDunk: {
            display: "block"
        },
        mainInfoFrame: {
            top: "100%",
            width: "100%"
        },
        homeFrame: {
            height: "100vh"
        }
    },
    scrolledBox: {
        maxHeight: "500px",
        overflowY: "auto",
        overflowX: "hidden",
        "&::-webkit-scrollbar": {
            width: 20
        },
        "&::-webkit-scrollbar-thumb": {
            background: "var(--default-blue)",
            borderRadius: 9000
        }
    }
})

export function Home() {
    const styles = useStyles();
    const [sessions, setSessions] = useState(null);
    const [lowDunkOpened, setOpened] = useState(false);
    
    
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
        <Header/>
        <button onClick={() => setOpened(!lowDunkOpened)} className={styles.lowDunk}>{lowDunkOpened ? "-" : "+"}</button>
        <div style={{transform: lowDunkOpened ? "translateY(-100%)" : "translateY(0)"}}
             className={styles.mainInfoFrame}>
            <Sessions sessions={sessions}/>
            {UserAuthInfo.userInfo.user.isAdmin && <UsersInfo/>}
        </div>
        <div>
            <GroupsOnMap sessions={sessions}/>    
        </div>
    </div>
}

function Header() {
    const user = UserAuthInfo.userInfo.user;
    const styles = useStyles();
    const icoWidth = 30;

    return <div className={styles.headerStyle}>
        <nav>
            <Link to="/home">
                <img className={styles.navImage} src={HomeIcon} alt="Домой" width={icoWidth}/>
                <img src={Sensor} alt="Маячки" width={icoWidth} className={styles.navImage}/>
                {user.isAdmin && <img src={Users} alt="Пользователи" width={icoWidth} className={styles.navImage}/>}
            </Link>
        </nav>
        <div>
            <Link to="/profile">
                {user.username}
            </Link>
            <img className={styles.avatar} src={"/api/media/def-avatar?id=" + user.avatarId} alt="Подождите..."/>
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
        <div className={classNames(defStyles.box, styles.scrolledBox)}>
            <div>
                {sessions.length > 0 ? sessions.map(e => <SessionInfo key={e.id} sessionName={e.name} countOfPeople={e.sensors.length}/>) :
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
    const [mapWidth, setWidth] = useState(window.innerWidth);
    const [mapHeight, setHeight] = useState(window.innerHeight);
    
    window.addEventListener("resize", function (e) {
        setWidth(window.innerWidth);
        setHeight(window.innerHeight);
    })
    
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
    
    return <YMaps>
            <Map
                width={mapWidth}
                height={mapHeight}
                defaultState={{
                    center: [55.751574, 37.573856],
                    zoom: 5,
                }}
            >
                {sessions.map(e => e.points.length > 0 ? <Placemark
                    key={e.id}
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
    
    return <div>
        <div className={styles.inlineLinkAndNameOfSection}>
            <p>
                <span>Ваши сотрудники </span>
                <Link className={defStyles.link} to="/create-session">Добавить</Link>
            </p>
        </div>
        <div className={classNames(defStyles.box, styles.scrolledBox)}>
            {users.map(e => <UserInfo key={e.id} user={e}/>)}
        </div>
    </div>
}

function UserInfo({user}) {
    const styles = useStyles();
    const iconWidth = 32;
    
    return <div className={styles.userContainer}>
        <span>{user.username}</span>
        <div>
            <img width={iconWidth} src={AdminBadge} alt="Администратор" style={{opacity: user.isAdmin ? 1 : 0}}/>
            <img width={iconWidth} src={Working} alt="Занят(-а)" style={{opacity: user.activeSession != null ? 1 : 0}}/>    
        </div>
    </div>
}