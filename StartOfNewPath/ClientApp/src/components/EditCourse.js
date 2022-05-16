import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import FormValidate from './FormValidate';

import '../styles/formValidate.css';

const EditCourse = (props) => {
    const navigate = useNavigate();

    const [courseId, setCourseId] = useState(0);
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
    const [courseResult, setCourseResult] = useState(0);
    const [minDate, setMinDate] = useState("2022-04-26");

    useEffect(() => {
        const queryParams = new URLSearchParams(window.location.search);
        setCourseId(queryParams.get("id"));

        const date = new Date();
        const month = date.getMonth() < 10 ? `0${date.getMonth() + 1}` : date.getMonth() + 1;
        const today = `${date.getFullYear()}-${month}-${date.getDate()}`;

        setMinDate(today);
    }, []);

    useEffect(() => {
        const getCourseByIdAsync = async () => {
            await getCourseById();
        };

        if (courseId > 0) {
            getCourseByIdAsync();
        }
    }, [courseId]);

    useEffect(() => {
        if (courseResult > 0) {
            navigate("/");
        }
    }, [courseResult]);

    const handleChange = (event) => {
        const data = {};
        Object.assign(data, course);

        const isBoolean = event.target.type == "checkbox";
        const isNumber = event.target.type == "number" || event.target.type == "range";

        data[event.target.name] = isNumber
            ? Number(event.target.value)
            : isBoolean
                ? event.target.checked
                : event.target.value;

        setCourse(data);
    }

    const handleSubmit = async (event) => {
        event.preventDefault();

        await updateCourse();
    }

    const updateCourse = async () => {
        const response = await fetch(`course`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(course),
        });

        const data = await response;
        if (data.status == 200) {
            const result = await data.json();
            setCourseResult(result);
        }
    }

    const getCourseById = async () => {
        const response = await fetch(`course/${courseId}`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            }
        });

        const data = await response.json();

        let date = new Date(Date.parse(data.startDate));
        let month = date.getMonth() + 1 < 10 ? `0${date.getMonth() + 1}` : date.getMonth() + 1;
        data.startDate = `${date.getFullYear()}-${month}-${date.getDate()}`;

        date = new Date(Date.parse(data.finishDate));
        month = date.getMonth() + 1 < 10 ? `0${date.getMonth() + 1}` : date.getMonth() + 1;
        data.finishDate = `${date.getFullYear()}-${month}-${date.getDate()}`;

        setCourse(data);
    }

    const render = () => {
        return <div className="create-course__container">
            <form className="row g-3" onSubmit={handleSubmit}>
                <div className="col-md-6">
                    <label htmlFor="name" className="form-label">Название курса</label>
                    <input type="text" className="form-control" id="name"
                        name="name" value={course.name} onChange={handleChange} />
                    <FormValidate name="#name" content="Название курса" />
                </div>
                <div className="col-md-6">
                    <label htmlFor="description" className="form-label">Описание курса</label>
                    <textarea className="form-control" id="description" rows="4"
                        name="description" value={course.description} onChange={handleChange} ></textarea>
                    <FormValidate name="#description" content="Описание курса<" />
                </div>
                <div className="form-range col-md-6">
                    <label htmlFor="difficulty" className="form-label">Сложность</label>
                    <input type="range" className="form-range" id="difficulty"
                        min="0" max="5" value={course.difficulty} name="difficulty" onChange={handleChange} />
                    <div>Уровень сложности: {course.difficulty}</div>
                </div>
                <div className="col-md-6">
                    <label htmlFor="maxMentee" className="form-label">Максимальное кол-во обучающихся</label>
                    <input type="number" className="form-control" id="maxMentee"
                        value={course.maxMentee} name="maxMentee" onChange={handleChange} />
                    <FormValidate name="#maxMentee" content="Кол-во обучающихся" />
                </div>
                <div className="col-md-6 form-check">
                    <input className="form-check-input" type="checkbox" id="isHaveCheckTestBefore"
                        name="isHaveCheckTestBefore" checked={course.isHaveCheckTestBefore} onChange={handleChange} />
                    <label className="form-check-label" htmlFor="isHaveCheckTestBefore">
                        Есть проверочное задание для регистрации на курс
                    </label>
                    <FormValidate name="#isHaveCheckTestBefore" content="Есть тест До" />
                </div>
                <div className="col-md-6 form-check">
                    <input className="form-check-input" type="checkbox" id="isHaveCheckTestAfter"
                        name="isHaveCheckTestAfter" checked={course.isHaveCheckTestAfter} onChange={handleChange} />
                    <label className="form-check-label" htmlFor="isHaveCheckTestAfter">
                        Есть проверочное задание для проверки знаний после прохождения курса
                    </label>
                    <FormValidate name="#isHaveCheckTestAfter" content="Есть тест После" />
                </div>
                <div className="col-md-6">
                    <label htmlFor="startDate" className="form-label">Дата начала курса</label>
                    <input type="date" className="form-control" id="startDate"
                        value={course.startDate} name="startDate" min={minDate} onChange={handleChange} />
                </div>
                <div className="col-md-6">
                    <label htmlFor="finishDate" className="form-label">Дата окончания курса</label>
                    <input type="date" className="form-control" id="finishDate"
                        value={course.finishDate} name="finishDate" min={minDate} onChange={handleChange} />
                </div>
                <div>
                    <button type="submit" className="btn btn-success">Обновить</button>
                    <button type="button" className="btn btn-dark" onClick={() => navigate("/")}>Отмена</button>
                </div>
            </form>
        </div>;
    }

    return render();
}

export default EditCourse;
