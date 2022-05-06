import React, { useContext } from 'react'
import { useNavigate } from 'react-router-dom';
import { observer } from 'mobx-react-lite';
import { Context } from '../..';

import '../../styles/profile.css';

const Profile = (props) => {
    const navigate = useNavigate();
    const { userStore } = useContext(Context);

    const handleChange = (event) => {
        const val = event.target.value;
        const name = event.target.name;
        const userData = userStore.user;

        userData[name] = val;
        userStore.setUser(userData);
    }

    const handleSubmit = async (event) => {
        event.preventDefault();
    }

    return (
        <form className="profile__container" onSubmit={handleSubmit}>
            <div className="mb-3 row">
                <label htmlFor="userName" className="col-sm-2 col-form-label">Имя пользователя</label>
                <div className="col-sm-10">
                    <input type="text" className="form-control" id="userName" name="userName" value={userStore.user.userName} onChange={handleChange} readOnly />
                </div>
            </div>
            <div className="mb-3 row">
                <label htmlFor="firstName" className="col-sm-2 col-form-label">Имя</label>
                <div className="col-sm-10">
                    <input type="text" className="form-control" id="firstName" name="firstName" value={userStore.user.firstName} onChange={handleChange} readOnly />
                </div>
            </div>
            <div className="mb-3 row">
                <label htmlFor="surname" className="col-sm-2 col-form-label">Фамилия</label>
                <div className="col-sm-10">
                    <input type="text" className="form-control" id="surname" name="surname" value={userStore.user.surname} onChange={handleChange} readOnly />
                </div>
            </div>
            <div className="mb-3 row">
                <label htmlFor="email" className="col-sm-2 col-form-label">Email</label>
                <div className="col-sm-10">
                    <input type="email" className="form-control" id="email" name="email" value={userStore.user.email} onChange={handleChange} readOnly />
                </div>
            </div>
            <div>
                <button type="submit" className="btn btn-success">Сохранить</button>
                <button type="button" className="btn btn-dark" onClick={() => navigate("/")}>Отмена</button>
            </div>
        </form>
    );
}

export default observer(Profile);