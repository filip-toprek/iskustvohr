import React, { useState } from 'react';
import { Form, Button } from 'react-bootstrap';
import userService from '../services/UserService';

// Component for the password reset page
export default function PasswordResetPage() {
    const [email, setEmail] = useState('');
    const [isSent, setIsSent] = useState(false);
    // Handle form submission
    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            const response = await userService.resetUserPassword({ email });
            if (response.status === 201) {
                setIsSent(true);
            }
        } catch (error) {
            console.error(error);
        }
    };

    // JSX Rendering
    return (
        <div>
            <Form onSubmit={handleSubmit} className='registerForm'>
            <div>
                <h2>Zaboravili ste lozinku?</h2>
                <p className="mt-0" style={{fontSize: "1.2em"}}>Molimo unesite Vašu e-mail adresu ispod i poslat ćemo vam link za ponovno postavljanje lozinke.</p>
            </div>
            {isSent && <div>
                <p className="m-0" style={{fontSize: "1em", color: "#00AEEF"}}>Uspješno poslan zahtijev. Molimo provjerite vašu e-mail poštu.</p>
            </div>}
                <Form.Group controlId="formBasicEmail">
                    <Form.Label>Adresa e-pošte</Form.Label>
                    <Form.Control
                        type="email"
                        placeholder="Unesite e-poštu"
                        value={email}
                        required
                        onChange={(e) => setEmail(e.target.value)}
                    />
                </Form.Group>
                <Button variant='none' type="submit">Pošalji zahtjev za reset lozinke</Button>
            </Form>
        </div>
    );
}
