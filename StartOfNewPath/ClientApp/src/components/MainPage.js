﻿import React, { useEffect, useState, useContext } from 'react';
import { NavLink, useNavigate } from 'react-router-dom';
import { observer } from 'mobx-react-lite';
import { Context } from '..';

import '../styles/mainPage.css';

const MainPage = (props) => {
    const navigate = useNavigate();
    const [coursesRender, setCoursesRender] = useState(null);
    const { userStore } = useContext(Context);

    useEffect(() => {
        const getAllGoursesAsync = async () => {
            await getAllGourses();
        };

        getAllGoursesAsync();
    }, [userStore.user]);

    const getAllGourses = async () => {
        const response = await fetch('course', {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            },
        });

        const data = await response.json();
        fillingCourseList(data);
    }

    const fillingCourseList = (courses) => {
        if (courses.length > 0) {
            const list = courses.map((element) => getCourse(element));

            setCoursesRender(
                <ul className="courses__container">
                    {list}
                </ul>
            );
        }
        else {
            setCoursesRender(<div>Необходимо добавить хотя бы 1 курс</div>);
        }
    }

    const getCourse = (element) => {
        return (
            <li key={element.id}>
                <div className="card">
                    <div className="card-body">
                        <h5 className="card-title">{element.name}</h5>
                        <p className="card-text">{element.description}</p>
                    </div>
                    <ul className="list-group list-group-flush">
                        <li className="list-group-item">Сложность: {element.difficulty}</li>
                        <li className="list-group-item">Макс участвников: {element.maxMentee}</li>
                        <li className="list-group-item">Популярность: {element.population}</li>
                        <li className="list-group-item">Необходимо пройти тест: {element.isHaveCheckTestBefore}</li>
                        <li className="list-group-item">Необходимо пройти проверочный тест: {element.isHaveCheckTestAfter}</li>
                    </ul>
                    <div className="card-body">
                        <NavLink className="card-link" to="/edit-course">Открыть</NavLink>
                        {userStore.user.id == element.ownerId &&
                            <NavLink className="card-link" to={"/edit-course?id=" + element.id}>Редактировать</NavLink>
                        }
                    </div>
                </div>
            </li>
        );
    }

    const render = () => {
        return <div>
            {userStore.isAuth &&
                <button type="button" className="btn btn-success" onClick={() => navigate("/create-course")}>Создать курс</button>
            }
            <h2>Популярные курсы</h2>
            {coursesRender}
        </div>
    }

    return render();
}

export default observer(MainPage);