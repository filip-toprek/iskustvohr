import React from 'react';
import { Container } from 'react-bootstrap';
import NewPasswordForm from '../components/NewPasswordForm';

// Component for the new password page
export default function NewPasswordPage() {
    return (
        <Container className="mt-5">
            <NewPasswordForm />
        </Container>
    );
}
