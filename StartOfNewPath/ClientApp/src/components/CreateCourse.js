import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import authService from './api-authorization/AuthorizeService';
import FormValidate from './FormValidate';

import '../styles/formValidate.css';

const CreateCourse = (props) => {
    const navigate = useNavigate();

    const [course, setCourse] = useState({
        name: "",
        description: "",
        difficulty: 0,
        maxMentee: 0,
        isHaveCheckTestBefore: false,
        isHaveCheckTestAfter: false,
        startDate: new Date(),
        finishDate: new Date(),
        population: 0,
        ownerId: ""
    });
    const [difficulty, setDifficulty] = useState(0);
    const [courseResult, setCourseResult] = useState(0);
    const [user, setUser] = useState(null);
    const [minDate, setMinDate] = useState("2022-04-26");

    useEffect(() => {
        let isMounted = true;
        const getUserAsync = async () => {
            if (isMounted) {
                await getUser();
            }
        };

        getUserAsync();

        const date = new Date();
        const month = date.getMonth() < 10 ? `0${date.getMonth() + 1}` : date.getMonth() + 1;
        const today = `${date.getFullYear()}-${month}-${date.getDate()}`;
        setMinDate(today);

        return () => {
            isMounted = false;
        };
    }, []);

    useEffect(() => {
        if (courseResult > 0) {
            navigate("/");
        }
    }, [courseResult]);

    const getUser = async () => {
        const [user] = await Promise.all([authService.getUser()])

        setUser(user);
    }

    const handleChange = (event) => {
        const data = course;
        const isBoolean = event.target.type == "checkbox";
        const isNumber = event.target.type == "number" || event.target.type == "range";

        data[event.target.name] = isNumber
            ? Number(event.target.value)
            : isBoolean
                ? event.target.checked
                : event.target.value;

        setCourse(data);
    }

    const difficultyChange = (event) => {
        const data = course;

        data[event.target.name] = +event.target.value;

        setDifficulty(+event.target.value);
        setCourse(data);
    }

    const handleSubmit = async (event) => {
        event.preventDefault();
        await createCourse(course);
    }

    const createCourse = async (course) => {
        course.ownerId = user.sub;

        const token = await authService.getAccessToken();
        const response = await fetch('course', {
            method: 'POST',
            headers: {
                'Authorization': `Bearer ${token}`,
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(course),
        });

        const data = await response.json();
        setCourseResult(data);
    }

    return (
        <div className="create-course__container">
            <form className="row g-3" onSubmit={handleSubmit}>
                <div className="col-md-6">
                    <label htmlFor="name" className="form-label">Название курса</label>
                    <input type="text" className="form-control" id="name" name="name" onChange={handleChange} />
                    <FormValidate name="#name" content="Название курса" />
                </div>
                <div className="col-md-6">
                    <label htmlFor="description" className="form-label">Описание курса</label>
                    <textarea className="form-control" id="description" rows="4" name="description" onChange={handleChange} ></textarea>
                    <FormValidate name="#description" content="Описание курса<" />
                </div>
                <div className="form-range col-md-6">
                    <label htmlFor="difficulty" className="form-label">Сложность</label>
                    <input type="range" className="form-range" id="difficulty" min="0" max="5" defaultValue="0" name="difficulty" onChange={difficultyChange} />
                    <div>Уровень сложности: {difficulty}</div>
                </div>
                <div className="col-md-6">
                    <label htmlFor="maxMentee" className="form-label">Максимальное кол-во обучающихся</label>
                    <input type="number" className="form-control" id="maxMentee" name="maxMentee" onChange={handleChange} />
                    <FormValidate name="#maxMentee" content="Кол-во обучающихся" />
                </div>
                <div className="col-md-6 form-check">
                    <input className="form-check-input" type="checkbox" id="isHaveCheckTestBefore" name="isHaveCheckTestBefore" onChange={handleChange} />
                    <label className="form-check-label" htmlFor="isHaveCheckTestBefore">
                        Есть проверочное задание для регистрации на курс
                    </label>
                    <FormValidate name="#isHaveCheckTestBefore" content="Есть тест До" />
                </div>
                <div className="col-md-6 form-check">
                    <input className="form-check-input" type="checkbox" id="isHaveCheckTestAfter" name="isHaveCheckTestAfter" onChange={handleChange} />
                    <label className="form-check-label" htmlFor="isHaveCheckTestAfter">
                        Есть проверочное задание для проверки знаний после прохождения курса
                    </label>
                    <FormValidate name="#isHaveCheckTestAfter" content="Есть тест После" />
                </div>
                <div className="col-md-6">
                    <label htmlFor="startDate" className="form-label">Дата начала курса</label>
                    <input type="date" className="form-control" id="startDate" name="startDate" min={minDate} onChange={handleChange} />
                </div>
                <div className="col-md-6">
                    <label htmlFor="finishDate" className="form-label">Дата окончания курса</label>
                    <input type="date" className="form-control" id="finishDate" name="finishDate" min={minDate} onChange={handleChange} />
                </div>
                <div>
                    <button type="submit" className="btn btn-success">Создать</button>
                    <button type="button" className="btn btn-dark" onClick={() => navigate("/")}>Отмена</button>
                </div>
            </form>
        </div>
    );
}

export default CreateCourse;
