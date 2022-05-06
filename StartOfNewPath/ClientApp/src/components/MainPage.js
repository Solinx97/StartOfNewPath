import React, { useEffect, useState, useContext } from 'react';
import { useNavigate } from 'react-router-dom';
import { observer } from 'mobx-react-lite';
import { Context } from '..';

const MainPage = (props) => {
    const [coursesRender, setCoursesRender] = useState(null);
    const { userStore } = useContext(Context);

    const navigate = useNavigate();

    useEffect(() => {
        const getAllGoursesAsync = async () => {
            await getAllGourses();
        };

        getAllGoursesAsync();
    }, []);

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
                <ul>
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
                <div>{element.name}</div>
                <div>{element.description}</div>
                <div>{element.difficulty}</div>
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