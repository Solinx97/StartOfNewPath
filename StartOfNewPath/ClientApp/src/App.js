import React, { Component, useState, useEffect } from 'react';
import {
    Routes,
    Route,
} from 'react-router-dom';
import { Layout } from './components/Layout';
import MainPage from './components/MainPage';
import CreateCourse from './components/CreateCourse';
import ApiAuthorizationRoutes from './components/api-authorization/ApiAuthorizationRoutes';

import './styles/custom.css';

export default class App extends Component {
    static displayName = App.name;

    render() {
        return (
            <Layout>
                <Routes>
                    <Route path='/' element={<MainPage />} />
                    <Route path='/create-course' element={<CreateCourse />} />
                    <Route path="*" element={<ApiAuthorizationRoutes />} />
                </Routes>
            </Layout>
        );
    }
}