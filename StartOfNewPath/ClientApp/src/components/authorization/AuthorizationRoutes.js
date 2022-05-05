import React from 'react';
import { Routes, Route } from 'react-router-dom';
import Login from './Login'
import Registration from './Registration'
import Logout from './Logout'
import { ApplicationPaths } from './AuthorizationConstants';

const ApiAuthorizationRoutes = () => {
    return (
        <Routes>
            <Route path={ApplicationPaths.Login} element={<Login />} />
            <Route path={ApplicationPaths.Register} element={<Registration />} />
            <Route path={ApplicationPaths.LogOut} element={<Logout />} />
        </Routes>
    );
}

export default ApiAuthorizationRoutes;