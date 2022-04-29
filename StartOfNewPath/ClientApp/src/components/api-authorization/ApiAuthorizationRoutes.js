import React from 'react';
import { Routes, Route } from 'react-router-dom';
import Login from './Login'
import Registration from './Registration'
import { ApplicationPaths } from './ApiAuthorizationConstants';

const ApiAuthorizationRoutes = () => {
    return (
        <Routes>
            <Route path={ApplicationPaths.Login} element={<Login />} />
            <Route path={ApplicationPaths.Register} element={<Registration />} />
        </Routes>
    );
}

export default ApiAuthorizationRoutes;