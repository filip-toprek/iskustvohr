import { useEffect, useState } from "react";
import userService from "../services/UserService";
import { Link } from "react-router-dom";

// Component for verifying email
export default function VerifyEmailPage() {
    const [isVerified, setIsVerified] = useState(false);

    useEffect(() => {
        const token = window.location.pathname.split("/")[2];
        userService.verifyUserEmail(token)
            .then(() => setIsVerified(true))
            .catch(() => setIsVerified(false));
    }, []);

    return (
        <div className="heading_home">
            {isVerified ? (
                <>
                    <h1>Uspješno ste verificirali svoju email adresu!</h1>
                    <p>Možete se prijaviti na svoj korisnički račun.</p>
                    <Link to="/prijava">Prijavi se</Link>
                </>
            ) : (
                <>
                    <h1>Došlo je do greške prilikom verifikacije email adrese.</h1>
                    <p>Pokušajte ponovno ili kontaktirajte podršku.</p>
                </>
            )}
        </div>
    );
}
