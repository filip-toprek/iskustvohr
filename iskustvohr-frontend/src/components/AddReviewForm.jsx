import React, { useState } from 'react';
import { Form } from 'react-bootstrap';
import { Button } from 'react-bootstrap';
import { Rating } from 'react-simple-star-rating'
import reviewService from '../services/ReviewService';
import userService from '../services/UserService';
import { useQuery } from 'react-query';

export default function AddReviewForm({user, isEditing, fetchReviews}){
    const [reviewText, setReviewText] = useState('');
    const [rating, setRating] = useState(0);
    const [userInfo, setUserInfo] = useState({});
    const [isFormOpen, setIsFormOpen] = useState(false);

    const userQuery = useQuery('user', async () => {
      const response = await userService.getUserById(localStorage.getItem('UserId'));
      setUserInfo(response.data);
      return response.data;
    });

    const handleOpenReviewForm = () => {
        setIsFormOpen(!isFormOpen);
        var myContainer = document.getElementById("reviewForm");
        myContainer.style.flexDirection = 'column';
    };

    const handleReviewTextChange = (event) => {
        setReviewText(event.target.value);
      };
    
    const handleRating = (rate) => {
    setRating(rate)
    }

    const handleSubmit = async () => {
        if(rating === 0){
            alert('Molimo ocijenite iskustvo!');
            return;
        }
        if(reviewText === ''){
            alert('Molimo unesite iskustvo!');
            return;
        }

        reviewService.addReview({reviewText: reviewText, reviewScore: rating, websiteUrl: window.location.pathname.split('/')[1]}).then((response) => {
            if (response.status === 201) {
                setReviewText('');
                setRating(0);
                fetchReviews();
            }
            }).catch((error) => {
                console.log(error);
            });
      };

    return (user.role == "User" && !isEditing) && (<>
        <Form className='addReviewForm' id="reviewForm">
          {userQuery.isLoading && <p>Loading user...</p>}
          <div className='reviewHeader-left'>
            <img src={userInfo.profileImageUrl} alt="User" className="mr-3" style={{ width: '50px', height: '50px', borderRadius: '50%' }} />
            {isFormOpen && <h5 className='mb-0 mt-3'>{userInfo.firstName} {userInfo.lastName}</h5>}
            </div>
            {!isFormOpen && <button className="addReviewButton" onClick={handleOpenReviewForm}>Napišite svoje iskustvo</button>}
            {isFormOpen &&
          <Form.Group controlId="formReviewText">
            <Form.Control as="textarea" rows={3} value={reviewText} onChange={handleReviewTextChange} />
          </Form.Group>
          }
            <Rating onClick={handleRating} initialValue={rating} emptyColor='#333' fillColor='#00AEEF'/>
            {isFormOpen && <Button variant="none" className='button' style={{width: "100%"}} onClick={handleSubmit}>Podijeli</Button>}

        </Form>
        </>);
}