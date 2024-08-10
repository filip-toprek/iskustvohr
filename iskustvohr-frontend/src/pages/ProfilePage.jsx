import React, { useState } from 'react';
import { Container, Form, Button } from 'react-bootstrap';
import { useQuery } from 'react-query';
import userService from '../services/UserService';
import MyReviewsPage from './MyReviewsPage';
// Component for the user profile page
export default function ProfilePage() {
  const [user, setUser] = useState({
    id: '',
    firstName: '',
    lastName: '',
    email: '',
    profileImageUrl: ''
  });

  // Function to fetch user data
  const fetchUser = async () => {
    const response = await userService.getUserById(localStorage.getItem('UserId'));
    return response.data;
  };

  // Fetch user data using react-query
  const { data: userData, isLoading, isError } = useQuery('user', fetchUser, {
    onSuccess: (data) => {
      setUser(data);
    },
    onError: (error) => {
      console.error('Error fetching user data:', error);
    }
  });

  // Function to handle form submission
  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const response = await userService.updateUser({
        id: user.id,
        firstName: user.firstName,
        lastName: user.lastName
      });
      if (response.status === 200) {
        alert('Profile updated successfully!');
        // Refetch user data after update
        await fetchUser();
      }
    } catch (error) {
      console.error('Error updating profile:', error);
      alert('Profile update failed!');
    }
  };

  return (
    <>
    <Container className="profile">
      {isLoading && <p>Učitava se...</p>}
      {isError && <p>Greška pri dohvaćanju informacija.</p>}
      {userData && (
        <>
          <div className="text-center mb-4">
            <img
              src={userData.profileImageUrl}
              alt="Profile"
              className="rounded-circle"
              style={{ width: '150px', height: '150px'}}
            />
            <p>
              <a href="https://www.gravatar.com/" target="_blank" rel="noopener noreferrer" style={{ color: '#008FC5', textDecoration: 'none'}}>Change profile image</a>
            </p>
          </div>
          <Form onSubmit={handleSubmit} className='registerForm'>
            <h2>Vaš profil</h2>
            <Form.Group controlId="formBasicFirstName">
              <Form.Label>First Name</Form.Label>
              <Form.Control
                type="text"
                placeholder="Unesite ime"
                value={user.firstName}
                onChange={(e) => setUser({ ...user, firstName: e.target.value })}
              />
            </Form.Group>

            <Form.Group controlId="formBasicLastName">
              <Form.Label>Last Name</Form.Label>
              <Form.Control
                type="text"
                placeholder="Unesite prezime"
                value={user.lastName}
                onChange={(e) => setUser({ ...user, lastName: e.target.value })}
              />
            </Form.Group>

            <Form.Group controlId="formBasicEmail">
              <Form.Label>Email</Form.Label>
              <Form.Control type="text" plaintext readOnly value={user.email} disabled style={{background: "#33333378"}}/>
            </Form.Group>

            <Button variant="primary" type="submit">
              Save Changes
            </Button>
          </Form>
        </>
      )}
    </Container>
    <MyReviewsPage/>
    </>
  );
}
