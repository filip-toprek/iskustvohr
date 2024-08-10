import http from "../common/http-common"

async function getWebsite(website) {
    return await http.get(`/Api/Website/${website}/`);
}

const websiteService = {
    getWebsite
};

export default websiteService;