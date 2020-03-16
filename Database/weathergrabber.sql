﻿--
-- Script was generated by Devart dbForge Studio 2019 for MySQL, Version 8.2.23.0
-- Product home page: http://www.devart.com/dbforge/mysql/studio
-- Script date 17.03.2020 0:16:04
-- Server version: 5.6.47-log
-- Client version: 4.1
--

-- 
-- Disable foreign keys
-- 
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;

-- 
-- Set SQL mode
-- 
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- 
-- Set character set the client will use to send SQL statements to the server
--
SET NAMES 'utf8';

DROP DATABASE IF EXISTS weathergrabber;

CREATE DATABASE IF NOT EXISTS weathergrabber
	CHARACTER SET latin1
	COLLATE latin1_swedish_ci;

--
-- Set default database
--
USE weathergrabber;

--
-- Create table `value_type`
--
CREATE TABLE IF NOT EXISTS value_type (
  Id INT(11) NOT NULL AUTO_INCREMENT,
  Value VARCHAR(50) DEFAULT NULL,
  Created DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (Id)
)
ENGINE = INNODB,
AUTO_INCREMENT = 28,
AVG_ROW_LENGTH = 630,
CHARACTER SET latin1,
COLLATE latin1_swedish_ci;

--
-- Create table `value`
--
CREATE TABLE IF NOT EXISTS value (
  Id INT(11) UNSIGNED NOT NULL AUTO_INCREMENT,
  ForecastId INT(11) UNSIGNED NOT NULL,
  ValueTypeId INT(11) NOT NULL,
  Value VARCHAR(255) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  Created DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (Id)
)
ENGINE = INNODB,
AUTO_INCREMENT = 108811,
CHARACTER SET latin1,
COLLATE latin1_swedish_ci;

--
-- Create table `forecast`
--
CREATE TABLE IF NOT EXISTS forecast (
  Id INT(11) UNSIGNED NOT NULL AUTO_INCREMENT,
  Date DATE NOT NULL,
  CityId INT(11) NOT NULL,
  Created DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (Id)
)
ENGINE = INNODB,
AUTO_INCREMENT = 6508,
CHARACTER SET latin1,
COLLATE latin1_swedish_ci;

--
-- Create table `cities`
--
CREATE TABLE IF NOT EXISTS cities (
  Id INT(11) NOT NULL AUTO_INCREMENT,
  Name VARCHAR(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  Created DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (Id)
)
ENGINE = INNODB,
AUTO_INCREMENT = 534,
CHARACTER SET latin1,
COLLATE latin1_swedish_ci;

--
-- Create index `Name` on table `cities`
--
ALTER TABLE cities 
  ADD UNIQUE INDEX Name(Name);

-- 
-- Dumping data for table value_type
--
INSERT INTO value_type VALUES
(1, 'Date', '2020-03-11 00:04:18'),
(2, 'Tip', '2020-03-11 00:04:18'),
(3, 'C max', '2020-03-11 00:04:18'),
(4, 'F max', '2020-03-11 00:04:18'),
(5, 'C min', '2020-03-11 00:04:18'),
(6, 'F min', '2020-03-11 00:04:18'),
(7, 'WF ms', '2020-03-11 00:04:18'),
(8, 'WF mih', '2020-03-11 00:04:18'),
(9, 'WF kmh', '2020-03-11 00:04:18'),
(10, 'Precipitation', '2020-03-11 00:04:18'),
(11, 'C Average', '2020-03-16 21:03:41'),
(12, 'F Average', '2020-03-16 21:03:42'),
(13, 'W Direction', '2020-03-16 21:03:42'),
(14, 'Gust ms', '2020-03-16 21:03:42'),
(15, 'Gust mih', '2020-03-16 21:03:42'),
(16, 'Gust kmh', '2020-03-16 21:03:42'),
(17, 'Pressure min', '2020-03-16 21:03:42'),
(18, 'Pressure max', '2020-03-16 21:03:42'),
(19, 'Humidity', '2020-03-16 21:03:42'),
(20, 'Uvb', '2020-03-16 21:03:42'),
(21, 'GeoM', '2020-03-16 21:03:42'),
(22, 'mm_hg_atm min', '2020-03-16 21:30:44'),
(23, 'mm_hg_atm max', '2020-03-16 21:30:44'),
(24, 'h_pa min', '2020-03-16 21:30:44'),
(25, 'h_pa max', '2020-03-16 21:30:44'),
(26, 'in_hg min', '2020-03-16 21:30:44'),
(27, 'in_hg max', '2020-03-16 21:30:44');

-- 
-- Dumping data for table value
--
-- Table weathergrabber.value does not contain any data (it is empty)

-- 
-- Dumping data for table forecast
--
-- Table weathergrabber.forecast does not contain any data (it is empty)

-- 
-- Dumping data for table cities
--
-- Table weathergrabber.cities does not contain any data (it is empty)

-- 
-- Restore previous SQL mode
-- 
/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;

-- 
-- Enable foreign keys
-- 
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;