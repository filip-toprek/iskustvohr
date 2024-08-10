import React, { useState } from 'react';
import { Form, Button } from 'react-bootstrap';
import userService from '../services/UserService';

export default function NewPasswordForm() {
    const [password, setPassword] = useState({ password: '', passwordRepeat: '' });
    const [error, setError] = useState('');

    const handleInputChange = (event) => {
        const { name, value } = event.target;
        setPassword(prevPassword => ({
            ...prevPassword,
            [name]: value
        }));
    };

    const handleSubmit = async (event) => {
        event.preventDefault();

        setError('');
        if (password.password !== password.passwordRepeat) {
            setError('Lozinke se ne podudaraju!');
            return;
        }

        const user = {
            id: window.location.pathname.split('/')[2],
            password: password.password,
            passwordResetToken: window.location.pathname.split('/')[3]
        };

        try {
            const response = await userService.changeUserPassword(user);
            if (response.status === 200) {
                window.location.href = '/prijava';
            }
        } catch (error) {
            console.error('Password change error:', error);
            setError('Zahtijev za promjenu lozinke je istekao. Molimo zatražite novi zahtijev.');
        }
    };

    return (
        <Form onSubmit={handleSubmit} className='registerForm'>
            <Form.Group controlId="formBasicPassword">
                <h2>Postavljanje nove lozinke</h2>
                {error && <p className='error'>{error}</p>}
                <Form.Label>Nova lozinka</Form.Label>
                <Form.Control
                    type="password"
                    name="password"
                    placeholder="Unesite novu lozinku"
                    value={password.password}
                    onChange={handleInputChange}
                />
            </Form.Group>
            <Form.Group controlId="formBasicPasswordRepeat">
                <Form.Label>Ponovite lozinku</Form.Label>
                <Form.Control
                    type="password"
                    name="passwordRepeat"
                    placeholder="Ponovite novu lozinku"
                    value={password.passwordRepeat}
                    onChange={handleInputChange}
                />
            </Form.Group>
            <Button variant="primary" type="submit">
                Postavi lozinku
            </Button>
        </Form>
    );
}
