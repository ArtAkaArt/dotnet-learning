PGDMP     	    (                z         
   Facilities    14.2    14.1 !               0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                      false                       0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                      false                       0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                      false                       1262    16566 
   Facilities    DATABASE     i   CREATE DATABASE "Facilities" WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE = 'Russian_Russia.1251';
    DROP DATABASE "Facilities";
             	   artakaart    false            �            1259    16634 	   Factories    TABLE     �   CREATE TABLE public."Factories" (
    "Id" integer NOT NULL,
    "Name" character varying(50),
    "Description" character varying(50)
);
    DROP TABLE public."Factories";
       public         heap 	   artakaart    false            �            1259    16633    Factories_Id_seq    SEQUENCE     �   CREATE SEQUENCE public."Factories_Id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 )   DROP SEQUENCE public."Factories_Id_seq";
       public       	   artakaart    false    211                       0    0    Factories_Id_seq    SEQUENCE OWNED BY     K   ALTER SEQUENCE public."Factories_Id_seq" OWNED BY public."Factories"."Id";
          public       	   artakaart    false    210            �            1259    16657    Tanks    TABLE     �   CREATE TABLE public."Tanks" (
    "Id" integer NOT NULL,
    "Name" character varying(50),
    "Volume" integer NOT NULL,
    "Maxvolume" integer NOT NULL,
    "UnitId" integer NOT NULL,
    "Description" character varying(50) DEFAULT ''::text
);
    DROP TABLE public."Tanks";
       public         heap 	   artakaart    false            �            1259    16656    Tanks_Id_seq    SEQUENCE     �   CREATE SEQUENCE public."Tanks_Id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 %   DROP SEQUENCE public."Tanks_Id_seq";
       public       	   artakaart    false    215                       0    0    Tanks_Id_seq    SEQUENCE OWNED BY     C   ALTER SEQUENCE public."Tanks_Id_seq" OWNED BY public."Tanks"."Id";
          public       	   artakaart    false    214            �            1259    16643    Units    TABLE     �   CREATE TABLE public."Units" (
    "Id" integer NOT NULL,
    "Name" character varying(50),
    "FactoryId" integer NOT NULL,
    "Description" character varying(50) DEFAULT ''::text
);
    DROP TABLE public."Units";
       public         heap 	   artakaart    false            �            1259    25040    Test    VIEW     �   CREATE VIEW public."Test" AS
 SELECT "Factories"."Name"
   FROM (public."Factories"
     LEFT JOIN public."Units" ON (("Factories"."Id" = "Units"."FactoryId")));
    DROP VIEW public."Test";
       public          postgres    false    211    213    211            �            1259    16642    Units_Id_seq    SEQUENCE     �   CREATE SEQUENCE public."Units_Id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 %   DROP SEQUENCE public."Units_Id_seq";
       public       	   artakaart    false    213                       0    0    Units_Id_seq    SEQUENCE OWNED BY     C   ALTER SEQUENCE public."Units_Id_seq" OWNED BY public."Units"."Id";
          public       	   artakaart    false    212            �            1259    16628    __EFMigrationsHistory    TABLE     �   CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);
 +   DROP TABLE public."__EFMigrationsHistory";
       public         heap 	   artakaart    false            n           2604    16637    Factories Id    DEFAULT     r   ALTER TABLE ONLY public."Factories" ALTER COLUMN "Id" SET DEFAULT nextval('public."Factories_Id_seq"'::regclass);
 ?   ALTER TABLE public."Factories" ALTER COLUMN "Id" DROP DEFAULT;
       public       	   artakaart    false    210    211    211            q           2604    16660    Tanks Id    DEFAULT     j   ALTER TABLE ONLY public."Tanks" ALTER COLUMN "Id" SET DEFAULT nextval('public."Tanks_Id_seq"'::regclass);
 ;   ALTER TABLE public."Tanks" ALTER COLUMN "Id" DROP DEFAULT;
       public       	   artakaart    false    215    214    215            o           2604    16646    Units Id    DEFAULT     j   ALTER TABLE ONLY public."Units" ALTER COLUMN "Id" SET DEFAULT nextval('public."Units_Id_seq"'::regclass);
 ;   ALTER TABLE public."Units" ALTER COLUMN "Id" DROP DEFAULT;
       public       	   artakaart    false    212    213    213                      0    16634 	   Factories 
   TABLE DATA           B   COPY public."Factories" ("Id", "Name", "Description") FROM stdin;
    public       	   artakaart    false    211   o$                 0    16657    Tanks 
   TABLE DATA           _   COPY public."Tanks" ("Id", "Name", "Volume", "Maxvolume", "UnitId", "Description") FROM stdin;
    public       	   artakaart    false    215   �$                 0    16643    Units 
   TABLE DATA           K   COPY public."Units" ("Id", "Name", "FactoryId", "Description") FROM stdin;
    public       	   artakaart    false    213   �%                 0    16628    __EFMigrationsHistory 
   TABLE DATA           R   COPY public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") FROM stdin;
    public       	   artakaart    false    209   +&                  0    0    Factories_Id_seq    SEQUENCE SET     A   SELECT pg_catalog.setval('public."Factories_Id_seq"', 1, false);
          public       	   artakaart    false    210                       0    0    Tanks_Id_seq    SEQUENCE SET     =   SELECT pg_catalog.setval('public."Tanks_Id_seq"', 22, true);
          public       	   artakaart    false    214                       0    0    Units_Id_seq    SEQUENCE SET     =   SELECT pg_catalog.setval('public."Units_Id_seq"', 19, true);
          public       	   artakaart    false    212            v           2606    16641    Factories PK_Factories 
   CONSTRAINT     Z   ALTER TABLE ONLY public."Factories"
    ADD CONSTRAINT "PK_Factories" PRIMARY KEY ("Id");
 D   ALTER TABLE ONLY public."Factories" DROP CONSTRAINT "PK_Factories";
       public         	   artakaart    false    211            |           2606    16664    Tanks PK_Tanks 
   CONSTRAINT     R   ALTER TABLE ONLY public."Tanks"
    ADD CONSTRAINT "PK_Tanks" PRIMARY KEY ("Id");
 <   ALTER TABLE ONLY public."Tanks" DROP CONSTRAINT "PK_Tanks";
       public         	   artakaart    false    215            y           2606    16650    Units PK_Units 
   CONSTRAINT     R   ALTER TABLE ONLY public."Units"
    ADD CONSTRAINT "PK_Units" PRIMARY KEY ("Id");
 <   ALTER TABLE ONLY public."Units" DROP CONSTRAINT "PK_Units";
       public         	   artakaart    false    213            t           2606    16632 .   __EFMigrationsHistory PK___EFMigrationsHistory 
   CONSTRAINT     {   ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");
 \   ALTER TABLE ONLY public."__EFMigrationsHistory" DROP CONSTRAINT "PK___EFMigrationsHistory";
       public         	   artakaart    false    209            z           1259    16670    IX_Tanks_UnitId    INDEX     I   CREATE INDEX "IX_Tanks_UnitId" ON public."Tanks" USING btree ("UnitId");
 %   DROP INDEX public."IX_Tanks_UnitId";
       public         	   artakaart    false    215            w           1259    16671    IX_Units_FactoryId    INDEX     O   CREATE INDEX "IX_Units_FactoryId" ON public."Units" USING btree ("FactoryId");
 (   DROP INDEX public."IX_Units_FactoryId";
       public         	   artakaart    false    213            ~           2606    16682    Tanks FK_Tanks_Units_UnitId    FK CONSTRAINT     �   ALTER TABLE ONLY public."Tanks"
    ADD CONSTRAINT "FK_Tanks_Units_UnitId" FOREIGN KEY ("UnitId") REFERENCES public."Units"("Id") ON DELETE CASCADE;
 I   ALTER TABLE ONLY public."Tanks" DROP CONSTRAINT "FK_Tanks_Units_UnitId";
       public       	   artakaart    false    215    3193    213            }           2606    16687 "   Units FK_Units_Factories_FactoryId    FK CONSTRAINT     �   ALTER TABLE ONLY public."Units"
    ADD CONSTRAINT "FK_Units_Factories_FactoryId" FOREIGN KEY ("FactoryId") REFERENCES public."Factories"("Id") ON DELETE CASCADE;
 P   ALTER TABLE ONLY public."Units" DROP CONSTRAINT "FK_Units_Factories_FactoryId";
       public       	   artakaart    false    3190    213    211               `   x�3�0���ӕ9��֋6]쾰S��^ ��bӅ����AxÅ�6\l*���.v^�R�(��¾[��`�����5�#۴=... @nhl         �   x�m���0F�)2��?�]�0@%��)V��6	i��N:�}�Nցv��z�ҁ�D)2�{�hb�{<��J�'/�~(K��5Y�H'D#5��y�Y�t�t��}�p��T-:<�!%�xFOxU)8����BlK[�K��r%(�Q��YΘ�0���         �   x����	�0 ��d
��N�0mR��� QK1��n#/��w���p���k�xbb�Fi�&l������b˨C�xǈl�A�#�nU�^�KL�Q�9���4��+�����,�_����[k�@K�         �   x�]ͽ
�0���\L���jGi]tB0QmR�W��ڥ���h�����;�$�w�JM
��Zdw��ؔn�>��f��ۇC��1�J^)2�"��)��|rM����F���00���ė,કRoW52L     