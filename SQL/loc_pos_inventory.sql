-- phpMyAdmin SQL Dump
-- version 5.0.2
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Jun 25, 2020 at 05:45 PM
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
-- Table structure for table `loc_pos_inventory`
--

CREATE TABLE `loc_pos_inventory` (
  `inventory_id` int(11) NOT NULL,
  `store_id` varchar(11) NOT NULL,
  `formula_id` int(11) NOT NULL,
  `product_ingredients` varchar(255) NOT NULL,
  `sku` varchar(255) NOT NULL,
  `stock_primary` decimal(11,2) NOT NULL,
  `stock_secondary` decimal(11,2) NOT NULL,
  `stock_no_of_servings` decimal(11,2) NOT NULL,
  `stock_status` int(11) NOT NULL,
  `critical_limit` int(11) NOT NULL,
  `guid` varchar(255) NOT NULL,
  `created_at` text NOT NULL,
  `crew_id` varchar(50) NOT NULL,
  `synced` varchar(255) NOT NULL,
  `server_date_modified` text NOT NULL,
  `server_inventory_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `loc_pos_inventory`
--

INSERT INTO `loc_pos_inventory` (`inventory_id`, `store_id`, `formula_id`, `product_ingredients`, `sku`, `stock_primary`, `stock_secondary`, `stock_no_of_servings`, `stock_status`, `critical_limit`, `guid`, `created_at`, `crew_id`, `synced`, `server_date_modified`, `server_inventory_id`) VALUES
(1, '14', 1, 'Famous Mix', '', '202.00', '202000.00', '3030.00', 1, 20, 'eb475414-7efd-1bb3-51b8-1b029a94cd1f', '2020-04-29 10:16:42', '', 'Unsynced', '2020-04-29 10:16:42', 1),
(2, '14', 2, 'Famous Batter', '', '104.14', '203082.00', '2187.06', 1, 20, 'eb475414-7efd-1bb3-51b8-1b029a94cd1f', '2020-06-25 12:37:39', '', 'Unsynced', '2020-04-29 10:16:42', 2),
(3, '14', 3, 'Chocolate', '', '102.00', '102000.00', '4080.00', 1, 20, 'eb475414-7efd-1bb3-51b8-1b029a94cd1f', '2020-06-24 19:23:51', '', 'Unsynced', '2020-04-29 10:16:42', 3),
(4, '14', 4, 'Peanut Butter', '', '25.00', '25.00', '1000.00', 1, 20, 'eb475414-7efd-1bb3-51b8-1b029a94cd1f', '2020-06-25 12:36:18', '', 'Unsynced', '2020-04-29 10:16:42', 4),
(5, '14', 5, 'Hazelnut', '', '25.00', '75000.00', '3400.00', 1, 20, 'eb475414-7efd-1bb3-51b8-1b029a94cd1f', '2020-04-29 10:16:42', '', 'Synced', '2020-04-29 10:16:42', 5),
(6, '14', 6, 'Custard', '', '-5.00', '-5.00', '824.00', 1, 20, 'eb475414-7efd-1bb3-51b8-1b029a94cd1f', '2020-06-24 19:20:08', '', 'Unsynced', '2020-04-29 10:16:42', 6),
(7, '14', 7, 'Caramel Syrup', '', '-0.02', '-15.00', '249.00', 1, 20, 'eb475414-7efd-1bb3-51b8-1b029a94cd1f', '2020-06-24 19:23:51', '', 'Unsynced', '2020-06-22 13:06:20', 7),
(8, '14', 8, 'Maple Syrup', '', '25.00', '25.00', '1250.00', 1, 20, 'eb475414-7efd-1bb3-51b8-1b029a94cd1f', '2020-06-24 19:20:08', '', 'Unsynced', '2020-04-29 10:16:42', 8),
(9, '14', 9, 'Blueberry', '', '-0.01', '-25.00', '2998.00', 1, 20, 'eb475414-7efd-1bb3-51b8-1b029a94cd1f', '2020-06-25 12:37:41', '', 'Unsynced', '2020-05-07 12:31:41', 9),
(10, '14', 10, 'Strawberry', '', '-1.00', '-1.00', '575.00', 1, 20, 'eb475414-7efd-1bb3-51b8-1b029a94cd1f', '2020-06-24 19:23:51', '', 'Unsynced', '2020-04-29 10:16:42', 10),
(11, '14', 11, 'Mango Peach', '', '-0.10', '-65.00', '522.00', 1, 20, 'eb475414-7efd-1bb3-51b8-1b029a94cd1f', '2020-06-25 12:37:23', '', 'Unsynced', '2020-04-29 10:16:42', 11),
(12, '14', 12, 'Cream Cheese', '', '0.87', '1746.00', '14.00', 1, 20, 'eb475414-7efd-1bb3-51b8-1b029a94cd1f', '2020-06-25 12:36:36', '', 'Unsynced', '2020-04-29 10:16:42', 12),
(13, '14', 13, 'Cheddar Cheese', '', '23.99', '2015.00', '2015.00', 1, 20, 'eb475414-7efd-1bb3-51b8-1b029a94cd1f', '2020-04-29 10:16:42', '', 'Synced', '2020-04-29 10:16:42', 13),
(14, '14', 14, 'Regular Ham', '', '24.00', '24.00', '192.00', 1, 20, 'eb475414-7efd-1bb3-51b8-1b029a94cd1f', '2020-04-29 10:16:42', '', 'Synced', '2020-04-29 10:16:42', 14),
(15, '14', 15, 'Chicken Ham', '', '22.00', '22.00', '190.00', 1, 20, 'eb475414-7efd-1bb3-51b8-1b029a94cd1f', '2020-04-29 10:16:42', '', 'Synced', '2020-04-29 10:16:42', 15),
(16, '14', 16, 'Garlic Dip Mix', '', '24.00', '24.00', '240.00', 1, 20, 'eb475414-7efd-1bb3-51b8-1b029a94cd1f', '2020-04-29 10:16:42', '', 'Synced', '2020-04-29 10:16:42', 16),
(17, '14', 17, 'Vegetable Oil (Health Plus)', '', '24.00', '24.00', '744.00', 1, 20, 'eb475414-7efd-1bb3-51b8-1b029a94cd1f', '2020-04-29 10:16:42', '', 'Synced', '2020-04-29 10:16:42', 17),
(18, '14', 18, 'Famous Blends Coffee', '', '24.00', '24.00', '360.00', 1, 20, 'eb475414-7efd-1bb3-51b8-1b029a94cd1f', '2020-04-29 10:16:42', '', 'Synced', '2020-04-29 10:16:42', 18),
(19, '14', 19, 'Chekhup Choco Drink Mix', '', '-1.80', '-27.00', '357.00', 1, 20, 'eb475414-7efd-1bb3-51b8-1b029a94cd1f', '2020-06-24 18:59:23', '', 'Unsynced', '2020-04-29 10:16:42', 19),
(20, '14', 20, 'Famous Sugar Syrup', '', '24.00', '24.00', '3672.00', 1, 20, 'eb475414-7efd-1bb3-51b8-1b029a94cd1f', '2020-06-24 19:20:08', '', 'Unsynced', '2020-04-29 10:16:42', 20),
(21, '14', 21, 'French Butter', '', '24.00', '24.00', '576.00', 1, 20, 'eb475414-7efd-1bb3-51b8-1b029a94cd1f', '2020-04-29 10:16:42', '', 'Synced', '2020-04-29 10:16:42', 21),
(22, '14', 22, 'Famous Iced Mix (200g)', '', '24.00', '24.00', '600.00', 1, 20, 'eb475414-7efd-1bb3-51b8-1b029a94cd1f', '2020-04-29 10:16:42', '', 'Synced', '2020-04-29 10:16:42', 22),
(23, '14', 23, 'Famous Iced Mix (100g)', '', '24.00', '24.00', '600.00', 1, 20, 'eb475414-7efd-1bb3-51b8-1b029a94cd1f', '2020-04-29 10:16:42', '', 'Synced', '2020-04-29 10:16:42', 23),
(24, '14', 24, 'Famous Iced Mix (50g)', '', '24.00', '24.00', '600.00', 1, 20, 'eb475414-7efd-1bb3-51b8-1b029a94cd1f', '2020-04-29 10:16:42', '', 'Synced', '2020-04-29 10:16:42', 24),
(25, '14', 25, 'Famous Iced Tea (200g) ', '', '24.00', '24.00', '600.00', 1, 20, 'eb475414-7efd-1bb3-51b8-1b029a94cd1f', '2020-04-29 10:16:42', '', 'Synced', '2020-04-29 10:16:42', 25),
(26, '14', 26, 'Famous Iced Tea (100g) ', '', '24.00', '24.00', '312.00', 1, 20, 'eb475414-7efd-1bb3-51b8-1b029a94cd1f', '2020-04-29 10:16:42', '', 'Synced', '2020-04-29 10:16:42', 26),
(27, '14', 27, 'Famous Iced Tea (50g) ', '', '24.00', '24.00', '150.00', 1, 20, 'eb475414-7efd-1bb3-51b8-1b029a94cd1f', '2020-04-29 10:16:42', '', 'Synced', '2020-04-29 10:16:42', 27),
(28, '14', 28, 'Coffee Beans (80g)', '', '24.00', '24.00', '1320.00', 1, 20, 'eb475414-7efd-1bb3-51b8-1b029a94cd1f', '2020-04-29 10:16:42', '', 'Synced', '2020-04-29 10:16:42', 28),
(29, '14', 29, 'Egg', '', '24.00', '24.00', '24.00', 1, 20, 'eb475414-7efd-1bb3-51b8-1b029a94cd1f', '2020-05-09 12:19:53', '', 'Synced', '2020-05-09 12:19:53', 29),
(30, '14', 30, 'Banana', '', '1856.00', '1856.00', '1856.00', 1, 30, 'eb475414-7efd-1bb3-51b8-1b029a94cd1f', '2020-06-24 19:20:08', '', 'Unsynced', '2020-06-17 23:53:31', 30),
(31, '14', 31, 'Oreo', '', '24.00', '24.00', '24.00', 1, 20, 'eb475414-7efd-1bb3-51b8-1b029a94cd1f', '2020-05-09 12:25:16', '', 'Synced', '2020-05-09 12:25:16', 31),
(32, '14', 32, 'Century Tuna', '', '25.00', '625.00', '425.00', 1, 20, 'eb475414-7efd-1bb3-51b8-1b029a94cd1f', '2020-05-09 12:32:11', '', 'Synced', '2020-05-09 12:32:11', 32),
(33, '14', 33, 'Vanila Custard', '', '25.00', '25.00', '825.00', 1, 20, 'eb475414-7efd-1bb3-51b8-1b029a94cd1f', '2020-05-09 12:44:49', '', 'Synced', '2020-05-09 12:44:49', 33),
(34, '14', 34, 'Butter', '', '25.00', '25.00', '750.00', 1, 20, 'eb475414-7efd-1bb3-51b8-1b029a94cd1f', '2020-06-24 19:20:08', '', 'Unsynced', '2020-05-09 12:46:45', 34),
(35, '14', 35, 'Crushed Pineapple', '', '25.00', '25.00', '225.00', 1, 20, 'eb475414-7efd-1bb3-51b8-1b029a94cd1f', '2020-05-09 12:52:19', '', 'Synced', '2020-05-09 12:52:19', 35),
(36, '14', 36, 'Swift Premium Corned Beef', '', '25.00', '25.00', '625.00', 1, 20, 'eb475414-7efd-1bb3-51b8-1b029a94cd1f', '2020-05-09 12:57:03', '', 'Synced', '2020-05-09 12:57:03', 36),
(37, '14', 37, 'Water', '', '0.00', '0.00', '0.00', 1, 10, 'eb475414-7efd-1bb3-51b8-1b029a94cd1f', '2020-06-25 17:06:36', '0', 'Synced', '2020-06-25 17:06:36', 38),
(38, '14', 38, 'Mayo', '', '0.00', '0.00', '0.00', 1, 20, 'eb475414-7efd-1bb3-51b8-1b029a94cd1f', '', '0', 'Synced', '', 39);

--
-- Indexes for dumped tables
--

--
-- Indexes for table `loc_pos_inventory`
--
ALTER TABLE `loc_pos_inventory`
  ADD PRIMARY KEY (`inventory_id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `loc_pos_inventory`
--
ALTER TABLE `loc_pos_inventory`
  MODIFY `inventory_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=39;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;

