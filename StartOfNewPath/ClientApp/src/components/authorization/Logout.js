import React, { useEffect } from 'react';
import { observer } from 'mobx-react-lite';
import { useAuthorizeService } from './AuthorizeService';

const Logout = () => {
    const [register, login, profile, logout] = useAuthorizeService({});

    useEffect(() => {
        const logoutAsync = async () => {
            await logout();
        };

        logoutAsync();
    }, []);

    return (<div>Выход...</div>);
}

export default observer(Logout);