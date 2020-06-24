-- phpMyAdmin SQL Dump
-- version 5.0.2
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Jun 24, 2020 at 06:28 AM
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
-- Table structure for table `loc_daily_transaction`
--

CREATE TABLE `loc_daily_transaction` (
  `transaction_id` int(11) NOT NULL,
  `transaction_number` varchar(255) NOT NULL,
  `grosssales` decimal(11,2) NOT NULL,
  `totaldiscount` decimal(11,2) NOT NULL,
  `amounttendered` decimal(11,2) NOT NULL,
  `change` decimal(11,2) NOT NULL,
  `amountdue` decimal(11,2) NOT NULL,
  `vatablesales` decimal(11,2) NOT NULL,
  `vatexemptsales` decimal(11,2) NOT NULL,
  `zeroratedsales` decimal(11,2) NOT NULL,
  `vatpercentage` decimal(11,2) NOT NULL,
  `lessvat` decimal(11,2) NOT NULL,
  `transaction_type` varchar(50) NOT NULL,
  `discount_type` varchar(50) NOT NULL,
  `totaldiscountedamount` decimal(11,2) NOT NULL,
  `si_number` int(10) NOT NULL,
  `crew_id` varchar(20) NOT NULL,
  `guid` varchar(50) NOT NULL,
  `active` varchar(2) NOT NULL,
  `store_id` varchar(11) NOT NULL,
  `created_at` text NOT NULL,
  `shift` varchar(255) NOT NULL,
  `zreading` text NOT NULL,
  `synced` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `loc_daily_transaction`
--

INSERT INTO `loc_daily_transaction` (`transaction_id`, `transaction_number`, `grosssales`, `totaldiscount`, `amounttendered`, `change`, `amountdue`, `vatablesales`, `vatexemptsales`, `zeroratedsales`, `vatpercentage`, `lessvat`, `transaction_type`, `discount_type`, `totaldiscountedamount`, `si_number`, `crew_id`, `guid`, `active`, `store_id`, `created_at`, `shift`, `zreading`, `synced`) VALUES
(1, '20240611235420', '175.00', '19.64', '200.00', '56.43', '143.57', '58.04', '98.21', '0.00', '6.96', '11.79', 'Walk-In', 'Percentage', '110.00', 1, 'FBW11-3710', 'eb475414-7efd-1bb3-51b8-1b029a94cd1f', '1', '11', '2020-06-24 11:25:04', 'First Shift', '2020-06-24', 'Unsynced'),
(2, '20240611251220', '60.00', '0.00', '60.00', '0.00', '60.00', '0.00', '53.57', '0.00', '6.43', '0.00', 'Walk-In', 'Percentage', '0.00', 2, 'FBW11-3710', 'eb475414-7efd-1bb3-51b8-1b029a94cd1f', '1', '11', '2020-06-24 11:29:04', 'First Shift', '2020-06-24', 'Unsynced'),
(3, '20240611292120', '65.00', '0.00', '65.00', '0.00', '65.00', '0.00', '58.04', '0.00', '6.96', '0.00', 'Walk-In', 'Percentage', '0.00', 3, 'FBW11-3710', 'eb475414-7efd-1bb3-51b8-1b029a94cd1f', '1', '11', '2020-06-24 11:29:58', 'First Shift', '2020-06-24', 'Unsynced'),
(4, '20240611300320', '60.00', '0.00', '60.00', '0.00', '60.00', '0.00', '53.57', '0.00', '6.43', '0.00', 'Walk-In', 'N/A', '0.00', 4, 'FBW11-3710', 'eb475414-7efd-1bb3-51b8-1b029a94cd1f', '1', '11', '2020-06-24 11:34:38', 'First Shift', '2020-06-24', 'Unsynced'),
(5, '20240611345520', '65.00', '0.00', '65.00', '0.00', '65.00', '0.00', '58.04', '0.00', '6.96', '0.00', 'Walk-In', 'N/A', '0.00', 5, 'FBW11-3710', 'eb475414-7efd-1bb3-51b8-1b029a94cd1f', '1', '11', '2020-06-24 11:36:02', 'First Shift', '2020-06-24', 'Unsynced'),
(6, '20240611372920', '65.00', '0.00', '65.00', '0.00', '65.00', '0.00', '58.04', '0.00', '6.96', '0.00', 'Walk-In', 'N/A', '0.00', 6, 'FBW11-3710', 'eb475414-7efd-1bb3-51b8-1b029a94cd1f', '1', '11', '2020-06-24 11:37:34', 'First Shift', '2020-06-24', 'Unsynced'),
(7, '20240611375020', '65.00', '0.00', '65.00', '0.00', '65.00', '0.00', '58.04', '0.00', '6.96', '0.00', 'Walk-In', 'N/A', '0.00', 7, 'FBW11-3710', 'eb475414-7efd-1bb3-51b8-1b029a94cd1f', '1', '11', '2020-06-24 11:38:23', 'First Shift', '2020-06-24', 'Unsynced'),
(8, '20240611382920', '65.00', '0.00', '65.00', '0.00', '65.00', '0.00', '58.04', '0.00', '6.96', '0.00', 'Walk-In', 'N/A', '0.00', 8, 'FBW11-3710', 'eb475414-7efd-1bb3-51b8-1b029a94cd1f', '1', '11', '2020-06-24 11:39:02', 'First Shift', '2020-06-24', 'Unsynced'),
(9, '20240611391220', '175.00', '19.64', '200.00', '56.43', '143.57', '58.04', '98.21', '0.00', '6.96', '11.79', 'Walk-In', 'Percentage', '110.00', 9, 'FBW11-3710', 'eb475414-7efd-1bb3-51b8-1b029a94cd1f', '1', '11', '2020-06-24 11:39:25', 'First Shift', '2020-06-24', 'Unsynced'),
(10, '20240611393120', '175.00', '19.64', '200.00', '56.43', '143.57', '58.04', '98.21', '0.00', '6.96', '11.79', 'Walk-In', 'Percentage', '110.00', 10, 'FBW11-3710', 'eb475414-7efd-1bb3-51b8-1b029a94cd1f', '1', '11', '2020-06-24 11:48:42', 'First Shift', '2020-06-24', 'Unsynced'),
(11, '20240611494820', '175.00', '19.64', '200.00', '56.43', '143.57', '58.04', '98.21', '0.00', '6.96', '11.79', 'Walk-In', 'Percentage', '110.00', 11, 'FBW11-3710', 'eb475414-7efd-1bb3-51b8-1b029a94cd1f', '1', '11', '2020-06-24 11:49:59', 'First Shift', '2020-06-24', 'Unsynced'),
(12, '20240611500920', '175.00', '19.64', '200.00', '56.43', '143.57', '58.04', '98.21', '0.00', '6.96', '11.79', 'Walk-In', 'Percentage', '110.00', 12, 'FBW11-3710', 'eb475414-7efd-1bb3-51b8-1b029a94cd1f', '1', '11', '2020-06-24 12:02:02', 'First Shift', '2020-06-24', 'Unsynced');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `loc_daily_transaction`
--
ALTER TABLE `loc_daily_transaction`
  ADD PRIMARY KEY (`transaction_id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `loc_daily_transaction`
--
ALTER TABLE `loc_daily_transaction`
  MODIFY `transaction_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=13;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
