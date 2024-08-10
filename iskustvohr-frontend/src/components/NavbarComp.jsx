import React from 'react';
import { Container, Nav, Navbar } from 'react-bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css';
import { useAuth } from '../context/AuthContext';
import { useNavigate } from 'react-router-dom';
import { ReactComponent as Logo } from '../logo/logo.svg';

export default function NavbarComp() {
    const user = useAuth();
    const navigate = useNavigate();
    
    const logout = async () => {
        try {
            await user.logout();
            navigate("/");
        } catch (err) {
            console.error('Logout error:', err);
        }
    };

    return (
        <Navbar collapseOnSelect expand="lg" className="navbar">
            <Container>
                <Navbar.Brand href="/" className='d-flex'>
                    <Logo width="50" className="d-inline-block align-top"/>
                    <p className="navbarText">Iskustvo.hr</p>
                </Navbar.Brand>
                <Navbar.Toggle aria-controls="responsive-navbar-nav" />
                <Navbar.Collapse id="responsive-navbar-nav">
                    <Nav className='me-auto'></Nav>
                    <Nav className='ml-auto' id="navLinks">
                        {user.token === "" ? (
                            <>
                                <Nav.Link href="/prijava">Prijava</Nav.Link>
                                <Nav.Link href="/registracija">Registracija</Nav.Link>
                            </>
                        ) : (
                            <>
                                <Nav.Link href="/profil">Profil</Nav.Link>
                                <Nav.Link onClick={logout}>Odjava</Nav.Link>
                            </>
                        )}
                    </Nav>
                </Navbar.Collapse>
            </Container>
        </Navbar>
    );
}
