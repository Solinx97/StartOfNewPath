import React, { useEffect, useContext } from 'react';
import {
    Routes,
    Route
} from 'react-router-dom';
import { Layout } from './components/Layout';
import MainPage from './components/MainPage';
import CreateCourse from './components/CreateCourse';
import ApiAuthorizationRoutes from './components/authorization/AuthorizationRoutes';
import { Context } from '.';

import './styles/custom.css';

const App = () => {
    const { userStore } = useContext(Context);

    useEffect(() => {
        const checkAuthAsync = async () => {
            await userStore.checkAuth();
        }

        checkAuthAsync();
    }, []);

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

export default App;