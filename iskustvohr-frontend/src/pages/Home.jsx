import "../style.css";
import React, { useState } from 'react';
import { Form, Button } from 'react-bootstrap';
import { useNavigate } from 'react-router-dom';
import LoadingIcons from 'react-loading-icons';
import websiteService from "../services/WebsiteService";

export default function Home() {
    const [url, setUrl] = useState('');
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState(null);
    const navigate = useNavigate();

    const handleInputChange = (e) => {
        const inputValue = e.target.value;
        setUrl(inputValue);
    };

    // Function to handle form submission
    const handleSubmit = async (e) => {
        e.preventDefault();

        // Validation
        if (url === '') {
            alert('Unesite URL web stranice!');
            return;
        }

        if (!isValidUrl(url)) {
            alert('Unesite ispravan URL web stranice!');
            return;
        }

        // Processing URL
        const websiteUrl = getFormattedUrl(url);

        // Resetting state
        setError(null);
        setIsLoading(true);

        // Fetching website data
        const urlDomain = websiteUrl.hostname;
        try {
            const response = await websiteService.getWebsite(urlDomain);
            if (response.status === 200) {
                navigate('/' + urlDomain + '/');
            }
        } catch (error) {
            setError('Web stranica nije pronađena!');
            setIsLoading(false);
        }
    };

    // Helper function to validate URL format
    const isValidUrl = (url) => {
        const urlPattern = /[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)/;
        return url.match(urlPattern);
    };

    // Helper function to format URL
    const getFormattedUrl = (url) => {
        return !url.includes('http://') && !url.includes('https://') ? new URL('https://' + url) : new URL(url);
    };

    // JSX rendering
    return (
        <div className="heading_home">
            <div className="middle">
                <h1 className="homeTitle">Iskustvo za sve</h1>
                <p>Unesite URL web stranice za koju želite provjeriti iskustva.</p>
                {isLoading && <LoadingIcons.Rings stroke="#00AEEF" strokeOpacity={0.82} height={120} width={120} />}
                {error && <p className="error">{error}</p>}
                {!isLoading && (
                    <Form onSubmit={handleSubmit} className="homeForm">
                        <Form.Control
                            className="urlInput"
                            type="text"
                            placeholder="njuskalo.hr"
                            value={url}
                            onChange={handleInputChange}
                        />
                        <Button className="homeButton" variant="none" type="submit">
                            Pretraži
                        </Button>
                    </Form>
                )}
            </div>
        </div>
    );
    }