import React, { useState } from 'react';
import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import LoginMenu from './authorization/LoginMenu';

import '../styles/navMenu.css';

const NavMenu = (props) => {
    const [collapsed, setCollapsed] = useState(true);

    const toggleNavbar = () => {
        stCollapsed(!collapsed);
    }

    return (
        <header>
            <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" light>
                <Container>
                    <NavbarBrand tag={Link} to="/">Начало нового пути</NavbarBrand>
                    <NavbarToggler onClick={toggleNavbar} className="mr-2" />
                    <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!collapsed} navbar>
                        <ul className="navbar-nav flex-grow">
                            <NavItem>
                                <NavLink tag={Link} className="text-dark" to="/">Популярные курсы</NavLink>
                            </NavItem>
                            <LoginMenu />
                        </ul>
                    </Collapse>
                </Container>
            </Navbar>
        </header>
    );
}

export { NavMenu };
