import React, { useState } from 'react';
import { Form, Button, Container, InputGroup } from 'react-bootstrap';
import businessService from '../services/BusinessService';

export default function ApplyForBusinessForm({ setIsApplying }) {
    const [businessEmail, setBusinessEmail] = useState('');
    const url = window.location.pathname.split('/')[1];

    const handleBusinessEmailChange = (event) => {
        const email = event.target.value;
        if (!email.includes('@')) {
            setBusinessEmail(email);
        }
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        if (!businessEmail.trim()) {
            alert('Molimo unesite email adresu!');
            return;
        }
        if(!businessEmail.includes('@')){
            alert('Email adresa nije ispravna!');
            return;
        }

        const formattedEmail = `${businessEmail}@${url}`;

        try {
            const response = await businessService.applyForBusiness({
                businessEmail: formattedEmail,
                websiteUrl: url
            });

            if (response.status === 201) {
                alert(`Zahtijev poslan na email adresu ${formattedEmail}`);
                setBusinessEmail('');
                setIsApplying(false);
            }
        } catch (error) {
            console.error('Error applying for business:', error);
            alert('Zahtjev za poslovni korisnički račun nije poslan!');
            setIsApplying(false);
        }
    };

    return (
        <Container className="mt-5">
            <Form onSubmit={handleSubmit} className='registerForm'>
                <Form.Group controlId="formApplyForBusiness">
                    <Form.Label>Vaša email adresa</Form.Label>
                    <InputGroup className="mb-3">
                        <Form.Control
                            placeholder="contact"
                            aria-label="Vaša email adresa"
                            aria-describedby="email-addon"
                            value={businessEmail}
                            onChange={handleBusinessEmailChange}
                        />
                        <InputGroup.Text id="email-addon">@{url}</InputGroup.Text>
                    </InputGroup>
                </Form.Group>
                <Button variant="primary" type="submit">Pošalji</Button>
            </Form>
        </Container>
    );
}
