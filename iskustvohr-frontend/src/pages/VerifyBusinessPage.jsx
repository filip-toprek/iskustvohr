import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import businessService from "../services/BusinessService";

// Component for verifying email
export default function VerifyBusinessPage() {
    const [isVerified, setIsVerified] = useState(false);

    useEffect(() => {
        const url = window.location.pathname.split("/")[2];
        const token = window.location.pathname.split("/")[3];
        businessService.verifyBusiness({url: url, token: token})
            .then(() => setIsVerified(true))
            .catch(() => setIsVerified(false));
    }, []);

    return (
        <div className="heading_home">
            {isVerified ? (
                <>
                    <h1>Vaš administrator račun je aktivan!</h1>
                    <p>Možete se prijaviti na svoj korisnički račun.</p>
                    <Link to="/prijava">Prijavi se</Link>
                </>
            ) : (
                <>
                    <h1>Došlo je do greške prilikom verifikacije.</h1>
                    <p>Pokušajte ponovno ili kontaktirajte podršku.</p>
                </>
            )}
        </div>
    );
}
