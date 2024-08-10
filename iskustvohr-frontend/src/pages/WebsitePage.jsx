import React, { useState } from 'react';
import { Container, Card, Button } from 'react-bootstrap';
import { Rating } from 'react-simple-star-rating';
import websiteService from '../services/WebsiteService';
import reviewService from '../services/ReviewService';
import { useAuth } from '../context/AuthContext';
import AddReviewForm from '../components/AddReviewForm';
import ReviewCard from '../components/ReviewCard';
import Paging from '../components/Paging';
import ApplyForBusinessForm from '../components/ApplyForBusinessForm';
import { useQuery } from 'react-query';

export default function WebsitePage() {
  const user = useAuth();
  const [isEditing, setIsEditing] = useState(false);
  const [currentPage, setCurrentPage] = useState(1);
  const [isApplying, setIsApplying] = useState(false);
  const url = window.location.pathname.split('/')[1];

  const websiteQuery = useQuery('website', async () => {
    const response = await websiteService.getWebsite(url);
    return response.data;
  });

  const reviewsQuery = useQuery(['reviews', currentPage], async () => {
    const url = window.location.pathname.split('/')[1];
    const reviewsResponse = await reviewService.getReviews(url, currentPage);
    return reviewsResponse.data;
  });

  const handleToggleApplyForm = () => {
    setIsApplying(prevState => !prevState);
  };

  return (
    <Container className="mt-5">
      {websiteQuery.isLoading && <p>Loading website...</p>}
      {websiteQuery.isError && <p>Error fetching website data</p>}
      {websiteQuery.isSuccess && (
        <Card style={{border: "none"}}>
          <Card.Body className='websiteCard'>
            <div className="websiteInfo">
              <div className="websiteInfo-left">
                <img src={websiteQuery.data.photoUrl} alt="Website" className="mr-3" style={{ width: '100px', height: '100px', borderRadius: '50%' }} />
                <div className="websiteInfoInside">
                  <h4>{websiteQuery.data.name}</h4>
                  <a href={"https://" + websiteQuery.data.url} target="_blank" rel="noopener noreferrer">{websiteQuery.data.url}</a>
                  {reviewsQuery.isSuccess && <Rating initialValue={reviewsQuery.data.averageRating} allowFraction={true} allowHover={false} readonly={true} emptyColor='#333' fillColor='#00AEEF'/>}
                </div>
              </div>
              <div className="websiteInfo-right">
              <Button
                  variant='none'
                  disabled={(websiteQuery.data.isAssigned || user.role === "Business") || user.role === ""}
                  onClick={handleToggleApplyForm}
                >
                  {websiteQuery.data.isAssigned ? "Profil ima administratora" : "Postani administrator"}
                </Button>
              </div>
            </div>
          </Card.Body>
        </Card>
      )}
      {isApplying && <ApplyForBusinessForm setIsApplying={setIsApplying} />}
      {!isApplying && reviewsQuery.isSuccess && (
        <>
          {!reviewsQuery.data.list.some(review => review.user.id === user.userId) && <AddReviewForm user={user} isEditing={isEditing} fetchReviews={setCurrentPage} />}
          {reviewsQuery.data.list.length > 0 ? (
            reviewsQuery.data.list.map(review => (
              <ReviewCard websiteName={url} key={review.id} review={review} isEditing={isEditing} setIsEditing={setIsEditing} fetchReviews={setCurrentPage} />
            ))
          ) : (
            <p>Trenutno nema iskustava.</p>
          )}
          <Paging pageCount={reviewsQuery.data.pageCount} currentPage={currentPage} setCurrentPage={setCurrentPage} />
        </>
      )}
    </Container>
  );
}
