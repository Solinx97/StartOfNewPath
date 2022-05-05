import React, { Fragment, useContext } from 'react';
import { NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import { ApplicationPaths } from './AuthorizationConstants';
import { observer } from 'mobx-react-lite';
import { Context } from '../../index';

const LoginMenu = (props) => {
    const { userStore } = useContext(Context);

    const authenticatedView = (userName, profilePath, logoutPath) => {
        return (<Fragment>
            <NavItem>
                <NavLink tag={Link} className="text-dark" to={profilePath}>Привет {userName}</NavLink>
            </NavItem>
            <NavItem>
                <NavLink tag={Link} className="text-dark" to={logoutPath}>Выход</NavLink>
            </NavItem>
        </Fragment>);

    }

    const anonymousView = (registerPath, loginPath) => {
        return (<Fragment>
            <NavItem>
                <NavLink tag={Link} className="text-dark" to={registerPath}>Регистрация</NavLink>
            </NavItem>
            <NavItem>
                <NavLink tag={Link} className="text-dark" to={loginPath}>Логин</NavLink>
            </NavItem>
        </Fragment>);
    }

    const render = () => {
        if (!userStore.getIsAuth()) {
            const registerPath = `${ApplicationPaths.Register}`;
            const loginPath = `${ApplicationPaths.Login}`;
            return anonymousView(registerPath, loginPath);
        } else {
            const profilePath = `${ApplicationPaths.Profile}`;
            const logoutPath = `${ApplicationPaths.LogOut}`;
            return authenticatedView(userStore.getUser().userName, profilePath, logoutPath);
        }
    }

    return render();
}

export default observer(LoginMenu);
