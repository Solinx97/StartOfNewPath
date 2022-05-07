import React, { useContext, useState } from 'react'
import { observer } from 'mobx-react-lite';
import { Context } from '../..';

import '../../styles/profile.css';
import { useAuthorizeService } from './AuthorizeService';

const Profile = (props) => {
    const { userStore } = useContext(Context);
    
    const [isEdit, setIsEdit] = useState(false);
    const [userProfile, setUserProfile] = useState({
        id: userStore.id,
        userName: "",
        firstName: "",
        surname: "",
        email: ""
    });

    const [register, login, profile] = useAuthorizeService(userProfile);

    const handleChange = (event) => {
        const val = event.target.value;
        const name = event.target.name;

        userProfile[name] = val;
        setUserProfile(userProfile);
    }

    const handleSubmit = async (event) => {
        event.preventDefault();

        await profile();
    }

    const handleSwitch = () => {
        setIsEdit(!isEdit);

        setUserProfile(userStore.user);
    }

    const userDataView = () => {
        return <div>
            <div className="mb-3 row">
                <div className="col-sm-2">Имя пользователя</div>
                <div>{userStore.user.userName}</div>
            </div>
            <div className="mb-3 row">
                <div className="col-sm-2">Имя</div>
                <div>{userStore.user.firstName}</div>
            </div>
            <div className="mb-3 row">
                <div className="col-sm-2">Фамилия</div>
                <div>{userStore.user.surname}</div>
            </div>
            <div className="mb-3 row">
                <div className="col-sm-2">Email</div>
                <div>{userStore.user.email}</div>
            </div>
            <div>
                <button type="button" className="btn btn-success" onClick={handleSwitch}>Редактировать</button>
            </div>
        </div>;
    }

    const userDataWithEditView = () => {
        return <form className="profile__container" onSubmit={handleSubmit}>
            <div className="mb-3 row">
                <label htmlFor="userName" className="col-sm-10 col-form-label">Имя пользователя</label>
                <div className="col-sm-10">
                    <input type="text" className="form-control" id="userName" name="userName" value={userProfile.userName} onChange={handleChange} />
                </div>
            </div>
            <div className="mb-3 row">
                <label htmlFor="firstName" className="col-sm-10 col-form-label">Имя</label>
                <div className="col-sm-10">
                    <input type="text" className="form-control" id="firstName" name="firstName" value={userProfile.firstName} onChange={handleChange} />
                </div>
            </div>
            <div className="mb-3 row">
                <label htmlFor="surname" className="col-sm-10 col-form-label">Фамилия</label>
                <div className="col-sm-10">
                    <input type="text" className="form-control" id="surname" name="surname" value={userProfile.surname} onChange={handleChange} />
                </div>
            </div>
            <div className="mb-3 row">
                <label htmlFor="email" className="col-sm-10 col-form-label">Email</label>
                <div className="col-sm-10">
                    <input type="email" className="form-control" id="email" name="email" value={userProfile.email} onChange={handleChange} />
                </div>
            </div>
            <div>
                <button type="submit" className="btn btn-success">Сохранить</button>
                <button type="button" className="btn btn-dark" onClick={handleSwitch}>Отмена</button>
            </div>
        </form>;
    }

    const render = () => {
        if (isEdit) {
            return userDataWithEditView();
        }
        else {
            return userDataView();
        }
    }

    return render();
}

export default observer(Profile);