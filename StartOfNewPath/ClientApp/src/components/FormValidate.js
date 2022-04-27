import React, { useState, useEffect } from 'react';

import '../styles/createCourse.css';

const FormValidate = ({ name, content }) => {
    const [elementName, setElementName] = useState(name);
    const [element, setElement] = useState();
    const [isValid, setValid] = useState(true);

    useEffect(() => {
        const targetElement = document.querySelector(elementName);
        setElement(targetElement);
    }, [elementName]);

    useEffect(() => {
        if (element != undefined) {
            element.oninput = onChange;
        }
    }, [element]);

    const onChange = (event) => {
        const value = Number.isInteger(event.target.value)
            ? Number(event.target.value)
            : event.target.value;

        const borderColor = validate(value);
        element.style.borderColor = borderColor;
    }

    const validate = (inputVal) => {
        if ((Number.isInteger(inputVal) && inputVal > 0)
            || inputVal.length > 0) {
            setValid(true);
            return "green";
        }
        else {
            setValid(false);
            return "red";
        }
    }

    return (<div className="invalidate">{isValid ? '' : `Поле '${content}' не корректно или необходимо заполнить!`}</div>)
}

export default FormValidate;