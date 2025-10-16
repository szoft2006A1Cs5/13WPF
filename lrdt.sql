-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1:3307
-- Generation Time: Oct 16, 2025 at 10:49 PM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `lrdt`
--
CREATE DATABASE IF NOT EXISTS `lrdt` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_hungarian_ci;
USE `lrdt`;

-- --------------------------------------------------------

--
-- Table structure for table `asztal`
--

CREATE TABLE `asztal` (
  `id` int(11) NOT NULL,
  `ferohely` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- Dumping data for table `asztal`
--

INSERT INTO `asztal` (`id`, `ferohely`) VALUES
(1, 2),
(2, 4),
(3, 6),
(4, 8);

-- --------------------------------------------------------

--
-- Table structure for table `fizetesimod`
--

CREATE TABLE `fizetesimod` (
  `id` int(8) NOT NULL,
  `nev` varchar(64) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- Dumping data for table `fizetesimod`
--

INSERT INTO `fizetesimod` (`id`, `nev`) VALUES
(1, 'Készpénz'),
(2, 'Bankkártya'),
(3, 'SZÉP Kártya');

-- --------------------------------------------------------

--
-- Table structure for table `pincer`
--

CREATE TABLE `pincer` (
  `id` int(11) NOT NULL,
  `nev` varchar(64) NOT NULL,
  `kep` varchar(64) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- Dumping data for table `pincer`
--

INSERT INTO `pincer` (`id`, `nev`, `kep`) VALUES
(1, 'Kovács Ádám', './img/kovacs_adam.jpg'),
(2, 'Nagy Petra', './img/nagy_petra.jpg'),
(3, 'Tóth Gábor', './img/toth_gabor.jpg');

-- --------------------------------------------------------

--
-- Table structure for table `rendeles`
--

CREATE TABLE `rendeles` (
  `id` int(11) NOT NULL,
  `datum` date NOT NULL,
  `fizetesiModId` int(8) DEFAULT NULL,
  `asztalId` int(11) NOT NULL,
  `pincerId` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- Dumping data for table `rendeles`
--

INSERT INTO `rendeles` (`id`, `datum`, `fizetesiModId`, `asztalId`, `pincerId`) VALUES
(1, '2025-10-12', 1, 1, 1),
(2, '2025-10-13', 2, 2, 2),
(3, '2025-10-13', NULL, 3, 3);

-- --------------------------------------------------------

--
-- Table structure for table `rendelestetel`
--

CREATE TABLE `rendelestetel` (
  `tetelId` int(11) NOT NULL,
  `rendelesId` int(11) NOT NULL,
  `mennyiseg` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- Dumping data for table `rendelestetel`
--

INSERT INTO `rendelestetel` (`tetelId`, `rendelesId`, `mennyiseg`) VALUES
(1, 1, 2),
(5, 1, 2),
(2, 2, 1),
(4, 2, 1),
(5, 2, 2),
(3, 3, 3);

-- --------------------------------------------------------

--
-- Table structure for table `tetel`
--

CREATE TABLE `tetel` (
  `id` int(11) NOT NULL,
  `nev` varchar(64) NOT NULL,
  `ar` int(11) NOT NULL,
  `elerheto` tinyint(1) NOT NULL,
  `kep` varchar(64) NOT NULL,
  `gyerekFelnott` tinyint(1) NOT NULL,
  `allergen` varchar(64) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- Dumping data for table `tetel`
--

INSERT INTO `tetel` (`id`, `nev`, `ar`, `elerheto`, `kep`, `gyerekFelnott`, `allergen`) VALUES
(1, 'Gulyásleves', 1800, 1, 'img/gulyasleves.jpg', 0, 'glutén'),
(2, 'Rántott sajt', 1600, 1, 'img/rantott_sajt.jpg', 0, 'tej, glutén'),
(3, 'Palacsinta', 900, 1, 'img/palacsinta.jpg', 0, 'tojás, tej, glutén'),
(4, 'Csirkemell rizssel', 2000, 1, 'img/csirkemell.jpg', 0, ''),
(5, 'Ásványvíz', 400, 1, 'img/viz.jpg', 0, ''),
(6, 'Kakaó', 500, 0, 'img/kakao.jpg', 1, 'tej');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `asztal`
--
ALTER TABLE `asztal`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `fizetesimod`
--
ALTER TABLE `fizetesimod`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `pincer`
--
ALTER TABLE `pincer`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `rendeles`
--
ALTER TABLE `rendeles`
  ADD PRIMARY KEY (`id`),
  ADD KEY `asztalId` (`asztalId`),
  ADD KEY `pincerId` (`pincerId`),
  ADD KEY `fizetesiModId` (`fizetesiModId`);

--
-- Indexes for table `rendelestetel`
--
ALTER TABLE `rendelestetel`
  ADD KEY `tetelId` (`tetelId`),
  ADD KEY `rendelesId` (`rendelesId`);

--
-- Indexes for table `tetel`
--
ALTER TABLE `tetel`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `asztal`
--
ALTER TABLE `asztal`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT for table `fizetesimod`
--
ALTER TABLE `fizetesimod`
  MODIFY `id` int(8) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT for table `pincer`
--
ALTER TABLE `pincer`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT for table `rendeles`
--
ALTER TABLE `rendeles`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;

--
-- AUTO_INCREMENT for table `tetel`
--
ALTER TABLE `tetel`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `rendeles`
--
ALTER TABLE `rendeles`
  ADD CONSTRAINT `rendeles_ibfk_1` FOREIGN KEY (`pincerId`) REFERENCES `pincer` (`id`),
  ADD CONSTRAINT `rendeles_ibfk_2` FOREIGN KEY (`asztalId`) REFERENCES `asztal` (`id`),
  ADD CONSTRAINT `rendeles_ibfk_3` FOREIGN KEY (`fizetesiModId`) REFERENCES `fizetesimod` (`id`);

--
-- Constraints for table `rendelestetel`
--
ALTER TABLE `rendelestetel`
  ADD CONSTRAINT `rendelestetel_ibfk_1` FOREIGN KEY (`tetelId`) REFERENCES `tetel` (`id`),
  ADD CONSTRAINT `rendelestetel_ibfk_2` FOREIGN KEY (`rendelesId`) REFERENCES `rendeles` (`id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
