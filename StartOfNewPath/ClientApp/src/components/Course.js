import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

const Course = (props) => {
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

    useEffect(() => {
        const queryParams = new URLSearchParams(window.location.search);
        setCourseId(queryParams.get("id"));
    }, []);

    useEffect(() => {
        const getCourseByIdAsync = async () => {
            await getCourseById();
        };

        if (courseId > 0) {
            getCourseByIdAsync();
        }
    }, [courseId]);

    const getCourseById = async () => {
        console.log(1);
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
        return <div>
            <div className="col-md-6">
                <div>{course.name}</div>
            </div>
            <div className="col-md-6">
                <div>{course.description}</div>
            </div>
            <div className="col-md-6">
                <div>{course.difficulty}</div>
            </div>
            <div className="col-md-6">
                <div>{course.maxMentee}</div>
            </div>
            <div className="col-md-6">
                <div>{course.isHaveCheckTestBefore.toString()}</div>
            </div>
            <div className="col-md-6">
                <div>{course.isHaveCheckTestAfter.toString()}</div>
            </div>
            <div className="col-md-6">
                <div>{course.startDate.toString()}</div>
            </div>
            <div className="col-md-6">
                <div>{course.finishDate.toString()}</div>
            </div>
            <div className="col-md-6">
                <div>{course.population}</div>
            </div>
            <div>
                <button type="button" className="btn btn-dark" onClick={() => navigate("/")}>Назад</button>
            </div>
        </div>;
    }

    return render();
}

export default Course;
