import React, { useState, useEffect } from 'react'
import { useNavigate } from 'react-router-dom';
import { useAuthorizeService } from './AuthorizeService';

const Registration = (props) => {
    const navigate = useNavigate();

    const [user, setUser] = useState({
        userName: "",
        firstName: "",
        surname: "",
        email: "",
        password: "",
        passwordConfirm: ""
    });

    const [isAuth, serManager, register] = useAuthorizeService(user);

    const handleChange = (event) => {
        const val = event.target.value;
        const name = event.target.name;
        const userData = user;

        userData[name] = val;
        setUser(userData);
    }

    const handleSubmit = async (event) => {
        event.preventDefault();

        await register(true);
    }

    return (
        <form className="registration__container" onSubmit={handleSubmit}>
            <div className="mb-3 row">
                <label htmlFor="userName" className="col-sm-2 col-form-label">Имя пользователя</label>
                <div className="col-sm-10">
                    <input type="text" className="form-control" id="userName" name="userName" onChange={handleChange} />
                </div>
            </div>
            <div className="mb-3 row">
                <label htmlFor="firstName" className="col-sm-2 col-form-label">Имя</label>
                <div className="col-sm-10">
                    <input type="text" className="form-control" id="firstName" name="firstName" onChange={handleChange} />
                </div>
            </div>
            <div className="mb-3 row">
                <label htmlFor="surname" className="col-sm-2 col-form-label">Фамилия</label>
                <div className="col-sm-10">
                    <input type="text" className="form-control" id="surname" name="surname" onChange={handleChange} />
                </div>
            </div>
            <div className="mb-3 row">
                <label htmlFor="email" className="col-sm-2 col-form-label">Email</label>
                <div className="col-sm-10">
                    <input type="email" className="form-control" id="email" name="email" onChange={handleChange} />
                </div>
            </div>
            <div className="mb-3 row">
                <label htmlFor="password" className="col-sm-2 col-form-label">Пароль</label>
                <div className="col-sm-10">
                    <input type="password" className="form-control" id="password" name="password" onChange={handleChange} />
                </div>
            </div>
            <div className="mb-3 row">
                <label forhtml="confirmPassword" className="col-sm-2 col-form-label">Подтверждение пароля</label>
                <div className="col-sm-10">
                    <input type="password" className="form-control" id="passwordConfirm" name="passwordConfirm" onChange={handleChange} />
                </div>
            </div>
            <div>
                <button type="submit" className="btn btn-success">Регистрация</button>
                <button type="button" className="btn btn-dark" onClick={() => navigate("/")}>Отмена</button>
            </div>
        </form>
    );
}

export default Registration;