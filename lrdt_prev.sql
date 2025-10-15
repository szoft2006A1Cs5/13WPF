-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Gép: 127.0.0.1
-- Létrehozás ideje: 2025. Okt 13. 12:53
-- Kiszolgáló verziója: 9.9.0
-- PHP verzió: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Adatbázis: `lrdt`
--
CREATE DATABASE IF NOT EXISTS `lrdt` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_hungarian_ci;
USE `lrdt`;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `asztal`
--

CREATE TABLE `asztal` (
  `id` int(11) NOT NULL,
  `ferohely` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- A tábla adatainak kiíratása `asztal`
--

INSERT INTO `asztal` (`id`, `ferohely`) VALUES
(1, 2),
(2, 4),
(3, 6),
(4, 8);

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `pincer`
--

CREATE TABLE `pincer` (
  `id` int(11) NOT NULL,
  `nev` varchar(64) NOT NULL,
  `kep` varchar(64) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- A tábla adatainak kiíratása `pincer`
--

INSERT INTO `pincer` (`id`, `nev`, `kep`) VALUES
(1, 'Kovács Ádám', 'kovacs_adam.jpg'),
(2, 'Nagy Petra', 'nagy_petra.jpg'),
(3, 'Tóth Gábor', 'toth_gabor.jpg');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `rendeles`
--

CREATE TABLE `rendeles` (
  `id` int(11) NOT NULL,
  `datum` date NOT NULL,
  `fizetesiMod` varchar(64) NOT NULL,
  `asztalId` int(11) NOT NULL,
  `pincerId` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- A tábla adatainak kiíratása `rendeles`
--

INSERT INTO `rendeles` (`id`, `datum`, `fizetesiMod`, `asztalId`, `pincerId`) VALUES
(1, '2025-10-12', 'Készpénz', 1, 1),
(2, '2025-10-13', 'Bankkártya', 2, 2),
(3, '2025-10-13', 'SZÉP kártya', 3, 3);

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `rendelestetel`
--

CREATE TABLE `rendelestetel` (
  `tetelId` int(11) NOT NULL,
  `rendelesId` int(11) NOT NULL,
  `mennyiseg` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- A tábla adatainak kiíratása `rendelestetel`
--

INSERT INTO `rendelestetel` (`tetelId`, `rendelesId`, `mennyiseg`) VALUES
(1, 1, 2),
(5, 1, 2),
(2, 2, 1),
(4, 2, 1),
(5, 2, 2),
(3, 3, 3),
(6, 3, 1);

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `tetel`
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
-- A tábla adatainak kiíratása `tetel`
--

INSERT INTO `tetel` (`id`, `nev`, `ar`, `elerheto`, `kep`, `gyerekFelnott`, `allergen`) VALUES
(1, 'Gulyásleves', 1800, 1, 'gulyasleves.jpg', 0, 'glutén'),
(2, 'Rántott sajt', 1600, 1, 'rantott_sajt.jpg', 0, 'tej, glutén'),
(3, 'Palacsinta', 900, 1, 'palacsinta.jpg', 0, 'tojás, tej, glutén'),
(4, 'Csirkemell rizssel', 2000, 1, 'csirkemell.jpg', 0, ''),
(5, 'Ásványvíz', 400, 1, 'viz.jpg', 0, ''),
(6, 'Kakaó', 500, 0, 'kakao.jpg', 1, 'tej');

--
-- Indexek a kiírt táblákhoz
--

--
-- A tábla indexei `asztal`
--
ALTER TABLE `asztal`
  ADD PRIMARY KEY (`id`);

--
-- A tábla indexei `pincer`
--
ALTER TABLE `pincer`
  ADD PRIMARY KEY (`id`);

--
-- A tábla indexei `rendeles`
--
ALTER TABLE `rendeles`
  ADD PRIMARY KEY (`id`),
  ADD KEY `asztalId` (`asztalId`),
  ADD KEY `pincerId` (`pincerId`);

--
-- A tábla indexei `rendelestetel`
--
ALTER TABLE `rendelestetel`
  ADD KEY `tetelId` (`tetelId`),
  ADD KEY `rendelesId` (`rendelesId`);

--
-- A tábla indexei `tetel`
--
ALTER TABLE `tetel`
  ADD PRIMARY KEY (`id`);

--
-- A kiírt táblák AUTO_INCREMENT értéke
--

--
-- AUTO_INCREMENT a táblához `asztal`
--
ALTER TABLE `asztal`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT a táblához `pincer`
--
ALTER TABLE `pincer`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT a táblához `rendeles`
--
ALTER TABLE `rendeles`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT a táblához `tetel`
--
ALTER TABLE `tetel`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- Megkötések a kiírt táblákhoz
--

--
-- Megkötések a táblához `rendeles`
--
ALTER TABLE `rendeles`
  ADD CONSTRAINT `rendeles_ibfk_1` FOREIGN KEY (`pincerId`) REFERENCES `pincer` (`id`),
  ADD CONSTRAINT `rendeles_ibfk_2` FOREIGN KEY (`asztalId`) REFERENCES `asztal` (`id`);

--
-- Megkötések a táblához `rendelestetel`
--
ALTER TABLE `rendelestetel`
  ADD CONSTRAINT `rendelestetel_ibfk_1` FOREIGN KEY (`tetelId`) REFERENCES `tetel` (`id`),
  ADD CONSTRAINT `rendelestetel_ibfk_2` FOREIGN KEY (`rendelesId`) REFERENCES `rendeles` (`id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
