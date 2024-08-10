import http from "../common/http-common"


async function login(email, password) {
    return await http.post("/Api/User/Login", {
      username: email,
      password: password,
      grant_type: "password"
    }, {headers: {'Content-type': 'application/x-www-form-urlencoded'}})
  }

async function register(data) {
    return await http.post("/Api/User/Register", data);
}

async function logout() {
    return await http.get("/Api/User/Logout");
}

async function getLocations() {
    return await http.get("/Api/Location/");
}

async function getUserById(id) {
    return await http.get(`/Api/User/?id=${id}`);
}

async function updateUser(data) {
    return await http.put("/Api/User/", data);
}

async function verifyUserEmail(token) {
    return await http.get(`/Api/User/Confirm/${token}`);
}

async function resetUserPassword(data) {
    return await http.post("/Api/User/Reset", data);
}

async function changeUserPassword(data) {
    return await http.put("/Api/User/Reset", data);
}

const userService = {
    login,
    register,
    logout,
    getLocations,
    getUserById,
    updateUser,
    verifyUserEmail,
    resetUserPassword,
    changeUserPassword
};

export default userService;