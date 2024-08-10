import React from 'react';
import { Container, Card } from 'react-bootstrap';
import { Rating } from 'react-simple-star-rating';
import { useQuery } from 'react-query';
import reviewService from '../services/ReviewService';
import { useNavigate } from 'react-router-dom';

// Component for displaying user's website page and reviews
export default function WebsitePage() {
  const navigate = useNavigate();

  // Fetch user's reviews
  const { data: reviews, isLoading, isError } = useQuery('userReviews', reviewService.getUserReviews);

  // Handle navigation to a specific website
  const handleUrlChange = (url) => {
    navigate('/' + url + '/');
  };

  // JSX rendering
  return (
    <Container className="mt-5">
      {/* Display user's average rating if available */}
      <Card className='my-3'>
        <Card.Body>
          <div className="d-flex align-items-center">
            {reviews && (
              <p className="mt-3">
                Moja prosječna ocijena:{' '}
                <Rating
                  initialValue={reviews?.data.averageRating}
                  allowFraction={true}
                  allowHover={false}
                  readonly={true}
                  emptyColor='#333' 
                  fillColor='#00AEEF'
                />
              </p>
            )}
          </div>
        </Card.Body>
      </Card>
      <h3 className="mt-4">Iskustva</h3>
      {/* Display user's reviews */}
      {isLoading ? (
        <p>Učitava se...</p>
      ) : isError ? (
        <p>Nema iskustava.</p>
      ) : (reviews?.data.list.map((review) => (
          <Card key={review.id} className="my-3">
            <Card.Body>
              <div className="d-flex align-items-left mb-3 gap-3 flex-column">
                {/* Display user's profile image, name, and website */}
                <div className="d-flex gap-3">
                  <img
                    src={review.user.profileImageUrl}
                    alt="User"
                    className="mr-3"
                    style={{ width: '50px', height: '50px', borderRadius: '50%' }}
                  />
                  <h5 className="mb-0 mt-3">
                    {review.user.firstName} {review.user.lastName}
                  </h5>
                  <p className="myReviewsWebsite" onClick={() => handleUrlChange(review.websiteUrl)}>
                    {review.websiteUrl}
                  </p>
                  <img
                    src={review.websitePhoto}
                    onClick={() => handleUrlChange(review.websiteUrl)}
                    alt="Website"
                    className="mr-3"
                    style={{ width: '50px', height: '50px', borderRadius: '50%' }}
                  />
                </div>
              </div>
              {/* Display review text and reply if available */}
              <div className='reviewContent'>
                <Card.Text className="mt-0">{review.reviewText}</Card.Text>
              </div>
              {/* Display review rating */}
                <div>
                  <div className="d-flex align-items-center">
                    <Rating initialValue={review.reviewScore} allowHover={false} readonly={true} emptyColor='#333' fillColor='#00AEEF'/>
                  </div>
                </div>
              {review.reply && (
                <div className='reviewInfo'>
                <hr style={{color: "#333333", opacity: "1"}}/>
                <div className='reviewHeader'>
                    <div className='reviewHeader-left'>
                        <img src={review.reply.user.profileImageUrl} alt="User" className="mr-3" style={{ width: '50px', height: '50px', borderRadius: '50%' }} />
                        <h5 className="mb-0 mt-3">
                            {review.websiteUrl}
                        </h5>
                        </div>
                        <div className='reviewHeader-right'>
                            <small className="mb-0 mt-3 text-muted">
                                {new Date(review.reply.updatedAt).toLocaleString()} {review.reply.createdAt !== review.reply.updatedAt && 'Edited'}
                            </small>
                        </div>
                    </div>
                    <div className='reviewContent'>
                        <Card.Text className="mt-0">{review.reply.replyText}</Card.Text>
                    </div>
                </div>
              )}
            </Card.Body>
          </Card>
        ))
      )}
    </Container>
  );
}
