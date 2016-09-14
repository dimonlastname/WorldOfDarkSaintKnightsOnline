-- phpMyAdmin SQL Dump
-- version 3.5.1
-- http://www.phpmyadmin.net
--
-- Хост: 127.0.0.1
-- Время создания: Сен 13 2016 г., 22:23
-- Версия сервера: 5.5.25
-- Версия PHP: 5.3.13

SET SQL_MODE="NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

--
-- База данных: `online`
--

-- --------------------------------------------------------

--
-- Структура таблицы `_characters`
--

CREATE TABLE IF NOT EXISTS `_characters` (
  `ID` bigint(11) NOT NULL AUTO_INCREMENT,
  `owner_id` int(11) NOT NULL,
  `race` int(1) NOT NULL,
  `class` int(2) NOT NULL,
  `name` varchar(16) NOT NULL,
  `sex` int(1) NOT NULL,
  `face` varchar(1000) NOT NULL,
  `equip` varchar(1000) NOT NULL,
  `exp` bigint(20) NOT NULL,
  `pos` varchar(1000) NOT NULL,
  `hp` int(11) NOT NULL,
  `mp` int(11) NOT NULL,
  `deleted` int(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`ID`),
  UNIQUE KEY `ID_3` (`ID`),
  KEY `ID` (`ID`),
  KEY `ID_2` (`ID`),
  KEY `ID_4` (`ID`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=9 ;

--
-- Дамп данных таблицы `_characters`
--

INSERT INTO `_characters` (`ID`, `owner_id`, `race`, `class`, `name`, `sex`, `face`, `equip`, `exp`, `pos`, `hp`, `mp`, `deleted`) VALUES
(1, 2, 2, 0, 'Nogebator228', 1, '{1,1,1}', '{r0101,r0201,t0001}', 12321, '{0,0,0,0}', 0, 0, 0);

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
