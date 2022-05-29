import React, { useEffect, useContext } from 'react';
import {
    Routes,
    Route
} from 'react-router-dom';
import { Layout } from './components/Layout';
import MainPage from './components/MainPage';
import Course from './components/Course';
import CreateCourse from './components/CreateCourse';
import EditCourse from './components/EditCourse';
import ApiAuthorizationRoutes from './components/authorization/AuthorizationRoutes';
import { Context } from '.';

import './styles/custom.css';

const App = () => {
    const { userStore } = useContext(Context);

    useEffect(() => {
        const checkAuthAsync = async () => {
            await userStore.checkAuth();
        }

        if (document.cookie.indexOf("accessToken") == 0) {
            checkAuthAsync();
        }
    }, []);

     return (
         <Layout>
             <Routes>
                 <Route path='/' element={<MainPage />} />
                 <Route path='/target-course' element={<Course />} />
                 <Route path='/create-course' element={<CreateCourse />} />
                 <Route path='/edit-course/' element={<EditCourse />} />
                 <Route path="*" element={<ApiAuthorizationRoutes />} />
             </Routes>
         </Layout>
     );
}

export default App;