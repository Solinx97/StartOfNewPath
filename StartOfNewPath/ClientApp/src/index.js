import 'bootstrap/dist/css/bootstrap.css';
import React, { createContext } from 'react';
import ReactDOM from 'react-dom';
import { BrowserRouter } from 'react-router-dom';
import App from './App';
import UserStore from './components/UserStore';

const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href');
const rootElement = document.getElementById('root');

const userStore = new UserStore();

export const Context = createContext({
    userStore: userStore
});

ReactDOM.render(
    <Context.Provider value={{ userStore}}>
        <BrowserRouter basename={baseUrl}>
            <App />
        </BrowserRouter>
    </Context.Provider>,
    rootElement);

// Uncomment the line above that imports the registerServiceWorker function
// and the line below to register the generated service worker.
// By default create-react-app includes a service worker to improve the
// performance of the application by caching static assets. This service
// worker can interfere with the Identity UI, so it is
// disabled by default when Identity is being used.
//
//registerServiceWorker();

