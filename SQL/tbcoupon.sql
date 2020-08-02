-- phpMyAdmin SQL Dump
-- version 5.0.2
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Aug 02, 2020 at 07:34 AM
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
-- Table structure for table `tbcoupon`
--

DROP TABLE IF EXISTS `tbcoupon`;
CREATE TABLE `tbcoupon` (
  `ID` int(11) NOT NULL,
  `Couponname_` text NOT NULL,
  `Desc_` text NOT NULL,
  `Discountvalue_` text NOT NULL,
  `Referencevalue_` text NOT NULL,
  `Type` text NOT NULL,
  `Bundlebase_` text NOT NULL,
  `BBValue_` text NOT NULL,
  `Bundlepromo_` text NOT NULL,
  `BPValue_` text NOT NULL,
  `Effectivedate` text NOT NULL,
  `Expirydate` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `tbcoupon`
--

INSERT INTO `tbcoupon` (`ID`, `Couponname_`, `Desc_`, `Discountvalue_`, `Referencevalue_`, `Type`, `Bundlebase_`, `BBValue_`, `Bundlepromo_`, `BPValue_`, `Effectivedate`, `Expirydate`) VALUES
(1, 'Senior/PWD Discount 20%', 'Senior/PWD Discount 20% - Standard National Discount', '20', '', 'Percentage(w/o vat)', '', '', '', '', '01/01/2020', '31/12/9998'),
(2, 'Combo Discount', '20 percent discount', '20', 'N/A', 'Percentage(w/ vat)', 'N/A', 'N/A', 'N/A', 'N/A', '1/5/2020', '1/5/2020'),
(3, '100 OFF GC - Chinese New Year', '100 OFF Gift Certificate for the celebration of Chinese New Year', '100', '', 'Fix-1', '', '', '', '', '25/01/2020', '31/01/2020'),
(4, '50 OFF on your next waffle', '50 OFF on your next waffle if you buy with minimum amount of 500', '50', '500', 'Fix-2', '', '', '', '', '01/01/2020', '31/01/2020'),
(5, 'Free Choco Waffle', 'Free Choco Waffle if you buy Peanut Butter waffle', '', '', 'Bundle-1(Fix)', '5', '1', '3', '1', '01/01/2020', '31/12/2020'),
(6, '10 OFF ON DRINKS', 'P10 OFF on Drinks if you buy 2 choco waffles\r\n', '10', '', 'Bundle-2(Fix)', '3', '2', '37,36,60,61,66,79', '1', '01/01/2020', '31/12/2020'),
(7, '10% OFF ON YOUR 3RD WAFFLE', '10% OFF for the 3rd Waffle if you buy 2 Perfect Combination Waffle', '10', '', 'Bundle-3(%)', '2,13,14,15,16,17,18,19,20,21,22,23,24,25,26,67,68,69,70,71,72,73,74,75', '2', '1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,62,63,67,68,69,70,71,72,73,74,75,76,77,78,80,81', '1', '01/01/2020', '31/12/2020');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `tbcoupon`
--
ALTER TABLE `tbcoupon`
  ADD PRIMARY KEY (`ID`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `tbcoupon`
--
ALTER TABLE `tbcoupon`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
