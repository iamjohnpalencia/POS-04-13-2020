-- phpMyAdmin SQL Dump
-- version 5.0.2
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Sep 06, 2020 at 03:46 PM
-- Server version: 10.4.13-MariaDB
-- PHP Version: 7.4.7

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `pos`
--

-- --------------------------------------------------------

--
-- Table structure for table `loc_settings`
--

CREATE TABLE `loc_settings` (
  `settings_id` int(11) NOT NULL,
  `C_Server` varchar(255) NOT NULL,
  `C_Username` varchar(255) NOT NULL,
  `C_Password` varchar(255) NOT NULL,
  `C_Database` varchar(255) NOT NULL,
  `C_Port` varchar(255) NOT NULL,
  `A_Export_Path` text NOT NULL,
  `A_Tax` text NOT NULL,
  `A_SIFormat` text NOT NULL,
  `A_Terminal_No` text NOT NULL,
  `A_ZeroRated` text NOT NULL,
  `Dev_Company_Name` text NOT NULL,
  `Dev_Address` text NOT NULL,
  `Dev_Tin` text NOT NULL,
  `Dev_Accr_No` text NOT NULL,
  `Dev_Accr_Date_Issued` text NOT NULL,
  `Dev_Accr_Valid_Until` text NOT NULL,
  `Dev_PTU_No` text NOT NULL,
  `Dev_PTU_Date_Issued` text NOT NULL,
  `Dev_PTU_Valid_Until` text NOT NULL,
  `S_Zreading` text NOT NULL,
  `S_BackupInterval` text NOT NULL,
  `S_BackupDate` text NOT NULL,
  `S_Batter` text NOT NULL,
  `S_Brownie_Mix` text NOT NULL,
  `S_Upgrade_Price_Add` text NOT NULL,
  `S_Waffle_Bag` text NOT NULL,
  `S_Packets` text NOT NULL,
  `S_Update_Version` text NOT NULL,
  `P_Footer_Info` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Truncate table before insert `loc_settings`
--

TRUNCATE TABLE `loc_settings`;
--
-- Dumping data for table `loc_settings`
--

INSERT INTO `loc_settings` (`settings_id`, `C_Server`, `C_Username`, `C_Password`, `C_Database`, `C_Port`, `A_Export_Path`, `A_Tax`, `A_SIFormat`, `A_Terminal_No`, `A_ZeroRated`, `Dev_Company_Name`, `Dev_Address`, `Dev_Tin`, `Dev_Accr_No`, `Dev_Accr_Date_Issued`, `Dev_Accr_Valid_Until`, `Dev_PTU_No`, `Dev_PTU_Date_Issued`, `Dev_PTU_Valid_Until`, `S_Zreading`, `S_BackupInterval`, `S_BackupDate`, `S_Batter`, `S_Brownie_Mix`, `S_Upgrade_Price_Add`, `S_Waffle_Bag`, `S_Packets`, `S_Update_Version`, `P_Footer_Info`) VALUES
(1, 'cG9zLWNsb3VkLXJldi5jbmswMW1xd3N5eGYuYXAtc291dGhlYXN0LTEucmRzLmFtYXpvbmF3cy5jb20=', 'bWFzdGVyYWRtaW4=', 'cGFzc3dvcmQyMDE5', 'cG9zcmV2', 'MzMwNg==', 'QzpcVXNlcnNcampyZXlcRG9jdW1lbnRzXElubm92ZW50aW9u', '0.12', '0000000000', '1', '0', 'Aiolosinnovativesolutions', 'Antipolo', '0485-9564-9876-0000', '0485-9564-9876-0000', '2020-01-01', '2020-01-01', '0485-9564-9876-0000', '2020-01-01', '2020-01-01', '2020-09-06', '3', '2020-09-01', '2', '55', '10', '53', '45', 'Version 1.0.0.0', '© 2019 - Innovention Food Resources Inc.');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `loc_settings`
--
ALTER TABLE `loc_settings`
  ADD PRIMARY KEY (`settings_id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `loc_settings`
--
ALTER TABLE `loc_settings`
  MODIFY `settings_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
