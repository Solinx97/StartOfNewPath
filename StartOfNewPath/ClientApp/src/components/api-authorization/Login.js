import React, { useState, useEffect } from 'react'
import { useNavigate } from 'react-router-dom';
import { useAuthorizeService } from './AuthorizeService';

import '../../styles/login.css';

const Login = (props) => {
    const navigate = useNavigate();

    const [user, setUser] = useState({
        userName: "",
        password: ""
    });

    const [isAuth, userManager, register, login] = useAuthorizeService(user);

    const handleSubmit = async () => {
        event.preventDefault();

        await login();
    }

    const handleChange = (event) => {
        const val = event.target.value;
        const name = event.target.name;
        const userData = user;

        userData[name] = val;
        setUser(userData);
    }

    return (
        <form className="login__container" onSubmit={handleSubmit}>
            <div className="mb-3 row">
                <label htmlFor="userName" className="col-sm-2 col-form-label">Имя пользователя</label>
                <div className="col-sm-10">
                    <input type="text" className="form-control" id="userName" name="userName" onChange={handleChange} />
                </div>
            </div>
            <div className="mb-3 row">
                <label htmlFor="password" className="col-sm-2 col-form-label">Пароль</label>
                <div className="col-sm-10">
                    <input type="password" className="form-control" id="password" name="password" onChange={handleChange} />
                </div>
            </div>
            <div>
                <button type="submit" className="btn btn-success">Авторизация</button>
                <button type="button" className="btn btn-dark" onClick={() => navigate("/")}>Отмена</button>
            </div>
        </form>
    );
}

export default Login;