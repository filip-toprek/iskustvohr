--
-- PostgreSQL database dump
--

-- Dumped from database version 16.1
-- Dumped by pg_dump version 16.1

-- Started on 2024-05-01 18:44:54

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 2 (class 3079 OID 26057)
-- Name: uuid-ossp; Type: EXTENSION; Schema: -; Owner: -
--

CREATE EXTENSION IF NOT EXISTS "uuid-ossp" WITH SCHEMA public;


--
-- TOC entry 4885 (class 0 OID 0)
-- Dependencies: 2
-- Name: EXTENSION "uuid-ossp"; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION "uuid-ossp" IS 'generate universally unique identifiers (UUIDs)';


SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 219 (class 1259 OID 18176)
-- Name: Business; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Business" (
    "Id" uuid NOT NULL,
    "WebsiteId" uuid NOT NULL,
    "IsConfirmed" boolean DEFAULT false NOT NULL,
    "EmailVerificationId" uuid DEFAULT public.uuid_generate_v4() NOT NULL,
    "BusinessEmail" character varying NOT NULL
);


ALTER TABLE public."Business" OWNER TO postgres;

--
-- TOC entry 218 (class 1259 OID 18152)
-- Name: Review; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Review" (
    "Id" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    "WebsiteId" uuid NOT NULL,
    "ReviewText" character varying NOT NULL,
    "ReviewScore" integer NOT NULL,
    "CreatedAt" timestamp without time zone NOT NULL,
    "UpdatedAt" timestamp without time zone NOT NULL,
    "CreatedBy" uuid NOT NULL,
    "UpdatedBy" uuid NOT NULL,
    "IsActive" boolean NOT NULL,
    "ReplyId" uuid,
    "IsReview" boolean DEFAULT false
);


ALTER TABLE public."Review" OWNER TO postgres;

--
-- TOC entry 216 (class 1259 OID 18012)
-- Name: Role; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Role" (
    "Id" uuid NOT NULL,
    "RoleName" character varying NOT NULL,
    "IsActive" boolean NOT NULL
);


ALTER TABLE public."Role" OWNER TO postgres;

--
-- TOC entry 220 (class 1259 OID 18212)
-- Name: User; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."User" (
    "Id" uuid NOT NULL,
    "FirstName" character varying NOT NULL,
    "LastName" character varying NOT NULL,
    "Email" character varying NOT NULL,
    "Password" character varying NOT NULL,
    "ProfileImageUrl" character varying NOT NULL,
    "RoleId" uuid NOT NULL,
    "BusinessId" uuid,
    "CreatedAt" timestamp without time zone NOT NULL,
    "UpdatedAt" timestamp without time zone NOT NULL,
    "CreatedBy" uuid NOT NULL,
    "UpdatedBy" uuid NOT NULL,
    "IsActive" boolean NOT NULL,
    "EmailConfirmed" boolean DEFAULT false NOT NULL,
    "EmailVerificationId" uuid DEFAULT public.uuid_generate_v4() NOT NULL,
    "PasswordResetToken" character varying(255),
    "PasswordResetTokenExpires" timestamp without time zone
);


ALTER TABLE public."User" OWNER TO postgres;

--
-- TOC entry 217 (class 1259 OID 18144)
-- Name: Website; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Website" (
    "Id" uuid NOT NULL,
    "Name" character varying NOT NULL,
    "PhotoUrl" character varying NOT NULL,
    "URL" character varying NOT NULL,
    "CreatedAt" timestamp without time zone NOT NULL,
    "UpdatedAt" timestamp without time zone NOT NULL,
    "IsAssigned" boolean DEFAULT false NOT NULL,
    "IsActive" boolean NOT NULL
);


ALTER TABLE public."Website" OWNER TO postgres;

--
-- TOC entry 4727 (class 2606 OID 18181)
-- Name: Business Business_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Business"
    ADD CONSTRAINT "Business_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 4725 (class 2606 OID 18158)
-- Name: Review Review_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Review"
    ADD CONSTRAINT "Review_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 4721 (class 2606 OID 18018)
-- Name: Role Role_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Role"
    ADD CONSTRAINT "Role_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 4729 (class 2606 OID 18220)
-- Name: User User_Email_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."User"
    ADD CONSTRAINT "User_Email_key" UNIQUE ("Email");


--
-- TOC entry 4731 (class 2606 OID 18218)
-- Name: User User_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."User"
    ADD CONSTRAINT "User_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 4723 (class 2606 OID 18151)
-- Name: Website Website_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Website"
    ADD CONSTRAINT "Website_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 4734 (class 2606 OID 18207)
-- Name: Business Business_WebsiteId_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Business"
    ADD CONSTRAINT "Business_WebsiteId_fkey" FOREIGN KEY ("WebsiteId") REFERENCES public."Website"("Id");


--
-- TOC entry 4732 (class 2606 OID 26048)
-- Name: Review Review_ReplyId_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Review"
    ADD CONSTRAINT "Review_ReplyId_fkey" FOREIGN KEY ("ReplyId") REFERENCES public."Review"("Id");


--
-- TOC entry 4733 (class 2606 OID 18202)
-- Name: Review Review_WebsiteId_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Review"
    ADD CONSTRAINT "Review_WebsiteId_fkey" FOREIGN KEY ("WebsiteId") REFERENCES public."Website"("Id");


--
-- TOC entry 4735 (class 2606 OID 18231)
-- Name: User User_BusinessId_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."User"
    ADD CONSTRAINT "User_BusinessId_fkey" FOREIGN KEY ("BusinessId") REFERENCES public."Business"("Id");


--
-- TOC entry 4736 (class 2606 OID 18226)
-- Name: User User_RoleId_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."User"
    ADD CONSTRAINT "User_RoleId_fkey" FOREIGN KEY ("RoleId") REFERENCES public."Role"("Id");


-- Completed on 2024-05-01 18:44:55

--
-- PostgreSQL database dump complete
--

