import { createContext, useContext, useState } from 'react';
import userService from '../services/UserService';

const AuthContext = createContext();

export const useAuth = () => useContext(AuthContext);


const AuthProvider = ({ children }) => {
    const [token, setToken] = useState(localStorage.getItem("AuthToken") || "");
    const [role, setRole] = useState(localStorage.getItem("Role") || "");
    const [userId, setUserId] = useState(localStorage.getItem("UserId") || "");

    const login = async (email, password) => {
        try {
            const response = await userService.login(email, password);
            if(response.status == 200) {
                setToken(response.data.access_token);
                setUserId(response.data.userId);
                setRole(response.data.role);
                localStorage.setItem("AuthToken", response.data.access_token);
                localStorage.setItem("UserId", response.data.userId);
                localStorage.setItem("Role", response.data.role);

            }
            return response;
        }
        catch (error) {
            return error;
        }
    }

    const logout = async () => {
        try {
            const response = await userService.logout();
            if(response.status == 200) {
                setToken("");
                localStorage.removeItem("AuthToken");
                localStorage.removeItem("UserId");
                localStorage.removeItem("Role");
            }
            return response;
        }
        catch (error) {
            return error;
        }
    }

  return (
    <AuthContext.Provider value={{ token, role, userId, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};

export default AuthProvider;