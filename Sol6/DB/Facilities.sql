--
-- PostgreSQL database dump
--

-- Dumped from database version 14.2
-- Dumped by pg_dump version 14.1

-- Started on 2022-06-10 20:34:18

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

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 211 (class 1259 OID 16634)
-- Name: Factories; Type: TABLE; Schema: public; Owner: artakaart
--

CREATE TABLE public."Factories" (
    "Id" integer NOT NULL,
    "Name" character varying(50),
    "Description" character varying(50)
);


ALTER TABLE public."Factories" OWNER TO artakaart;

--
-- TOC entry 210 (class 1259 OID 16633)
-- Name: Factories_Id_seq; Type: SEQUENCE; Schema: public; Owner: artakaart
--

CREATE SEQUENCE public."Factories_Id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."Factories_Id_seq" OWNER TO artakaart;

--
-- TOC entry 3351 (class 0 OID 0)
-- Dependencies: 210
-- Name: Factories_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: artakaart
--

ALTER SEQUENCE public."Factories_Id_seq" OWNED BY public."Factories"."Id";


--
-- TOC entry 215 (class 1259 OID 16657)
-- Name: Tanks; Type: TABLE; Schema: public; Owner: artakaart
--

CREATE TABLE public."Tanks" (
    "Id" integer NOT NULL,
    "Name" character varying(50),
    "Volume" integer NOT NULL,
    "Maxvolume" integer NOT NULL,
    "UnitId" integer NOT NULL,
    "Description" character varying(50) DEFAULT ''::text
);


ALTER TABLE public."Tanks" OWNER TO artakaart;

--
-- TOC entry 214 (class 1259 OID 16656)
-- Name: Tanks_Id_seq; Type: SEQUENCE; Schema: public; Owner: artakaart
--

CREATE SEQUENCE public."Tanks_Id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."Tanks_Id_seq" OWNER TO artakaart;

--
-- TOC entry 3352 (class 0 OID 0)
-- Dependencies: 214
-- Name: Tanks_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: artakaart
--

ALTER SEQUENCE public."Tanks_Id_seq" OWNED BY public."Tanks"."Id";


--
-- TOC entry 213 (class 1259 OID 16643)
-- Name: Units; Type: TABLE; Schema: public; Owner: artakaart
--

CREATE TABLE public."Units" (
    "Id" integer NOT NULL,
    "Name" character varying(50),
    "FactoryId" integer NOT NULL,
    "Description" character varying(50) DEFAULT ''::text
);


ALTER TABLE public."Units" OWNER TO artakaart;

--
-- TOC entry 216 (class 1259 OID 25040)
-- Name: Test; Type: VIEW; Schema: public; Owner: postgres
--

CREATE VIEW public."Test" AS
 SELECT "Factories"."Name"
   FROM (public."Factories"
     LEFT JOIN public."Units" ON (("Factories"."Id" = "Units"."FactoryId")));


ALTER TABLE public."Test" OWNER TO postgres;

--
-- TOC entry 212 (class 1259 OID 16642)
-- Name: Units_Id_seq; Type: SEQUENCE; Schema: public; Owner: artakaart
--

CREATE SEQUENCE public."Units_Id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."Units_Id_seq" OWNER TO artakaart;

--
-- TOC entry 3353 (class 0 OID 0)
-- Dependencies: 212
-- Name: Units_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: artakaart
--

ALTER SEQUENCE public."Units_Id_seq" OWNED BY public."Units"."Id";


--
-- TOC entry 209 (class 1259 OID 16628)
-- Name: __EFMigrationsHistory; Type: TABLE; Schema: public; Owner: artakaart
--

CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);


ALTER TABLE public."__EFMigrationsHistory" OWNER TO artakaart;

--
-- TOC entry 3182 (class 2604 OID 16637)
-- Name: Factories Id; Type: DEFAULT; Schema: public; Owner: artakaart
--

ALTER TABLE ONLY public."Factories" ALTER COLUMN "Id" SET DEFAULT nextval('public."Factories_Id_seq"'::regclass);


--
-- TOC entry 3185 (class 2604 OID 16660)
-- Name: Tanks Id; Type: DEFAULT; Schema: public; Owner: artakaart
--

ALTER TABLE ONLY public."Tanks" ALTER COLUMN "Id" SET DEFAULT nextval('public."Tanks_Id_seq"'::regclass);


--
-- TOC entry 3183 (class 2604 OID 16646)
-- Name: Units Id; Type: DEFAULT; Schema: public; Owner: artakaart
--

ALTER TABLE ONLY public."Units" ALTER COLUMN "Id" SET DEFAULT nextval('public."Units_Id_seq"'::regclass);


--
-- TOC entry 3341 (class 0 OID 16634)
-- Dependencies: 211
-- Data for Name: Factories; Type: TABLE DATA; Schema: public; Owner: artakaart
--

COPY public."Factories" ("Id", "Name", "Description") FROM stdin;
1	НПЗ#1	Первый нефтеперерабатывающий завод
2	НПЗ#2	Второй нефтеперерабатывающий завод
\.


