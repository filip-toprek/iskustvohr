import http from "../common/http-common"

async function getReviews(website, page=1) {
    console.log(process.env);
    return await http.get(`/Api/Review/${website}/?page=${page}`);
}

async function getUserReviews() {
    return await http.get(`/Api/Reviews/User/`);
}

async function addReview(data) {
    return await http.post(`/Api/Review/${data.websiteUrl}/`, {reviewText: data.reviewText, reviewScore: data.reviewScore});
}

async function addReply(data) {
    return await http.post(`/Api/Reply/${data.websiteUrl}/`, {replyText: data.reviewText, reviewId: data.reviewId});
}

async function deleteReview(id) {
    return await http.delete(`/Api/Review?id=${id}`);
}

async function editReview(data) {
    return await http.put(`/Api/Review/${data.websiteUrl}/`, {id: data.id, reviewText: data.reviewText, reviewScore: data.reviewScore});
}

const reviewService = {
    getReviews,
    getUserReviews,
    addReview,
    deleteReview,
    editReview,
    addReply
};

export default reviewService;