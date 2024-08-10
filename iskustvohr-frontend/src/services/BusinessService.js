import http from "../common/http-common"

async function applyForBusiness(data) {
    return await http.post(`/Api/Business/${data.websiteUrl}/`, {businessEmail: data.businessEmail}, 
    {headers: {'Authorization': `Bearer ${localStorage.getItem("AuthToken")}`}});
}

async function verifyBusiness(data) {
    return await http.get(`/Api/Business/${data.url}/${data.token}/`, 
    {headers: {'Authorization': `Bearer ${localStorage.getItem("AuthToken")}`}});
}

const businessService = {
    applyForBusiness,
    verifyBusiness
};

export default businessService;