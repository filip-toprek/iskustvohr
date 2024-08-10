import { Container } from "react-bootstrap";

export default function ContactPage() {
    return (
        <Container>
                <div className="privacyPolicy">
                    <h1>Kontakt</h1>
                    <p>Ukoliko imate bilo kakvih pitanja ili problema, slobodno nas kontaktirajte putem e-maila ili telefona.</p>
                    <p>Naša podrška je dostupna svakim radnim danom te će Vam u najraćem roku odgovoriti.</p>
                    {/*Mailto podrska@iskustvo.hr*/}
                    <p>E-mail: <a href="mailto:podrska@iskustvo.hr" style={{textDecoration: "none", color: "#00AEEF"}}>podrska@iskustvo.hr</a></p>
                </div>
        </Container>
    );
}