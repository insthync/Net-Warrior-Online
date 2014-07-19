-- phpMyAdmin SQL Dump
-- version 3.2.4
-- http://www.phpmyadmin.net
--
-- โฮสต์: localhost
-- เวลาในการสร้าง: 
-- รุ่นของเซิร์ฟเวอร์: 5.1.41
-- รุ่นของ PHP: 5.3.1

SET SQL_MODE="NO_AUTO_VALUE_ON_ZERO";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

--
-- ฐานข้อมูล: `noizekingdom`
--

-- --------------------------------------------------------

--
-- โครงสร้างตาราง `char_reg_value`
--

CREATE TABLE IF NOT EXISTS `char_reg_value` (
  `charid` int(11) NOT NULL,
  `str` varchar(255) NOT NULL,
  `value` varchar(255) NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- โครงสร้างตาราง `guild_member`
--

CREATE TABLE IF NOT EXISTS `guild_member` (
  `guildid` int(11) NOT NULL,
  `charid` int(11) NOT NULL,
  `position` tinyint(2) NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- โครงสร้างตาราง `inventory`
--

CREATE TABLE IF NOT EXISTS `inventory` (
  `charid` int(11) NOT NULL,
  `inventoryidx` int(11) NOT NULL,
  `itemid` int(11) NOT NULL,
  `amount` smallint(3) NOT NULL,
  `equip` smallint(3) NOT NULL,
  `refine` smallint(3) NOT NULL,
  `attributeid` int(11) NOT NULL,
  `slot1` int(11) NOT NULL,
  `slot2` int(11) NOT NULL,
  `slot3` int(11) NOT NULL,
  `slot4` int(11) NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- โครงสร้างตาราง `user_char`
--

CREATE TABLE IF NOT EXISTS `user_char` (
  `charid` int(11) NOT NULL AUTO_INCREMENT,
  `loginid` int(11) NOT NULL,
  `name` varchar(32) NOT NULL,
  `class` tinyint(2) NOT NULL,
  `level` smallint(3) NOT NULL,
  `exp` bigint(20) NOT NULL,
  `gold` int(11) NOT NULL,
  `cur_hp` int(11) NOT NULL,
  `cur_sp` int(11) NOT NULL,
  `stpoint` int(11) NOT NULL,
  `skpoint` int(11) NOT NULL,
  `strength` smallint(4) NOT NULL,
  `dexterity` smallint(4) NOT NULL,
  `intelligent` smallint(4) NOT NULL,
  `model_hair` tinyint(2) NOT NULL,
  `model_face` tinyint(2) NOT NULL,
  `currentmap` smallint(3) NOT NULL,
  `currentmap_x` float NOT NULL,
  `currentmap_y` float NOT NULL,
  `savemap` smallint(3) NOT NULL,
  `savemap_x` float NOT NULL,
  `savemap_y` float NOT NULL,
  PRIMARY KEY (`charid`)
) ENGINE=MyISAM  DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- โครงสร้างตาราง `user_login`
--

CREATE TABLE IF NOT EXISTS `user_login` (
  `loginid` int(11) NOT NULL AUTO_INCREMENT,
  `username` varchar(32) NOT NULL,
  `password` varchar(64) NOT NULL,
  `email` varchar(128) NOT NULL,
  `level` tinyint(3) NOT NULL,
  PRIMARY KEY (`loginid`)
) ENGINE=MyISAM  DEFAULT CHARSET=latin1;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
