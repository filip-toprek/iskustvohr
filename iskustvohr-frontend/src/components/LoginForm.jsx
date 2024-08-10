import React, { useState } from 'react';
import { Form, Button } from 'react-bootstrap';
import { useNavigate, Link } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import LoadingIcons from 'react-loading-icons';

export default function LoginForm() {
    const auth = useAuth();
    const navigate = useNavigate();

    const [error, setError] = useState('');
    const [isLoading, setIsLoading] = useState(false);
    const [user, setUser] = useState({
        email: '',
        password: ''
    });

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setUser(prevUser => ({
            ...prevUser,
            [name]: value
        }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');
        setIsLoading(true);

        try {
            const response = await auth.login(user.email, user.password);

            if (response.status === 200) {
                navigate('/profil');
            }else {
                setIsLoading(false);
                if(response.response.data.error === 'Email_Not_Confirmed')
                    setError('E-pošta nije potvrđena!');
                else
                    setError('Pogrešna e-pošta ili lozinka!');
            }
        } catch (error) {
            console.error('Login error:', error);
            setError('Problem sa serverom!');
            setIsLoading(false);
        }
    };

    return (
        <Form onSubmit={handleSubmit} className='registerForm'>
            <h2>Prijava</h2>
            {error && <p className='error'>{error}</p>}
            <Form.Group controlId="formBasicEmail">
                <Form.Label>Adresa e-pošte</Form.Label>
                <Form.Control
                    type="email"
                    name="email"
                    placeholder="Unesite e-poštu"
                    value={user.email}
                    required
                    onChange={handleInputChange}
                />
            </Form.Group>

            <Form.Group controlId="formBasicPassword">
                <Form.Label>Lozinka</Form.Label>
                <Form.Control
                    type="password"
                    name="password"
                    placeholder="Lozinka"
                    value={user.password}
                    required
                    onChange={handleInputChange}
                />
                <Link to="/posalji-lozinku">Zaboravili ste lozinku?</Link><br/>
            </Form.Group>

            {!isLoading && <Button variant="none" type="submit">
                Prijavi se
            </Button>}
            {isLoading && <LoadingIcons.Rings stroke="#00AEEF" strokeOpacity={0.82} height={120} width={120} />}
        </Form>
    );
}
