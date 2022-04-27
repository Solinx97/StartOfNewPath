import React, { Component } from 'react';
import { Routes, Route } from 'react-router-dom';
import Login from './Login'
import Registration from './Registration'
import { Logout } from './Logout'
import { ApplicationPaths, LoginActions, LogoutActions } from './ApiAuthorizationConstants';

export default class ApiAuthorizationRoutes extends Component {

  render () {
    return(
        <Routes>
          <Route path={ApplicationPaths.Login} element={<Login />} />
          {/*<Route path={ApplicationPaths.LoginFailed} element={loginAction(LoginActions.LoginFailed)} />*/}
          {/*<Route path={ApplicationPaths.LoginCallback} element={loginAction(LoginActions.LoginCallback)} />*/}
          {/*<Route path={ApplicationPaths.Profile} element={loginAction(LoginActions.Profile)} />*/}
          <Route path={ApplicationPaths.Register} element={<Registration />} />
          {/*<Route path={ApplicationPaths.LogOut} element={logoutAction(LogoutActions.Logout)} />*/}
          {/*<Route path={ApplicationPaths.LogOutCallback} element={logoutAction(LogoutActions.LogoutCallback)} />*/}
          {/*<Route path={ApplicationPaths.LoggedOut} element={logoutAction(LogoutActions.LoggedOut)} />*/}
        </Routes>
    );
  }
}

//function loginAction(name){
//    return <Login />;
//}

//function logoutAction(name) {
//    return (<Logout action={name} />);
//}
