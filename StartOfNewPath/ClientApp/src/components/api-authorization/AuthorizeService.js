import { useContext } from 'react';
import { Context } from '../../index';
import { useNavigate } from 'react-router-dom';

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
                navigate("/authentication/login");
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

            navigate("/");
        }
    }

    return [register, login];
}