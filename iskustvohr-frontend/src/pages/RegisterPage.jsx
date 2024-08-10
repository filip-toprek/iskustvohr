import React from 'react';
import { Container } from 'react-bootstrap';
import RegisterForm from '../components/RegisterForm';

// Component for the register page
export default function RegisterPage() {
  return (
    <Container className="mt-5">
      <RegisterForm />
    </Container>
  );
}