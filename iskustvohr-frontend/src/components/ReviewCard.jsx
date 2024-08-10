import React, { useState } from 'react';
import { Card, Form, Button } from 'react-bootstrap';
import { Rating } from 'react-simple-star-rating';
import reviewService from '../services/ReviewService';
import AddReplyForm from './AddReplyForm';
import { useAuth } from '../context/AuthContext';

export default function ReviewCard({ review, fetchReviews, websiteName }) {
    const user = useAuth();
    const [reviewText, setReviewText] = useState('');
    const [rating, setRating] = useState(0);
    const [isReplying, setIsReplying] = useState(false);
    const [isEditing, setIsEditing] = useState(false);
    const [error, setError] = useState(null);

    const handleReviewTextChange = (event) => {
        setReviewText(event.target.value);
    };

    const handleRating = (rate) => {
        setRating(rate);
    };

    const handleSubmitEdit = (id) => {
        if (!reviewText || !rating && user.role !== 'Business') {
            setError('Molimo odaberite ocijenu ili unesite iskustvo!');
            return;
        }

        reviewService
            .editReview({
                reviewText: reviewText,
                reviewScore: rating,
                id: id,
                websiteUrl: window.location.pathname.split('/')[1],
            })
            .then((response) => {
                setIsEditing(false);
                if (response.status === 200) {
                    fetchReviews();
                }
            })
            .catch((error) => {
                setError('Došlo je do greške!');
            });
    };

    const handleDeleteReview = (id) => {
        reviewService
            .deleteReview(id)
            .then((response) => {
                if (response.status === 200) {
                    fetchReviews();
                }
            })
            .catch((error) => {
                setError('Došlo je do greške!');
            });
    };

    const handleEditReview = (text, rate) => {
        setIsEditing(!isEditing);
        !isEditing ? setReviewText(text) : setReviewText('');
        !isEditing ? setRating(rate) : setRating(0);
    };

    return (
        <Card key={review.id} className="my-3">
            <Card.Body>
                <div className="d-flex align-items-left mb-3 gap-2 flex-column">
                    <div className="reviewInfo">
                        <div className='reviewHeader'>
                            <div className='reviewHeader-left'>
                                <img
                                    src={review.user.profileImageUrl}
                                    alt="User"
                                    className="mr-3"
                                    style={{ width: '50px', height: '50px', borderRadius: '50%' }}
                                />
                                <h5 className="mb-0 mt-3">
                                    {review.user.firstName} {review.user.lastName}
                                </h5>
                            </div>
                            <div className='reviewHeader-right'>
                                <small className="mb-0 mt-3 text-muted">
                                    {new Date(review.updatedAt).toLocaleString()}{' '}
                                    {review.createdAt !== review.updatedAt && 'Edited'}
                                </small>
                            </div>
                        </div>
                        {error && <p className='error'>{error}</p>}
                    </div>
                <div className='reviewContent'>
                    {(!isEditing && review.user.id === localStorage.getItem('UserId')) || (review.user.id !== localStorage.getItem('UserId')) ? (
                        <Card.Text className="mt-0">{review.reviewText}</Card.Text>
                        ) : (
                            <Form.Control as="textarea" rows={3} value={reviewText} onChange={handleReviewTextChange} />
                            )}
                            {isEditing && review.user.id === localStorage.getItem('UserId') && (
                               <>
                                <div style={{display: "flex", flexDirection: "column", padding: "10px"}}>
                                    <Button variant="none" className='button' style={{alignSelf: "baseline"}} onClick={() => handleSubmitEdit(review.id)}>
                                        Izmijeni
                                    </Button>
                                    <Button variant="none" className='buttonBad' style={{alignSelf: "baseline", background: "#333"}} onClick={() => setIsEditing(false)}>
                                        Odustani
                                    </Button>
                                </div>
                                </>
                            )}
                            {!isEditing && 
                            <div className='reviewButtons'>
                            {review.user.id === localStorage.getItem('UserId') && (
                                <p className="deleteButton" onClick={() => handleDeleteReview(review.id)}>
                                    Obriši
                                </p>
                            )}
                            {review.user.id === localStorage.getItem('UserId') && (
                                <p className="editButton" onClick={() => handleEditReview(review.reviewText, review.reviewScore)}>
                                    Izmijeni
                                </p>
                            )}
                        </div>}
                </div>
                {(!isEditing && review.user.id === localStorage.getItem('UserId')) || (review.user.id !== localStorage.getItem('UserId')) ? (
                    <Rating initialValue={review.reviewScore} allowHover={false} readonly={true} emptyColor='#333' fillColor='#00AEEF'/>
                    ) : (
                        <Rating onClick={handleRating} initialValue={review.reviewScore} emptyColor='#333' fillColor='#00AEEF'/>
                        )}
                        {!review.reply && !isReplying && user.role === 'Business' && (
                                <Button variant="none" className='button' onClick={() => setIsReplying(true)}>
                                    Odgovori
                                </Button>
                            )}
                {!review.reply && isReplying && user.role === 'Business' && <AddReplyForm reviewId={review.id} fetchReviews={fetchReviews} setIsReplying={setIsReplying} />}
                {review.reply && (
                    <div className='reviewInfo'>
                        <hr style={{color: "#333333", opacity: "1"}}/>
                        <div className='reviewHeader'>
                            <div className='reviewHeader-left'>
                                <img src={review.reply.user.profileImageUrl} alt="User" className="mr-3" style={{ width: '50px', height: '50px', borderRadius: '50%' }} />
                                <h5 className="mb-0 mt-3">
                                    {websiteName}
                                </h5>
                                </div>
                                <div className='reviewHeader-right'>
                                    <small className="mb-0 mt-3 text-muted">
                                        {new Date(review.reply.updatedAt).toLocaleString()} {review.reply.createdAt !== review.reply.updatedAt && 'Edited'}
                                    </small>
                                </div>
                            </div>
                            <div className='reviewContent'>
                                {(!isEditing && review.reply.user.id === localStorage.getItem('UserId')) || (review.reply.user.id !== localStorage.getItem('UserId')) ? (
                        <Card.Text className="mt-0">{review.reply.replyText}</Card.Text>
                        ) : (
                            <Form.Control as="textarea" rows={3} value={reviewText} onChange={handleReviewTextChange} />
                            )}
                            {isEditing && review.reply.user.id === localStorage.getItem('UserId') && (
                                <div style={{display: "flex", flexDirection: "column", padding: "10px"}}>
                                    <Button variant="none" className='button' style={{alignSelf: "baseline"}} onClick={() => handleSubmitEdit(review.reply.id)}>
                                        Izmijeni
                                    </Button>
                                    <Button variant="none" className='buttonBad' style={{alignSelf: "baseline", background: "#333"}} onClick={() => setIsEditing(false)}>
                                        Odustani
                                    </Button>
                                </div>
                            )}
                                {!isEditing && 
                            <div className='reviewButtons'>
                            {review.reply.user.id === localStorage.getItem('UserId') && (
                                <p className="deleteButton" onClick={() => handleDeleteReview(review.reply.id)}>
                                    Obriši
                                </p>
                            )}
                            {review.reply.user.id === localStorage.getItem('UserId') && (
                                <p className="editButton" onClick={() => handleEditReview(review.reply.replyText, 0)}>
                                    Izmijeni
                                </p>
                            )}
                        </div>}
                            </div>
                        </div>
                )}
                </div>
            </Card.Body>
        </Card>
    );
}
