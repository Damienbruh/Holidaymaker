--
-- PostgreSQL database dump
--

-- Dumped from database version 16.4
-- Dumped by pg_dump version 16.4

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

DROP DATABASE IF EXISTS holiday_maker;
--
-- Name: holiday_maker; Type: DATABASE; Schema: -; Owner: postgres
--

CREATE DATABASE holiday_maker WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'English_Sweden.1252';


ALTER DATABASE holiday_maker OWNER TO postgres;

\connect holiday_maker

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
-- Name: public; Type: SCHEMA; Schema: -; Owner: postgres
--

-- *not* creating schema, since initdb creates it


ALTER SCHEMA public OWNER TO postgres;

--
-- Name: SCHEMA public; Type: COMMENT; Schema: -; Owner: postgres
--

COMMENT ON SCHEMA public IS '';


SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: addons; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.addons (
    addons_id integer NOT NULL,
    addon text NOT NULL,
    price integer NOT NULL,
    hotel_fk integer NOT NULL
);


ALTER TABLE public.addons OWNER TO postgres;

--
-- Name: addons_addons_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.addons ALTER COLUMN addons_id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.addons_addons_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: bookings; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.bookings (
    bookings_id integer NOT NULL,
    start_date timestamp without time zone,
    end_date timestamp without time zone,
    status text
);


ALTER TABLE public.bookings OWNER TO postgres;

--
-- Name: bookings_bookings_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.bookings ALTER COLUMN bookings_id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.bookings_bookings_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: bookings_join_addons; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.bookings_join_addons (
    addon_fk integer NOT NULL,
    booking_fk integer NOT NULL
);


ALTER TABLE public.bookings_join_addons OWNER TO postgres;

--
-- Name: bookings_join_customer; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.bookings_join_customer (
    customer_fk integer NOT NULL,
    booking_fk integer NOT NULL
);


ALTER TABLE public.bookings_join_customer OWNER TO postgres;

--
-- Name: bookings_join_rooms; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.bookings_join_rooms (
    rooms_fk integer NOT NULL,
    booking_fk integer NOT NULL
);


ALTER TABLE public.bookings_join_rooms OWNER TO postgres;

--
-- Name: customers; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.customers (
    customer_id integer NOT NULL,
    name text,
    email text,
    phone_number text,
    birthyear integer
);


ALTER TABLE public.customers OWNER TO postgres;

--
-- Name: COLUMN customers.name; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public.customers.name IS 'name';


--
-- Name: customers_customer_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.customers ALTER COLUMN customer_id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.customers_customer_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: hotel_features; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.hotel_features (
    hotel_features_id integer NOT NULL,
    pool boolean,
    entertainment boolean,
    resturant boolean,
    kidclub boolean
);


ALTER TABLE public.hotel_features OWNER TO postgres;

--
-- Name: hotel_features_hotel_features_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.hotel_features ALTER COLUMN hotel_features_id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.hotel_features_hotel_features_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: hotels; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.hotels (
    hotel_id integer NOT NULL,
    street_name text,
    postal_code text,
    city text,
    region text,
    country text,
    distance_to_ski_slope text,
    distance_to_town_center text,
    rating integer
);


ALTER TABLE public.hotels OWNER TO postgres;

--
-- Name: hotels_address_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.hotels ALTER COLUMN hotel_id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.hotels_address_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: hotels_join_features; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.hotels_join_features (
    feature_fk integer NOT NULL,
    hotel_fk integer NOT NULL
);


ALTER TABLE public.hotels_join_features OWNER TO postgres;

--
-- Name: rooms; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.rooms (
    room_id integer NOT NULL,
    size text,
    price integer,
    hotel_fk integer,
    room_number integer
);


ALTER TABLE public.rooms OWNER TO postgres;

--
-- Name: rooms_room_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.rooms ALTER COLUMN room_id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.rooms_room_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Data for Name: addons; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.addons (addons_id, addon, price, hotel_fk) FROM stdin;
\.


--
-- Data for Name: bookings; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.bookings (bookings_id, start_date, end_date, status) FROM stdin;
\.


--
-- Data for Name: bookings_join_addons; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.bookings_join_addons (addon_fk, booking_fk) FROM stdin;
\.


--
-- Data for Name: bookings_join_customer; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.bookings_join_customer (customer_fk, booking_fk) FROM stdin;
\.


--
-- Data for Name: bookings_join_rooms; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.bookings_join_rooms (rooms_fk, booking_fk) FROM stdin;
\.


--
-- Data for Name: customers; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.customers (customer_id, name, email, phone_number, birthyear) FROM stdin;
\.


--
-- Data for Name: hotel_features; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.hotel_features (hotel_features_id, pool, entertainment, resturant, kidclub) FROM stdin;
\.


--
-- Data for Name: hotels; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.hotels (hotel_id, street_name, postal_code, city, region, country, distance_to_ski_slope, distance_to_town_center, rating) FROM stdin;
\.


--
-- Data for Name: hotels_join_features; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.hotels_join_features (feature_fk, hotel_fk) FROM stdin;
\.


