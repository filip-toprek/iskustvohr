import React from 'react';
import { Container } from 'react-bootstrap';
import LoginForm from '../components/LoginForm';

// Component for the login page
export default function LoginPage() {
  return (
    <Container className="mt-5">
      <LoginForm />
    </Container>
  );
}