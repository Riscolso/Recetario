CREATE DATABASE  IF NOT EXISTS `recetario` /*!40100 DEFAULT CHARACTER SET latin1 */;
USE `recetario`;
-- MySQL dump 10.13  Distrib 8.0.22, for Win64 (x86_64)
--
-- Host: localhost    Database: recetario
-- ------------------------------------------------------
-- Server version	5.7.32-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `__efmigrationshistory`
--

DROP TABLE IF EXISTS `__efmigrationshistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(95) NOT NULL,
  `ProductVersion` varchar(32) NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `__efmigrationshistory`
--

LOCK TABLES `__efmigrationshistory` WRITE;
/*!40000 ALTER TABLE `__efmigrationshistory` DISABLE KEYS */;
/*!40000 ALTER TABLE `__efmigrationshistory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `actor`
--

DROP TABLE IF EXISTS `actor`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `actor` (
  `idActor` int(11) NOT NULL AUTO_INCREMENT,
  `UserName` varchar(256) CHARACTER SET utf8mb4 DEFAULT NULL,
  `NormalizedUserName` varchar(256) CHARACTER SET utf8mb4 DEFAULT NULL,
  `Email` varchar(256) CHARACTER SET utf8mb4 DEFAULT NULL,
  `NormalizedEmail` varchar(256) CHARACTER SET utf8mb4 DEFAULT NULL,
  `EmailConfirmed` tinyint(1) NOT NULL,
  `PasswordHash` longtext CHARACTER SET utf8mb4,
  `SecurityStamp` longtext CHARACTER SET utf8mb4,
  `ConcurrencyStamp` longtext CHARACTER SET utf8mb4,
  `PhoneNumber` longtext CHARACTER SET utf8mb4,
  `PhoneNumberConfirmed` tinyint(1) NOT NULL,
  `TwoFactorEnabled` tinyint(1) NOT NULL,
  `LockoutEnd` datetime(6) DEFAULT NULL,
  `LockoutEnabled` tinyint(1) NOT NULL,
  `AccessFailedCount` int(11) NOT NULL,
  `NombreActor` varchar(55) CHARACTER SET utf8 NOT NULL,
  `FechaNac` date NOT NULL,
  `Tipo` int(11) NOT NULL,
  PRIMARY KEY (`idActor`),
  UNIQUE KEY `UserNameIndex` (`NormalizedUserName`),
  KEY `EmailIndex` (`NormalizedEmail`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `actor`
--

LOCK TABLES `actor` WRITE;
/*!40000 ALTER TABLE `actor` DISABLE KEYS */;
/*!40000 ALTER TABLE `actor` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetroleclaims`
--

DROP TABLE IF EXISTS `aspnetroleclaims`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetroleclaims` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `RoleId` int(11) NOT NULL,
  `ClaimType` longtext CHARACTER SET utf8mb4,
  `ClaimValue` longtext CHARACTER SET utf8mb4,
  PRIMARY KEY (`Id`),
  KEY `IX_AspNetRoleClaims_RoleId` (`RoleId`),
  CONSTRAINT `FK_AspNetRoleClaims_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `aspnetroles` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetroleclaims`
--

LOCK TABLES `aspnetroleclaims` WRITE;
/*!40000 ALTER TABLE `aspnetroleclaims` DISABLE KEYS */;
/*!40000 ALTER TABLE `aspnetroleclaims` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetroles`
--

DROP TABLE IF EXISTS `aspnetroles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetroles` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(256) CHARACTER SET utf8mb4 DEFAULT NULL,
  `NormalizedName` varchar(256) CHARACTER SET utf8mb4 DEFAULT NULL,
  `ConcurrencyStamp` longtext CHARACTER SET utf8mb4,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `RoleNameIndex` (`NormalizedName`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetroles`
--

LOCK TABLES `aspnetroles` WRITE;
/*!40000 ALTER TABLE `aspnetroles` DISABLE KEYS */;
/*!40000 ALTER TABLE `aspnetroles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetuserclaims`
--

DROP TABLE IF EXISTS `aspnetuserclaims`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetuserclaims` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `UserId` int(11) NOT NULL,
  `ClaimType` longtext CHARACTER SET utf8mb4,
  `ClaimValue` longtext CHARACTER SET utf8mb4,
  PRIMARY KEY (`Id`),
  KEY `IX_AspNetUserClaims_UserId` (`UserId`),
  CONSTRAINT `FK_AspNetUserClaims_actor_UserId` FOREIGN KEY (`UserId`) REFERENCES `actor` (`idActor`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetuserclaims`
--

LOCK TABLES `aspnetuserclaims` WRITE;
/*!40000 ALTER TABLE `aspnetuserclaims` DISABLE KEYS */;
/*!40000 ALTER TABLE `aspnetuserclaims` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetuserlogins`
--

DROP TABLE IF EXISTS `aspnetuserlogins`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetuserlogins` (
  `LoginProvider` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
  `ProviderKey` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
  `ProviderDisplayName` longtext CHARACTER SET utf8mb4,
  `UserId` int(11) NOT NULL,
  PRIMARY KEY (`LoginProvider`,`ProviderKey`),
  KEY `IX_AspNetUserLogins_UserId` (`UserId`),
  CONSTRAINT `FK_AspNetUserLogins_actor_UserId` FOREIGN KEY (`UserId`) REFERENCES `actor` (`idActor`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetuserlogins`
--

LOCK TABLES `aspnetuserlogins` WRITE;
/*!40000 ALTER TABLE `aspnetuserlogins` DISABLE KEYS */;
/*!40000 ALTER TABLE `aspnetuserlogins` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetuserroles`
--

DROP TABLE IF EXISTS `aspnetuserroles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetuserroles` (
  `UserId` int(11) NOT NULL,
  `RoleId` int(11) NOT NULL,
  PRIMARY KEY (`UserId`,`RoleId`),
  KEY `IX_AspNetUserRoles_RoleId` (`RoleId`),
  CONSTRAINT `FK_AspNetUserRoles_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `aspnetroles` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_AspNetUserRoles_actor_UserId` FOREIGN KEY (`UserId`) REFERENCES `actor` (`idActor`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetuserroles`
--

LOCK TABLES `aspnetuserroles` WRITE;
/*!40000 ALTER TABLE `aspnetuserroles` DISABLE KEYS */;
/*!40000 ALTER TABLE `aspnetuserroles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetusertokens`
--

DROP TABLE IF EXISTS `aspnetusertokens`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetusertokens` (
  `UserId` int(11) NOT NULL,
  `LoginProvider` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
  `Name` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
  `Value` longtext CHARACTER SET utf8mb4,
  PRIMARY KEY (`UserId`,`LoginProvider`,`Name`),
  CONSTRAINT `FK_AspNetUserTokens_actor_UserId` FOREIGN KEY (`UserId`) REFERENCES `actor` (`idActor`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetusertokens`
--

LOCK TABLES `aspnetusertokens` WRITE;
/*!40000 ALTER TABLE `aspnetusertokens` DISABLE KEYS */;
/*!40000 ALTER TABLE `aspnetusertokens` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `etiqueta`
--

DROP TABLE IF EXISTS `etiqueta`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `etiqueta` (
  `idEtiqueta` int(11) NOT NULL AUTO_INCREMENT,
  `Etiqueta` varchar(45) CHARACTER SET utf8 NOT NULL,
  PRIMARY KEY (`idEtiqueta`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `etiqueta`
--

LOCK TABLES `etiqueta` WRITE;
/*!40000 ALTER TABLE `etiqueta` DISABLE KEYS */;
/*!40000 ALTER TABLE `etiqueta` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ingrediente`
--

DROP TABLE IF EXISTS `ingrediente`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ingrediente` (
  `idIngrediente` int(11) NOT NULL AUTO_INCREMENT,
  `Nombre` varchar(45) CHARACTER SET utf8 NOT NULL,
  PRIMARY KEY (`idIngrediente`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ingrediente`
--

LOCK TABLES `ingrediente` WRITE;
/*!40000 ALTER TABLE `ingrediente` DISABLE KEYS */;
/*!40000 ALTER TABLE `ingrediente` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `lleva`
--

DROP TABLE IF EXISTS `lleva`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `lleva` (
  `Receta_idReceta` int(11) NOT NULL,
  `Receta_Actor_idActor` int(11) NOT NULL,
  `Ingrediente_idIngrediente` int(11) NOT NULL,
  `IngredienteCrudo` varchar(45) CHARACTER SET utf8 NOT NULL DEFAULT '',
  PRIMARY KEY (`Receta_idReceta`,`Receta_Actor_idActor`,`Ingrediente_idIngrediente`),
  KEY `fk_Receta_has_Ingrediente_Ingrediente1_idx` (`Ingrediente_idIngrediente`),
  KEY `fk_Receta_has_Ingrediente_Receta1_idx` (`Receta_idReceta`,`Receta_Actor_idActor`),
  CONSTRAINT `fk_Receta_has_Ingrediente_Ingrediente1` FOREIGN KEY (`Ingrediente_idIngrediente`) REFERENCES `ingrediente` (`idIngrediente`),
  CONSTRAINT `fk_Receta_has_Ingrediente_Receta1` FOREIGN KEY (`Receta_idReceta`, `Receta_Actor_idActor`) REFERENCES `receta` (`idReceta`, `Actor_idActor`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `lleva`
--

LOCK TABLES `lleva` WRITE;
/*!40000 ALTER TABLE `lleva` DISABLE KEYS */;
/*!40000 ALTER TABLE `lleva` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `paso`
--

DROP TABLE IF EXISTS `paso`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `paso` (
  `NoPaso` int(11) NOT NULL,
  `Receta_idReceta` int(11) NOT NULL,
  `Receta_Actor_idActor` int(11) NOT NULL,
  `Texto` varchar(600) CHARACTER SET utf8 DEFAULT NULL,
  `TiempoTemporizador` int(11) DEFAULT NULL,
  PRIMARY KEY (`NoPaso`,`Receta_idReceta`,`Receta_Actor_idActor`),
  KEY `fk_Paso_Receta1_idx` (`Receta_idReceta`,`Receta_Actor_idActor`),
  CONSTRAINT `fk_Paso_Receta1` FOREIGN KEY (`Receta_idReceta`, `Receta_Actor_idActor`) REFERENCES `receta` (`idReceta`, `Actor_idActor`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `paso`
--

LOCK TABLES `paso` WRITE;
/*!40000 ALTER TABLE `paso` DISABLE KEYS */;
/*!40000 ALTER TABLE `paso` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `receta`
--

DROP TABLE IF EXISTS `receta`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `receta` (
  `idReceta` int(11) NOT NULL AUTO_INCREMENT,
  `Actor_idActor` int(11) NOT NULL,
  `Nombre` varchar(45) CHARACTER SET utf8 NOT NULL,
  `ProcentajePromedio` int(11) NOT NULL,
  `TiempoPrep` varchar(10) CHARACTER SET utf8 NOT NULL,
  PRIMARY KEY (`idReceta`,`Actor_idActor`),
  KEY `fk_Receta_Actor1_idx` (`Actor_idActor`),
  CONSTRAINT `fk_Receta_Actor1` FOREIGN KEY (`Actor_idActor`) REFERENCES `actor` (`idActor`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `receta`
--

LOCK TABLES `receta` WRITE;
/*!40000 ALTER TABLE `receta` DISABLE KEYS */;
/*!40000 ALTER TABLE `receta` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `usa`
--

DROP TABLE IF EXISTS `usa`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `usa` (
  `Receta_idReceta` int(11) NOT NULL,
  `Receta_Actor_idActor` int(11) NOT NULL,
  `Etiqueta_idEtiqueta` int(11) NOT NULL,
  PRIMARY KEY (`Receta_idReceta`,`Receta_Actor_idActor`,`Etiqueta_idEtiqueta`),
  KEY `fk_Receta_has_Etiqueta_Etiqueta1_idx` (`Etiqueta_idEtiqueta`),
  KEY `fk_Receta_has_Etiqueta_Receta1_idx` (`Receta_idReceta`,`Receta_Actor_idActor`),
  CONSTRAINT `fk_Receta_has_Etiqueta_Etiqueta1` FOREIGN KEY (`Etiqueta_idEtiqueta`) REFERENCES `etiqueta` (`idEtiqueta`),
  CONSTRAINT `fk_Receta_has_Etiqueta_Receta1` FOREIGN KEY (`Receta_idReceta`, `Receta_Actor_idActor`) REFERENCES `receta` (`idReceta`, `Actor_idActor`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `usa`
--

LOCK TABLES `usa` WRITE;
/*!40000 ALTER TABLE `usa` DISABLE KEYS */;
/*!40000 ALTER TABLE `usa` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `visualizacion`
--

DROP TABLE IF EXISTS `visualizacion`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `visualizacion` (
  `Actor_idActor` int(11) NOT NULL,
  `Receta_idReceta` int(11) NOT NULL,
  `Receta_Actor_idActor` int(11) NOT NULL,
  `ProcentajeCompl` int(11) DEFAULT NULL,
  `Calificacion` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`Actor_idActor`,`Receta_idReceta`,`Receta_Actor_idActor`),
  KEY `fk_Actor_has_Receta_Actor1_idx` (`Actor_idActor`),
  KEY `fk_Actor_has_Receta_Receta1_idx` (`Receta_idReceta`,`Receta_Actor_idActor`),
  CONSTRAINT `fk_Actor_has_Receta_Actor1` FOREIGN KEY (`Actor_idActor`) REFERENCES `actor` (`idActor`),
  CONSTRAINT `fk_Actor_has_Receta_Receta1` FOREIGN KEY (`Receta_idReceta`, `Receta_Actor_idActor`) REFERENCES `receta` (`idReceta`, `Actor_idActor`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `visualizacion`
--

LOCK TABLES `visualizacion` WRITE;
/*!40000 ALTER TABLE `visualizacion` DISABLE KEYS */;
/*!40000 ALTER TABLE `visualizacion` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2020-12-16 22:13:31
