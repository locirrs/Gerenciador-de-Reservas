CREATE DATABASE  IF NOT EXISTS `gerenciamento_hotel` /*!40100 DEFAULT CHARACTER SET latin1 */;
USE `gerenciamento_hotel`;
-- MySQL dump 10.13  Distrib 5.6.13, for Win32 (x86)
--
-- Host: 127.0.0.1    Database: gerenciamento_hotel
-- ------------------------------------------------------
-- Server version	5.6.16

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `tb_acomodacao`
--

DROP TABLE IF EXISTS `tb_acomodacao`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `tb_acomodacao` (
  `codigo` int(11) NOT NULL AUTO_INCREMENT,
  `descricao` varchar(45) DEFAULT NULL,
  `tipo` smallint(6) DEFAULT NULL,
  `preco_diaria` varchar(45) DEFAULT NULL,
  `numeracao` varchar(15) DEFAULT NULL,
  `qtd_pessoas_adultas` int(11) DEFAULT NULL,
  `qtd_criancas` int(11) DEFAULT NULL,
  PRIMARY KEY (`codigo`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tb_acomodacao`
--

LOCK TABLES `tb_acomodacao` WRITE;
/*!40000 ALTER TABLE `tb_acomodacao` DISABLE KEYS */;
/*!40000 ALTER TABLE `tb_acomodacao` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tb_checkin`
--

DROP TABLE IF EXISTS `tb_checkin`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `tb_checkin` (
  `codigo` int(11) NOT NULL AUTO_INCREMENT,
  `data_entrada` date DEFAULT NULL,
  `hora_entrada` varchar(45) DEFAULT NULL,
  `data_saida` date DEFAULT NULL,
  `hora_saida` varchar(45) DEFAULT NULL,
  `codigo_hospede` int(11) NOT NULL,
  `codigo_acomodacao` int(11) NOT NULL,
  `valor_diaria` decimal(10,2) DEFAULT NULL,
  `codigo_funcionario` int(11) NOT NULL,
  `status` smallint(6) DEFAULT NULL,
  PRIMARY KEY (`codigo`),
  KEY `fk_tb_checkin_tb_hospede_idx` (`codigo_hospede`),
  KEY `fk_tb_checkin_tb_acomodacao_idx` (`codigo_acomodacao`),
  KEY `fk_tb_checkin_tb_funcionario_idx` (`codigo_funcionario`),
  CONSTRAINT `fk_tb_checkin_tb_acomodacao` FOREIGN KEY (`codigo_acomodacao`) REFERENCES `tb_acomodacao` (`codigo`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_tb_checkin_tb_funcionario` FOREIGN KEY (`codigo_funcionario`) REFERENCES `tb_funcionario` (`codigo`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_tb_checkin_tb_hospede` FOREIGN KEY (`codigo_hospede`) REFERENCES `tb_hospede` (`codigo`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tb_checkin`
--

LOCK TABLES `tb_checkin` WRITE;
/*!40000 ALTER TABLE `tb_checkin` DISABLE KEYS */;
/*!40000 ALTER TABLE `tb_checkin` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tb_checkout`
--

DROP TABLE IF EXISTS `tb_checkout`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `tb_checkout` (
  `codigo` int(11) NOT NULL AUTO_INCREMENT,
  `codigo_checkin` int(11) NOT NULL,
  `data_saida` date DEFAULT NULL,
  `hora_saida` varchar(45) DEFAULT NULL,
  `numero_diarias` int(11) DEFAULT NULL,
  `valor_diária` decimal(10,2) DEFAULT NULL,
  `valor_telefonemas` decimal(10,2) DEFAULT NULL,
  `valor_consumo` decimal(10,2) DEFAULT NULL,
  `valor_total` decimal(10,2) DEFAULT NULL,
  `forma_pagamento` smallint(6) DEFAULT NULL,
  PRIMARY KEY (`codigo`),
  KEY `fk_tb_checkout_tb_checkin_idx` (`codigo_checkin`),
  CONSTRAINT `fk_tb_checkout_tb_checkin` FOREIGN KEY (`codigo_checkin`) REFERENCES `tb_checkin` (`codigo`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tb_checkout`
--

LOCK TABLES `tb_checkout` WRITE;
/*!40000 ALTER TABLE `tb_checkout` DISABLE KEYS */;
/*!40000 ALTER TABLE `tb_checkout` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tb_consumo`
--

DROP TABLE IF EXISTS `tb_consumo`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `tb_consumo` (
  `codigo` int(11) NOT NULL AUTO_INCREMENT,
  `data_consumo` varchar(45) DEFAULT NULL,
  `codigo_checkin` int(11) NOT NULL,
  `codigo_item_consumo` int(11) NOT NULL,
  `quantidade` int(11) DEFAULT NULL,
  `valor_unitario` decimal(10,2) DEFAULT NULL,
  `valor_final` decimal(10,2) DEFAULT NULL,
  PRIMARY KEY (`codigo`),
  KEY `fk_tb_consumo_tb_checkin_idx` (`codigo_checkin`),
  KEY `fk_tb_consumo_tb_item_consumo_idx` (`codigo_item_consumo`),
  CONSTRAINT `fk_tb_consumo_tb_checkin` FOREIGN KEY (`codigo_checkin`) REFERENCES `tb_checkin` (`codigo`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_tb_consumo_tb_item_consumo` FOREIGN KEY (`codigo_item_consumo`) REFERENCES `tb_itens_consumo` (`codigo`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tb_consumo`
--

LOCK TABLES `tb_consumo` WRITE;
/*!40000 ALTER TABLE `tb_consumo` DISABLE KEYS */;
/*!40000 ALTER TABLE `tb_consumo` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tb_funcionario`
--

DROP TABLE IF EXISTS `tb_funcionario`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `tb_funcionario` (
  `codigo` int(11) NOT NULL AUTO_INCREMENT,
  `nome` varchar(100) DEFAULT NULL,
  `endereco` varchar(100) DEFAULT NULL,
  `telefone` varchar(45) DEFAULT NULL,
  `documento_identificacao` varchar(45) DEFAULT NULL,
  `tipo_documento_identificacao` smallint(6) DEFAULT NULL,
  `data_nascimento` date DEFAULT NULL,
  PRIMARY KEY (`codigo`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tb_funcionario`
--

LOCK TABLES `tb_funcionario` WRITE;
/*!40000 ALTER TABLE `tb_funcionario` DISABLE KEYS */;
/*!40000 ALTER TABLE `tb_funcionario` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tb_hospede`
--

DROP TABLE IF EXISTS `tb_hospede`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `tb_hospede` (
  `codigo` int(11) NOT NULL AUTO_INCREMENT,
  `nome` varchar(100) DEFAULT NULL,
  `endereco` varchar(100) DEFAULT NULL,
  `email` varchar(100) DEFAULT NULL,
  `documento_identificacao` varchar(45) DEFAULT NULL,
  `tipo_documento_identificacao` smallint(6) DEFAULT NULL,
  `data_nascimento` date DEFAULT NULL,
  `nome_pai_mae` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`codigo`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tb_hospede`
--

LOCK TABLES `tb_hospede` WRITE;
/*!40000 ALTER TABLE `tb_hospede` DISABLE KEYS */;
/*!40000 ALTER TABLE `tb_hospede` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tb_hospede_checkin`
--

DROP TABLE IF EXISTS `tb_hospede_checkin`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `tb_hospede_checkin` (
  `codigo` int(11) NOT NULL AUTO_INCREMENT,
  `codigo_checkin` int(11) NOT NULL,
  `codigo_hospede` int(11) NOT NULL,
  PRIMARY KEY (`codigo`),
  KEY `fk_tb_hospede_checkin_tb_checkin_idx` (`codigo_checkin`),
  KEY `fk_tb_hospede_checkin_tb_hospede_idx` (`codigo_hospede`),
  CONSTRAINT `fk_tb_hospede_checkin_tb_checkin` FOREIGN KEY (`codigo_checkin`) REFERENCES `tb_checkin` (`codigo`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_tb_hospede_checkin_tb_hospede` FOREIGN KEY (`codigo_hospede`) REFERENCES `tb_hospede` (`codigo`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tb_hospede_checkin`
--

LOCK TABLES `tb_hospede_checkin` WRITE;
/*!40000 ALTER TABLE `tb_hospede_checkin` DISABLE KEYS */;
/*!40000 ALTER TABLE `tb_hospede_checkin` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tb_itens_consumo`
--

DROP TABLE IF EXISTS `tb_itens_consumo`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `tb_itens_consumo` (
  `codigo` int(11) NOT NULL AUTO_INCREMENT,
  `descricao` varchar(45) DEFAULT NULL,
  `valor` decimal(10,2) DEFAULT NULL,
  PRIMARY KEY (`codigo`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tb_itens_consumo`
--

LOCK TABLES `tb_itens_consumo` WRITE;
/*!40000 ALTER TABLE `tb_itens_consumo` DISABLE KEYS */;
/*!40000 ALTER TABLE `tb_itens_consumo` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tb_reserva`
--

DROP TABLE IF EXISTS `tb_reserva`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `tb_reserva` (
  `codigo` int(11) NOT NULL AUTO_INCREMENT,
  `codigo_hospede` int(11) NOT NULL,
  `data_entrada` date DEFAULT NULL,
  `data_saida` date DEFAULT NULL,
  `codigo_acomodacao` int(11) NOT NULL,
  `qtd_adultos` int(11) DEFAULT NULL,
  `qtd_criancas` int(11) DEFAULT NULL,
  PRIMARY KEY (`codigo`),
  KEY `fk_tb_reserva_tb_hospede_idx` (`codigo_hospede`),
  KEY `fk_tb_reserva_tb_acomodacao_idx` (`codigo_acomodacao`),
  CONSTRAINT `fk_tb_reserva_tb_hospede` FOREIGN KEY (`codigo_hospede`) REFERENCES `tb_hospede` (`codigo`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_tb_reserva_tb_acomodacao` FOREIGN KEY (`codigo_acomodacao`) REFERENCES `tb_acomodacao` (`codigo`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tb_reserva`
--

LOCK TABLES `tb_reserva` WRITE;
/*!40000 ALTER TABLE `tb_reserva` DISABLE KEYS */;
/*!40000 ALTER TABLE `tb_reserva` ENABLE KEYS */;
UNLOCK TABLES;

ALTER TABLE `gerenciamento_hotel`.`tb_funcionario` 
ADD COLUMN `tipo_usuario` INT NULL AFTER `data_nascimento`;

ALTER TABLE `gerenciamento_hotel`.`tb_funcionario` 
ADD COLUMN `senha` VARCHAR(45) NULL AFTER `tipo_usuario`;


insert into tb_funcionario (nome,endereco,telefone,documento_identificacao,tipo_documento_identificacao,data_nascimento,tipo_usuario,senha) values ('master','abcd','123456','12345',1,'2000-01-01',1,'master');

--
-- Dumping events for database 'gerenciamento_hotel'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2015-04-28 18:10:19
