import { makeAutoObservable } from 'mobx';

export default class UserStore {
    constructor() {
        this.isAuth = false;
        this.user = {};

        makeAutoObservable(this);
    }

    getIsAuth() {
        return this.isAuth;
    }

    setIsAuth(isAuth) {
        this.isAuth = isAuth;
    }

    getUser() {
        return this.user;
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
        else {
            this.setIsAuth(false);
        }
    }
}