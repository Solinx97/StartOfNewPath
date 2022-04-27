import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import authService from './api-authorization/AuthorizeService';

const MainPage = (props) => {
    const [isAuthenticated, setAuthenticated] = useState(false);
    const [loading, setloading] = useState(false);
    const [coursesRender, setCoursesRender] = useState(null);

    const navigate = useNavigate();

    useEffect(() => {
        let isMounted = true;
        const getAllGoursesAsync = async () => {
            if (isMounted) {
                await getAllGourses();
            }
        };

        getAllGoursesAsync();

        return () => {
            isMounted = false;
        };
    }, []);

    useEffect(() => {
        let isMounted = true;
        const checkAuthorizationAsync = async () => {
            if (isMounted) {
                await checkAuthorization();
            }
        };

        checkAuthorizationAsync();

        return () => {
            isMounted = false;
        };
    }, [isAuthenticated]);

    const getAllGourses = async () => {
        const response = await fetch('course', {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            },
        });

        const data = await response.json();
        setloading(false);

        fillingCourseList(data);
    }

    const fillingCourseList = (courses) => {
        const list = courses.map((element) => getCourse(element));

        setCoursesRender(
            <ul>
                {list}
            </ul>
        );
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

    const checkAuthorization = async () => {
        const isAuthenticated = await authService.isAuthenticated();
        setAuthenticated(isAuthenticated);
    }

    return (
        <div>
            {isAuthenticated &&
                <button type="button" className="btn btn-success" onClick={() => navigate("/create-course")}>Создать курс</button>
            }
            <h2>Популярные курсы</h2>
            {coursesRender}
        </div>
    );
}

export default MainPage;