--
-- TOC entry 3345 (class 0 OID 16657)
-- Dependencies: 215
-- Data for Name: Tanks; Type: TABLE DATA; Schema: public; Owner: artakaart
--

COPY public."Tanks" ("Id", "Name", "Volume", "Maxvolume", "UnitId", "Description") FROM stdin;
19	string19	47	200	3	string19
6	Резервуар 256	62	500	3	
2	Резервуар 2	843	2000	1	
22	string	90	500	3	stringstringstringstring
1	Резервуар 1	1500	2000	1	
3	Дополнительный резервуар 24	1500	3000	2	
4	Резервуар 35	1525	3000	2	
5	Резервуар 47	500	5000	2	
\.


--
-- TOC entry 3343 (class 0 OID 16643)
-- Dependencies: 213
-- Data for Name: Units; Type: TABLE DATA; Schema: public; Owner: artakaart
--

COPY public."Units" ("Id", "Name", "FactoryId", "Description") FROM stdin;
1	ГФУ-2	1	Газофракционирующая установка
2	АВТ-6	1	Атмосферно-вакуумная трубчатка
3	АВТ-10	2	Атмосферно-вакуумная трубчатка
\.


--
-- TOC entry 3339 (class 0 OID 16628)
-- Dependencies: 209
-- Data for Name: __EFMigrationsHistory; Type: TABLE DATA; Schema: public; Owner: artakaart
--

COPY public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") FROM stdin;
20220325163145_Initial	6.0.3
20220327135524_UpdateColumNae	6.0.3
20220328155229_AddedDescription	6.0.3
20220330155342_Updated_DTO_Configuration	6.0.3
20220404142914_Upd_Context	6.0.3
\.


--
-- TOC entry 3354 (class 0 OID 0)
-- Dependencies: 210
-- Name: Factories_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: artakaart
--

SELECT pg_catalog.setval('public."Factories_Id_seq"', 4, true);


--
-- TOC entry 3355 (class 0 OID 0)
-- Dependencies: 214
-- Name: Tanks_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: artakaart
--

SELECT pg_catalog.setval('public."Tanks_Id_seq"', 22, true);


--
-- TOC entry 3356 (class 0 OID 0)
-- Dependencies: 212
-- Name: Units_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: artakaart
--

SELECT pg_catalog.setval('public."Units_Id_seq"', 19, true);


--
-- TOC entry 3190 (class 2606 OID 16641)
-- Name: Factories PK_Factories; Type: CONSTRAINT; Schema: public; Owner: artakaart
--

ALTER TABLE ONLY public."Factories"
    ADD CONSTRAINT "PK_Factories" PRIMARY KEY ("Id");


--
-- TOC entry 3196 (class 2606 OID 16664)
-- Name: Tanks PK_Tanks; Type: CONSTRAINT; Schema: public; Owner: artakaart
--

ALTER TABLE ONLY public."Tanks"
    ADD CONSTRAINT "PK_Tanks" PRIMARY KEY ("Id");


--
-- TOC entry 3193 (class 2606 OID 16650)
-- Name: Units PK_Units; Type: CONSTRAINT; Schema: public; Owner: artakaart
--

ALTER TABLE ONLY public."Units"
    ADD CONSTRAINT "PK_Units" PRIMARY KEY ("Id");


--
-- TOC entry 3188 (class 2606 OID 16632)
-- Name: __EFMigrationsHistory PK___EFMigrationsHistory; Type: CONSTRAINT; Schema: public; Owner: artakaart
--

ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");


--
-- TOC entry 3194 (class 1259 OID 16670)
-- Name: IX_Tanks_UnitId; Type: INDEX; Schema: public; Owner: artakaart
--

CREATE INDEX "IX_Tanks_UnitId" ON public."Tanks" USING btree ("UnitId");


--
-- TOC entry 3191 (class 1259 OID 16671)
-- Name: IX_Units_FactoryId; Type: INDEX; Schema: public; Owner: artakaart
--

CREATE INDEX "IX_Units_FactoryId" ON public."Units" USING btree ("FactoryId");


--
-- TOC entry 3198 (class 2606 OID 16682)
-- Name: Tanks FK_Tanks_Units_UnitId; Type: FK CONSTRAINT; Schema: public; Owner: artakaart
--

ALTER TABLE ONLY public."Tanks"
    ADD CONSTRAINT "FK_Tanks_Units_UnitId" FOREIGN KEY ("UnitId") REFERENCES public."Units"("Id") ON DELETE CASCADE;


--
-- TOC entry 3197 (class 2606 OID 16687)
-- Name: Units FK_Units_Factories_FactoryId; Type: FK CONSTRAINT; Schema: public; Owner: artakaart
--

ALTER TABLE ONLY public."Units"
    ADD CONSTRAINT "FK_Units_Factories_FactoryId" FOREIGN KEY ("FactoryId") REFERENCES public."Factories"("Id") ON DELETE CASCADE;


-- Completed on 2022-06-10 20:34:18

--
-- PostgreSQL database dump complete
--

