import { makeAutoObservable } from 'mobx';

export default class UserStore {
    isAuth = false;
    user = {};

    constructor() {
        makeAutoObservable(this);
    }

    setIsAuth(isAuth) {
        this.isAuth = isAuth;
    }

    setUser(user) {
        this.user = user;
    }

    async checkAuth() {
        const response = await fetch('authentication/refresh', {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            }
        });

        const result = await response;
        if (result.status == 200) {
            const data = await result.json();

            this.setUser(data);
            this.setIsAuth(true);
        }

        if (result.status == 401) {
            this.setUser({});
            this.setIsAuth(false);

            console.log("Need Authorize");
        }
    }
}