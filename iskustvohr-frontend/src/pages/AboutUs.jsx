import React from 'react';
import { Container } from 'react-bootstrap';

export default function AboutUs() {
    return (
        <Container>
                <div className="privacyPolicy">
                    <h1>O nama</h1>
                    <p>Ovaj projekt je nastao kao rješenje problema na traženje različitih iskustava za nove web trgovine.</p>
                    <p>Ukoliko želite me kontaktirati iskoristite e-mail ispod.</p>
                    <p>E-mail: <a href="mailto:podrska@iskustvo.hr" style={{textDecoration: "none", color: "#00AEEF"}}>podrska@iskustvo.hr</a></p>
                </div>
        </Container>
    );
}