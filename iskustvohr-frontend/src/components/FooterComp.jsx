import React from 'react';
import { Container, Row, Col } from 'react-bootstrap';
import { Link } from 'react-router-dom';

export default function FooterComp() {
    return (
        <footer className="footer">
      <Container>
        <Row>
          <Col>
            <div className="footer-section-l">
              <Link to="/politika-privatnosti">Politika privatnosti</Link>
            </div>
          </Col>
          <Col className="text-center">
            <div className="footer-section-c">
              &copy; Iskustvo.hr 2024. Sva prava pridržana
            </div>
          </Col>
          <Col className="text-right">
            <div className="footer-section-r">
              <Link to="/o-nama">O nama</Link>
              <Link to="/kontakt">Kontakt</Link>
            </div>
          </Col>
        </Row>
      </Container>
    </footer>

    );
}