--
-- Data for Name: rooms; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.rooms (room_id, size, price, hotel_fk, room_number) FROM stdin;
\.


--
-- Name: addons_addons_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.addons_addons_id_seq', 1, false);


--
-- Name: bookings_bookings_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.bookings_bookings_id_seq', 1, false);


--
-- Name: customers_customer_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.customers_customer_id_seq', 1, false);


--
-- Name: hotel_features_hotel_features_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.hotel_features_hotel_features_id_seq', 1, false);


--
-- Name: hotels_address_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.hotels_address_id_seq', 1, false);


--
-- Name: rooms_room_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.rooms_room_id_seq', 1, false);


--
-- Name: addons addons_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.addons
    ADD CONSTRAINT addons_pk PRIMARY KEY (addons_id);


--
-- Name: bookings_join_addons bookings_join_addons_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.bookings_join_addons
    ADD CONSTRAINT bookings_join_addons_pk PRIMARY KEY (addon_fk, booking_fk);


--
-- Name: bookings_join_customer bookings_join_customer_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.bookings_join_customer
    ADD CONSTRAINT bookings_join_customer_pk PRIMARY KEY (booking_fk, customer_fk);


--
-- Name: bookings_join_rooms bookings_join_rooms_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.bookings_join_rooms
    ADD CONSTRAINT bookings_join_rooms_pk PRIMARY KEY (booking_fk, rooms_fk);


--
-- Name: bookings bookings_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.bookings
    ADD CONSTRAINT bookings_pk PRIMARY KEY (bookings_id);


--
-- Name: customers customers_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.customers
    ADD CONSTRAINT customers_pk PRIMARY KEY (customer_id);


--
-- Name: hotel_features hotel_features_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.hotel_features
    ADD CONSTRAINT hotel_features_pk PRIMARY KEY (hotel_features_id);


--
-- Name: hotels_join_features hotels_join_features_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.hotels_join_features
    ADD CONSTRAINT hotels_join_features_pk PRIMARY KEY (feature_fk, hotel_fk);


--
-- Name: hotels hotels_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.hotels
    ADD CONSTRAINT hotels_pk PRIMARY KEY (hotel_id);


--
-- Name: rooms rooms_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.rooms
    ADD CONSTRAINT rooms_pk PRIMARY KEY (room_id);


--
-- Name: addons addons_hotels_hotel_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.addons
    ADD CONSTRAINT addons_hotels_hotel_id_fk FOREIGN KEY (hotel_fk) REFERENCES public.hotels(hotel_id);


--
-- Name: bookings_join_addons bookings_join_addons_addons_addons_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.bookings_join_addons
    ADD CONSTRAINT bookings_join_addons_addons_addons_id_fk FOREIGN KEY (addon_fk) REFERENCES public.addons(addons_id);


--
-- Name: bookings_join_addons bookings_join_addons_bookings_bookings_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.bookings_join_addons
    ADD CONSTRAINT bookings_join_addons_bookings_bookings_id_fk FOREIGN KEY (booking_fk) REFERENCES public.bookings(bookings_id);


--
-- Name: bookings_join_customer bookings_join_customer_bookings_bookings_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.bookings_join_customer
    ADD CONSTRAINT bookings_join_customer_bookings_bookings_id_fk FOREIGN KEY (booking_fk) REFERENCES public.bookings(bookings_id);


--
-- Name: bookings_join_customer bookings_join_customer_customers_customer_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.bookings_join_customer
    ADD CONSTRAINT bookings_join_customer_customers_customer_id_fk FOREIGN KEY (customer_fk) REFERENCES public.customers(customer_id);


--
-- Name: bookings_join_rooms bookings_join_rooms_bookings_bookings_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.bookings_join_rooms
    ADD CONSTRAINT bookings_join_rooms_bookings_bookings_id_fk FOREIGN KEY (booking_fk) REFERENCES public.bookings(bookings_id);


--
-- Name: bookings_join_rooms bookings_join_rooms_rooms_room_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.bookings_join_rooms
    ADD CONSTRAINT bookings_join_rooms_rooms_room_id_fk FOREIGN KEY (rooms_fk) REFERENCES public.rooms(room_id);


--
-- Name: hotels_join_features hotels_join_features_hotel_features_hotel_features_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.hotels_join_features
    ADD CONSTRAINT hotels_join_features_hotel_features_hotel_features_id_fk FOREIGN KEY (feature_fk) REFERENCES public.hotel_features(hotel_features_id);


--
-- Name: hotels_join_features hotels_join_features_hotels_hotel_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.hotels_join_features
    ADD CONSTRAINT hotels_join_features_hotels_hotel_id_fk FOREIGN KEY (hotel_fk) REFERENCES public.hotels(hotel_id);


--
-- Name: rooms rooms_hotels_hotel_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.rooms
    ADD CONSTRAINT rooms_hotels_hotel_id_fk FOREIGN KEY (hotel_fk) REFERENCES public.hotels(hotel_id);


--
-- Name: SCHEMA public; Type: ACL; Schema: -; Owner: postgres
--

REVOKE USAGE ON SCHEMA public FROM PUBLIC;


--
-- PostgreSQL database dump complete
--

