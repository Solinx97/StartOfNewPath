import { useContext } from 'react';
import { Context } from '../../index';
import { useNavigate } from 'react-router-dom';
import { ApplicationPaths } from './AuthorizationConstants';

export const useAuthorizeService = (data) => {
    const navigate = useNavigate();
    const { userStore } = useContext(Context);

    const register = async (useAutoLogin = false) => {
        const response = await fetch('account', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data),
        });

        const statusCode = await response.status;
        if (statusCode == 200) {
            if (useAutoLogin) {
                await login();
            }
            else {
                navigate(ApplicationPaths.Login);
            }
        }
    }

    const login = async () => {
        const response = await fetch('account/login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data),
        });

        const result = await response;
        if (result.status == 200) {
            const data = await result.json();

            userStore.setUser(data);
            userStore.setIsAuth(true);

            navigate(ApplicationPaths.DefaultLoginRedirectPath);
        }
    }

    const logout = async () => {
        const response = await fetch('account/logout', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            }
        });

        const result = await response;
        if (result.status == 200) {
            userStore.setUser({});
            userStore.setIsAuth(false);

            navigate(ApplicationPaths.DefaultLoginRedirectPath);
        }
    }

    return [register, login, logout];
}