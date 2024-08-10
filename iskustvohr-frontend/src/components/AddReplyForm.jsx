import React, { useState } from 'react';
import { Form, Button } from 'react-bootstrap';
import reviewService from '../services/ReviewService';

export default function AddReplyForm({ reviewId, fetchReviews, setIsReplying }) {
    const [reviewText, setReviewText] = useState('');
    const [error, setError] = useState(null);
    const url = window.location.pathname.split('/')[1];

    const handleReviewTextChange = (event) => {
        setReviewText(event.target.value);
    }

    const handleSubmit = async (event) => {
        event.preventDefault();

        setError(null);
        if (reviewText.trim() === '') {
            setError('Molimo unesite odgovor!');
            return;
        }

        try {
            const response = await reviewService.addReply({
                reviewText: reviewText,
                reviewId: reviewId,
                websiteUrl: window.location.pathname.split('/')[1]
            });

            if (response.status === 201) {
                setReviewText('');
                setIsReplying(false);
                fetchReviews();
            }
        } catch (error) {
            setError('Na žalost došlo je do greške!');
        }
    }

    return (
        <div>
            <Form onSubmit={handleSubmit}>
                {error && <p className="error">{error}</p>}
                <Form.Group controlId="formReviewText">
                    <Form.Label>{url}</Form.Label>
                    <Form.Control
                        as="textarea"
                        rows={3}
                        value={reviewText}
                        onChange={handleReviewTextChange}
                    />
                </Form.Group>
                <Button variant="none" className='button' type="submit">Podijeli</Button>
            </Form>
        </div>
    )
}
