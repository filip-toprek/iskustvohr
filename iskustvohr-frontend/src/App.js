import './App.css';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import NavbarComp from './components/NavbarComp';
import Home from './pages/Home';
import WebsitePage from './pages/WebsitePage';
import LoginPage from './pages/LoginPage';
import RegisterPage from './pages/RegisterPage';
import ProfilePage from './pages/ProfilePage';
import VerifyBusinessPage from './pages/VerifyBusinessPage';
import VerifyEmailPage from './pages/VerifyEmailPage';
import PasswordResetPage from './pages/PasswordResetPage';
import NewPasswordPage from './pages/NewPasswordPage';
import PrivacyPolicyPage from './pages/PrivacyPolicyPage';
import NotFoundPage from './pages/NotFoundPage';
import FooterComp from './components/FooterComp';
import ContactPage from './pages/ContactPage';
import AboutUs from './pages/AboutUs';

function App() {
  return (
    <BrowserRouter>
    <NavbarComp />
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/prijava" element={<LoginPage />} />
        <Route path="/registracija" element={<RegisterPage />} />
        <Route path="/profil" element={<ProfilePage />} />
        <Route path="/potvrdi-email/:token" element={<VerifyEmailPage />} />
        <Route path="/potvrdi-administratora/:url/:token" element={<VerifyBusinessPage />} />
        <Route path="/posalji-lozinku/" element={<PasswordResetPage />} />
        <Route path="/postavi-lozinku/:id/:token" element={<NewPasswordPage />} />
        <Route path="/politika-privatnosti" element={<PrivacyPolicyPage />} />
        <Route path="/kontakt" element={<ContactPage />} />
        <Route path="/o-nama" element={<AboutUs />} />
        <Route path="/:url" element={<WebsitePage />} />
        <Route path="*" element={<NotFoundPage />} />
      </Routes>
      <FooterComp />
    </BrowserRouter>
  );
}

export default App;
