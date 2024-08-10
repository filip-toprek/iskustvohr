import React, { useState } from 'react';
import { Form, Button } from 'react-bootstrap';
import { useNavigate } from 'react-router-dom';
import userService from '../services/UserService';
import LoadingIcons from 'react-loading-icons';

export default function RegisterForm() {
    const [user, setUser] = useState({
        firstName: '',
        lastName: '',
        email: '',
        password: '',
        verifyPassword: '',
        locationId: '',
    });
    const [error, setError] = useState('');
    const navigate = useNavigate();
    const [isLoading, setIsLoading] = useState(false);
    const handleSubmit = (e) => {
        e.preventDefault();
        if (user.password !== user.verifyPassword) {
            alert("Lozinke se ne podudaraju!");
            return;
        }

        setIsLoading(true);
        setError(null);

        userService.register(user)
            .then((response) => {
                if (response.status === 201) {
                    navigate('/prijava');
                }
            })
            .catch((error) => {
                console.error('Registration failed:', error);
                setError('Registracija nije uspjela!');
                setIsLoading(false);
            });
    };

    return (
        <Form onSubmit={handleSubmit} className='registerForm'>
                <h2 className='text-center'>Registracija</h2>
                {error && <p className='error'>{error}</p>}
            <Form.Group controlId="formBasicFirstName">
                <Form.Label>Ime</Form.Label>
                <Form.Control
                    type="text"
                    placeholder="Unesite ime"
                    value={user.firstName}
                    required
                    onChange={(e) => setUser({ ...user, firstName: e.target.value })}
                />
            </Form.Group>

            <Form.Group controlId="formBasicLastName">
                <Form.Label>Prezime</Form.Label>
                <Form.Control
                    type="text"
                    placeholder="Unesite prezime"
                    value={user.lastName}
                    required
                    onChange={(e) => setUser({ ...user, lastName: e.target.value })}
                />
            </Form.Group>

            <Form.Group controlId="formBasicEmail">
                <Form.Label>Adresa e-pošte</Form.Label>
                <Form.Control
                    type="email"
                    placeholder="Unesite e-poštu"
                    value={user.email}
                    required
                    onChange={(e) => setUser({ ...user, email: e.target.value })}
                />
            </Form.Group>

            <Form.Group controlId="formBasicPassword">
                <Form.Label>Lozinka</Form.Label>
                <Form.Control
                    type="password"
                    placeholder="Unesite lozinku"
                    value={user.password}
                    required
                    onChange={(e) => setUser({ ...user, password: e.target.value })}
                />
            </Form.Group>

            <Form.Group controlId="formBasicVerifyPassword">
                <Form.Label>Potvrdite lozinku</Form.Label>
                <Form.Control
                    type="password"
                    placeholder="Potvrdite lozinku"
                    value={user.verifyPassword}
                    required
                    onChange={(e) => setUser({ ...user, verifyPassword: e.target.value })}
                />
            </Form.Group>

            {!isLoading && <Button variant="none" type="submit">
                Registriraj se
            </Button>}
            {isLoading && <LoadingIcons.Rings stroke="#00AEEF" strokeOpacity={0.82} height={120} width={120} />}
        </Form>
    );
}
