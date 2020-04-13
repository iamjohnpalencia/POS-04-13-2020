-- phpMyAdmin SQL Dump
-- version 5.0.2
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Apr 10, 2020 at 08:34 PM
-- Server version: 10.4.11-MariaDB
-- PHP Version: 7.4.4

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
-- Table structure for table `admin_coupon`
--

CREATE TABLE `admin_coupon` (
  `coupon_id` int(11) NOT NULL,
  `coupon_code` varchar(10) NOT NULL,
  `coupon_name` varchar(50) NOT NULL,
  `coupon_val` int(11) NOT NULL,
  `coupon_desc` varchar(50) NOT NULL,
  `discount_type` varchar(50) NOT NULL,
  `effective_date` varchar(255) NOT NULL,
  `expiry_date` varchar(255) NOT NULL,
  `created_at` date NOT NULL,
  `updated_at` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `origin` varchar(20) NOT NULL,
  `active` tinyint(2) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `admin_masterlist`
--

CREATE TABLE `admin_masterlist` (
  `masterlist_id` int(11) NOT NULL,
  `masterlist_username` varchar(255) NOT NULL,
  `masterlist_password` varchar(255) NOT NULL,
  `client_ipadd` varchar(50) NOT NULL,
  `client_guid` varchar(255) NOT NULL,
  `client_product_key` varchar(255) NOT NULL,
  `user_id` varchar(11) NOT NULL,
  `active` tinyint(2) NOT NULL,
  `created_at` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `client_store_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `admin_outlets`
--

CREATE TABLE `admin_outlets` (
  `loc_store_id` int(11) NOT NULL,
  `store_id` int(11) NOT NULL,
  `brand_name` varchar(255) NOT NULL,
  `store_name` varchar(255) NOT NULL,
  `user_guid` varchar(255) NOT NULL,
  `location_name` varchar(50) NOT NULL,
  `postal_code` varchar(50) NOT NULL,
  `address` varchar(255) NOT NULL,
  `Barangay` varchar(255) NOT NULL,
  `municipality` varchar(255) NOT NULL,
  `municipality_name` varchar(255) NOT NULL,
  `province` varchar(255) NOT NULL,
  `province_name` varchar(255) NOT NULL,
  `tin_no` varchar(255) NOT NULL,
  `tel_no` varchar(255) NOT NULL,
  `active` tinyint(2) NOT NULL,
  `created_at` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `MIN` varchar(255) NOT NULL,
  `MSN` varchar(255) NOT NULL,
  `PTUN` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `loc_admin_category`
--

CREATE TABLE `loc_admin_category` (
  `category_id` int(11) NOT NULL,
  `category_name` varchar(50) NOT NULL,
  `brand_name` varchar(255) NOT NULL,
  `updated_at` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `origin` varchar(50) NOT NULL,
  `status` tinyint(2) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `loc_admin_products`
--

CREATE TABLE `loc_admin_products` (
  `product_id` int(11) NOT NULL,
  `product_sku` varchar(50) NOT NULL,
  `product_name` varchar(50) NOT NULL,
  `formula_id` varchar(255) NOT NULL,
  `product_barcode` varchar(255) NOT NULL,
  `product_category` varchar(255) NOT NULL,
  `product_price` int(255) NOT NULL,
  `product_desc` varchar(255) NOT NULL,
  `product_image` longtext NOT NULL,
  `product_status` varchar(2) NOT NULL,
  `origin` varchar(50) NOT NULL,
  `date_modified` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `guid` varchar(255) NOT NULL,
  `ip_address` varchar(20) NOT NULL,
  `store_id` int(11) NOT NULL,
  `crew_id` varchar(50) NOT NULL,
  `synced` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `loc_daily_transaction`
--

CREATE TABLE `loc_daily_transaction` (
  `transaction_id` int(11) NOT NULL,
  `transaction_number` varchar(255) NOT NULL,
  `amounttendered` decimal(11,2) NOT NULL,
  `discount` decimal(11,2) NOT NULL,
  `moneychange` decimal(11,2) NOT NULL,
  `amountdue` decimal(11,2) NOT NULL,
  `vatable` decimal(11,2) NOT NULL,
  `vat_exempt` decimal(11,2) NOT NULL,
  `zero_rated` decimal(11,2) NOT NULL,
  `vat` decimal(11,2) NOT NULL,
  `si_number` int(10) NOT NULL,
  `crew_id` varchar(20) NOT NULL,
  `guid` varchar(50) NOT NULL,
  `ip_address` varchar(50) NOT NULL,
  `active` varchar(2) NOT NULL,
  `store_id` varchar(11) NOT NULL,
  `date` date NOT NULL,
  `time` time NOT NULL,
  `transaction_type` varchar(50) NOT NULL,
  `shift` varchar(255) NOT NULL,
  `zreading` date NOT NULL,
  `synced` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `loc_daily_transaction_details`
--

CREATE TABLE `loc_daily_transaction_details` (
  `details_id` int(11) NOT NULL,
  `product_id` int(11) NOT NULL,
  `product_sku` varchar(255) NOT NULL,
  `product_name` varchar(255) NOT NULL,
  `quantity` int(20) NOT NULL,
  `price` decimal(11,2) NOT NULL,
  `total` decimal(11,2) NOT NULL,
  `crew_id` varchar(50) NOT NULL,
  `transaction_number` varchar(255) NOT NULL,
  `active` tinyint(2) NOT NULL,
  `created_at` date NOT NULL,
  `timenow` time NOT NULL,
  `guid` varchar(255) NOT NULL,
  `store_id` varchar(50) NOT NULL,
  `total_cost_of_goods` decimal(11,2) NOT NULL,
  `product_category` varchar(255) NOT NULL,
  `zreading` date NOT NULL,
  `synced` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `loc_deposit`
--

CREATE TABLE `loc_deposit` (
  `dep_id` int(11) NOT NULL,
  `name` varchar(255) NOT NULL,
  `crew_id` varchar(50) NOT NULL,
  `transaction_number` varchar(255) NOT NULL,
  `amount` decimal(11,2) NOT NULL,
  `bank` varchar(255) NOT NULL,
  `transaction_date` varchar(255) NOT NULL,
  `store_id` varchar(11) NOT NULL,
  `guid` varchar(255) NOT NULL,
  `date_created` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `synced` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `loc_expenses_hold`
--

CREATE TABLE `loc_expenses_hold` (
  `expense_id` int(11) NOT NULL,
  `expense_type` varchar(50) NOT NULL,
  `item_info` varchar(255) NOT NULL,
  `quantity` int(11) NOT NULL,
  `price` decimal(10,2) NOT NULL,
  `amount` decimal(10,2) NOT NULL,
  `attachment` text NOT NULL,
  `created_at` date NOT NULL,
  `time` time NOT NULL,
  `crew_id` int(11) NOT NULL,
  `guid` varchar(255) NOT NULL,
  `store_id` int(20) NOT NULL,
  `active` tinyint(2) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `loc_expense_details`
--

CREATE TABLE `loc_expense_details` (
  `expense_id` int(11) NOT NULL,
  `expense_number` varchar(255) NOT NULL,
  `expense_type` varchar(50) NOT NULL,
  `item_info` varchar(255) NOT NULL,
  `quantity` int(11) NOT NULL,
  `price` decimal(10,2) NOT NULL,
  `amount` decimal(10,2) NOT NULL,
  `attachment` text NOT NULL,
  `created_at` date NOT NULL,
  `time` time NOT NULL,
  `crew_id` varchar(50) NOT NULL,
  `guid` varchar(255) NOT NULL,
  `store_id` int(20) NOT NULL,
  `active` tinyint(2) NOT NULL,
  `zreading` date NOT NULL,
  `synced` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `loc_expense_list`
--

CREATE TABLE `loc_expense_list` (
  `expense_id` int(11) NOT NULL,
  `crew_id` varchar(50) NOT NULL,
  `expense_number` varchar(255) NOT NULL,
  `total_amount` decimal(11,2) NOT NULL,
  `paid_amount` decimal(11,2) NOT NULL,
  `unpaid_amount` decimal(11,2) NOT NULL,
  `store_id` varchar(255) NOT NULL,
  `guid` varchar(255) NOT NULL,
  `date` date NOT NULL,
  `time` time NOT NULL,
  `active` tinyint(2) NOT NULL,
  `zreading` date NOT NULL,
  `synced` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `loc_fm_stock`
--

CREATE TABLE `loc_fm_stock` (
  `fm_id` int(11) NOT NULL,
  `formula_id` varchar(255) NOT NULL,
  `stock_quantity` int(11) NOT NULL,
  `stock_total` int(11) NOT NULL,
  `crew_id` varchar(255) NOT NULL,
  `store_id` varchar(11) NOT NULL,
  `guid` varchar(255) NOT NULL,
  `date` date NOT NULL,
  `time` time NOT NULL,
  `status` tinyint(2) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `loc_hold_inventory`
--

CREATE TABLE `loc_hold_inventory` (
  `hold_id` int(255) NOT NULL,
  `sr_total` int(255) NOT NULL,
  `f_id` int(255) NOT NULL,
  `qty` int(255) NOT NULL,
  `id` int(255) NOT NULL,
  `nm` varchar(255) NOT NULL,
  `org_serve` int(255) NOT NULL,
  `name` varchar(255) NOT NULL,
  `cog` decimal(11,2) NOT NULL,
  `ocog` decimal(11,2) NOT NULL,
  `prd.addid` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `loc_inbox_messages`
--

CREATE TABLE `loc_inbox_messages` (
  `inbox_id` int(11) NOT NULL,
  `crew_id` int(11) NOT NULL,
  `message` varchar(255) NOT NULL,
  `type` varchar(20) NOT NULL,
  `date_time` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `origin` varchar(20) NOT NULL,
  `active` tinyint(2) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `loc_inv_temp_data`
--

CREATE TABLE `loc_inv_temp_data` (
  `inventory_id` int(11) NOT NULL,
  `store_id` varchar(11) NOT NULL,
  `formula_id` int(11) NOT NULL,
  `product_ingredients` varchar(255) NOT NULL,
  `sku` varchar(255) NOT NULL,
  `stock_quantity` int(11) NOT NULL,
  `stock_total` int(20) NOT NULL,
  `stock_status` int(11) NOT NULL,
  `critical_limit` int(11) NOT NULL,
  `guid` varchar(255) NOT NULL,
  `date_modified` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `date_created` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `loc_partners_transaction`
--

CREATE TABLE `loc_partners_transaction` (
  `id` int(11) NOT NULL,
  `arrid` int(11) NOT NULL,
  `bankname` varchar(255) NOT NULL,
  `date_modified` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `crew_id` varchar(55) NOT NULL,
  `store_id` varchar(55) NOT NULL,
  `guid` varchar(255) NOT NULL,
  `active` tinyint(2) NOT NULL,
  `synced` varchar(55) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `loc_partners_transaction`
--

INSERT INTO `loc_partners_transaction` (`id`, `arrid`, `bankname`, `date_modified`, `crew_id`, `store_id`, `guid`, `active`, `synced`) VALUES
(1, 4, 'BDO', '2020-03-21 05:49:58', 'FBW5-6967', '5', 'eb475414-7efd-1bb3-51b8-1b029a94cd1f', 1, 'Unsynced'),
(2, 3, 'BPI', '2020-03-16 10:49:38', 'FBW5-6967', '5', 'eb475414-7efd-1bb3-51b8-1b029a94cd1f', 1, 'Unsynced'),
(3, 5, 'METRO BANK', '2020-03-24 05:34:58', 'FBW5-6967', '5', 'eb475414-7efd-1bb3-51b8-1b029a94cd1f', 1, 'Unsynced'),
(4, 2, 'CHINA BANK', '2020-03-24 06:29:56', 'FBW5-6967', '5', 'eb475414-7efd-1bb3-51b8-1b029a94cd1f', 1, 'Unsynced'),
(5, 1, 'LAND BANK', '2020-03-24 06:48:58', 'FBW5-6967', '5', 'eb475414-7efd-1bb3-51b8-1b029a94cd1f', 1, 'Unsynced');

-- --------------------------------------------------------

--
-- Table structure for table `loc_pending_orders`
--

CREATE TABLE `loc_pending_orders` (
  `order_id` int(11) NOT NULL,
  `crew_id` varchar(50) NOT NULL,
  `customer_name` varchar(50) NOT NULL,
  `product_name` varchar(50) NOT NULL,
  `product_quantity` int(50) NOT NULL,
  `product_price` int(50) NOT NULL,
  `product_total` int(50) NOT NULL,
  `product_id` int(11) NOT NULL,
  `product_sku` varchar(255) NOT NULL,
  `product_category` varchar(255) NOT NULL,
  `product_addon_id` int(11) NOT NULL,
  `datetime` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `guid` varchar(50) NOT NULL,
  `active` int(11) NOT NULL,
  `increment` varchar(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

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
  `stock_quantity` int(11) NOT NULL,
  `stock_total` int(20) NOT NULL,
  `stock_status` int(11) NOT NULL,
  `critical_limit` int(11) NOT NULL,
  `guid` varchar(255) NOT NULL,
  `date_modified` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `crew_id` varchar(50) NOT NULL,
  `synced` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `loc_product_formula`
--

CREATE TABLE `loc_product_formula` (
  `formula_id` int(11) NOT NULL,
  `product_ingredients` varchar(255) NOT NULL,
  `primary_unit` varchar(50) NOT NULL,
  `primary_value` varchar(50) NOT NULL,
  `secondary_unit` varchar(50) NOT NULL,
  `secondary_value` varchar(50) NOT NULL,
  `serving_unit` varchar(50) NOT NULL,
  `serving_value` varchar(50) NOT NULL,
  `no_servings` varchar(250) NOT NULL,
  `status` tinyint(2) NOT NULL,
  `date_modified` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `unit_cost` decimal(11,2) NOT NULL,
  `store_id` varchar(50) NOT NULL,
  `guid` varchar(255) NOT NULL,
  `crew_id` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `loc_promo_coupon_list`
--

CREATE TABLE `loc_promo_coupon_list` (
  `coupon_id` int(11) NOT NULL,
  `coupon_code` varchar(10) NOT NULL,
  `coupon_name` varchar(255) NOT NULL,
  `coupon_desc` varchar(255) NOT NULL,
  `reference_value` varchar(255) NOT NULL,
  `type` varchar(50) NOT NULL,
  `coupon_type` varchar(255) NOT NULL,
  `discount_value` decimal(11,2) NOT NULL,
  `productbundle` varchar(255) NOT NULL,
  `product_id` varchar(255) NOT NULL,
  `min_spend` int(11) NOT NULL,
  `max_spend` int(11) NOT NULL,
  `origin` varchar(20) NOT NULL,
  `active` tinyint(2) NOT NULL,
  `guid` varchar(255) NOT NULL,
  `store_id` varchar(11) NOT NULL,
  `created_at` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `loc_refund_return_details`
--

CREATE TABLE `loc_refund_return_details` (
  `refret_id` int(11) NOT NULL,
  `transaction_number` varchar(255) NOT NULL,
  `crew_id` varchar(50) NOT NULL,
  `reason` text NOT NULL,
  `total` decimal(11,2) NOT NULL,
  `guid` varchar(255) NOT NULL,
  `ipaddress` varchar(255) NOT NULL,
  `store_id` int(11) NOT NULL,
  `datestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `zreading` date NOT NULL,
  `synced` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

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
  `Dev_PTU_Valid_Until` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `loc_system_logs`
--

CREATE TABLE `loc_system_logs` (
  `crew_id` varchar(50) NOT NULL,
  `log_type` varchar(255) NOT NULL,
  `log_description` text NOT NULL,
  `log_date_time` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `log_store` varchar(20) NOT NULL,
  `guid` varchar(255) NOT NULL,
  `ip_address` varchar(255) NOT NULL,
  `loc_systemlog_id` varchar(255) NOT NULL,
  `zreading` date NOT NULL,
  `synced` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `loc_transaction_mode_details`
--

CREATE TABLE `loc_transaction_mode_details` (
  `mode_id` int(11) NOT NULL,
  `transaction_type` varchar(255) NOT NULL,
  `transaction_number` varchar(255) NOT NULL,
  `fullname` varchar(255) NOT NULL,
  `reference` varchar(255) NOT NULL,
  `markup` varchar(255) NOT NULL,
  `date_time_created` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `status` tinyint(4) NOT NULL,
  `store_id` varchar(255) NOT NULL,
  `guid` varchar(255) NOT NULL,
  `synced` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `loc_updates`
--

CREATE TABLE `loc_updates` (
  `up_id` int(11) NOT NULL,
  `up_version` varchar(255) NOT NULL,
  `up_timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `loc_updates`
--

INSERT INTO `loc_updates` (`up_id`, `up_version`, `up_timestamp`) VALUES
(1, 'V1.0.202003.23', '2020-03-23 02:13:24');

-- --------------------------------------------------------

--
-- Table structure for table `loc_users`
--

CREATE TABLE `loc_users` (
  `user_id` int(11) NOT NULL,
  `user_level` varchar(100) NOT NULL,
  `full_name` varchar(255) NOT NULL,
  `username` varchar(255) NOT NULL,
  `password` varchar(255) NOT NULL,
  `contact_number` varchar(20) NOT NULL,
  `email` varchar(255) NOT NULL,
  `position` varchar(100) NOT NULL,
  `gender` varchar(20) NOT NULL,
  `created_at` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `updated_at` timestamp NOT NULL DEFAULT current_timestamp(),
  `active` varchar(2) NOT NULL,
  `guid` varchar(50) NOT NULL,
  `store_id` varchar(11) NOT NULL,
  `uniq_id` varchar(50) NOT NULL,
  `synced` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `tbcoupon`
--

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
(1, 'Senior/PWD Discount 20%', 'Senior/PWD Discount 20% - Standard National Discount', '20', '', 'Percentage', '', '', '', '', '01/01/2020', '31/12/9998'),
(2, '100 OFF GC - Chinese New Year', '100 OFF Gift Certificate for the celebration of Chinese New Year', '100', '', 'Fix-1', '', '', '', '', '25/01/2020', '31/01/2020'),
(3, '50 OFF on your next waffle', '50 OFF on your next waffle if you buy with minimum amount of 500', '50', '500', 'Fix-2', '', '', '', '', '01/01/2020', '31/01/2020'),
(4, 'Free Choco Waffle', 'Free Choco Waffle if you buy Peanut Butter waffle', '', '', 'Bundle-1(Fix)', '2', '1', '1', '1', '01/01/2020', '31/12/2020'),
(5, '10 OFF ON DRINKS', 'P10 OFF on Drinks if you buy 2 choco waffles\r\n', '10', '', 'Bundle-2(Fix)', '1', '2', '5,6,7,8', '1', '01/01/2020', '31/12/2020'),
(6, '10% OFF ON YOUR 3RD WAFFLE', '10% OFF for the 3rd Waffle if you buy 2 Perfect Combination Waffle', '10', '', 'Bundle-3(%)', '4', '2', '1,2,3,4', '1', '01/01/2020', '31/12/2020'),
(7, '123', '123', '', '', 'Bundle-1(Fix)', '7,8', '10', '7,8', '100', '1/5/2020', '1/5/2020');

-- --------------------------------------------------------

--
-- Table structure for table `testcoupon`
--

CREATE TABLE `testcoupon` (
  `coupon_id` int(11) NOT NULL,
  `coupon_type` varchar(255) NOT NULL,
  `coupon_desc` text NOT NULL,
  `coupon_reference_val` int(11) NOT NULL,
  `coupon_disc_val` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `triggers_loc_admin_products`
--

CREATE TABLE `triggers_loc_admin_products` (
  `product_id` int(11) NOT NULL,
  `product_sku` varchar(50) NOT NULL,
  `product_name` varchar(50) NOT NULL,
  `formula_id` varchar(255) NOT NULL,
  `product_barcode` varchar(255) NOT NULL,
  `product_category` varchar(255) NOT NULL,
  `product_price` int(255) NOT NULL,
  `product_desc` varchar(255) NOT NULL,
  `product_image` longtext NOT NULL,
  `product_status` varchar(2) NOT NULL,
  `origin` varchar(50) NOT NULL,
  `date_modified` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `guid` varchar(255) NOT NULL,
  `ip_address` varchar(20) NOT NULL,
  `store_id` int(11) NOT NULL,
  `crew_id` varchar(50) NOT NULL,
  `synced` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Triggers `triggers_loc_admin_products`
--
DELIMITER $$
CREATE TRIGGER `Copy_To_Loc_admin_products` AFTER INSERT ON `triggers_loc_admin_products` FOR EACH ROW INSERT INTO loc_admin_products(`product_sku`, `product_name`, `formula_id`, `product_barcode`, `product_category`, `product_price`, `product_desc`, `product_image`, `product_status`, `origin`, `date_modified`, `guid`, `ip_address`, `store_id`, `crew_id`, `synced`)
SELECT `product_sku`, `product_name`, `formula_id`, `product_barcode`, `product_category`, `product_price`, `product_desc`, `product_image`, `product_status`, `origin`, `date_modified`, `guid`, `ip_address`, `store_id`, `crew_id`, `synced`
  FROM triggers_loc_admin_products
 WHERE NOT EXISTS(SELECT `product_sku`, `product_name`, `formula_id`, `product_barcode`, `product_category`, `product_price`, `product_desc`, `product_image`, `product_status`, `origin`, `date_modified`, `guid`, `ip_address`, `store_id`, `crew_id`, `synced`
                    FROM loc_admin_products
                   WHERE loc_admin_products.product_sku = triggers_loc_admin_products.product_sku )
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Table structure for table `triggers_loc_users`
--

CREATE TABLE `triggers_loc_users` (
  `user_id` int(11) NOT NULL,
  `user_level` varchar(100) NOT NULL,
  `full_name` varchar(255) NOT NULL,
  `username` varchar(255) NOT NULL,
  `password` varchar(255) NOT NULL,
  `contact_number` varchar(20) NOT NULL,
  `email` varchar(255) NOT NULL,
  `position` varchar(100) NOT NULL,
  `gender` varchar(20) NOT NULL,
  `created_at` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `updated_at` timestamp NOT NULL DEFAULT current_timestamp(),
  `active` varchar(2) NOT NULL,
  `guid` varchar(50) NOT NULL,
  `store_id` varchar(11) NOT NULL,
  `uniq_id` varchar(50) NOT NULL,
  `synced` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `triggers_loc_users`
--

INSERT INTO `triggers_loc_users` (`user_id`, `user_level`, `full_name`, `username`, `password`, `contact_number`, `email`, `position`, `gender`, `created_at`, `updated_at`, `active`, `guid`, `store_id`, `uniq_id`, `synced`) VALUES
(1, 'Head Crew', 'Mark Daniel Reyes', 'dd', '296506902c693b458707ad6f7e24a544', '09541225478', 'dd@gmail.com', 'Head Crew', 'Male', '2020-03-16 09:47:43', '2020-03-16 09:47:43', '1', 'eb475414-7efd-1bb3-51b8-1b029a94cd1f', '5', 'FBW5-6967', 'Unsynced');

--
-- Triggers `triggers_loc_users`
--
DELIMITER $$
CREATE TRIGGER `Copy_To_Loc_Users` AFTER INSERT ON `triggers_loc_users` FOR EACH ROW INSERT INTO loc_users(`user_level`, `full_name`, `username`, `password`, `contact_number`, `email`, `position`, `gender`, `created_at`, `updated_at`, `active`, `guid`, `store_id`, `uniq_id`)
SELECT `user_level`, `full_name`, `username`, `password`, `contact_number`, `email`, `position`, `gender`, `created_at`, `updated_at`, `active`, `guid`, `store_id`, `uniq_id`
  FROM Triggers_loc_users
 WHERE NOT EXISTS(SELECT `user_level`, `full_name`, `username`, `password`, `contact_number`, `email`, `position`, `gender`, `created_at`, `updated_at`, `active`, `guid`, `store_id`, `uniq_id`
                    FROM loc_users
                   WHERE loc_users.uniq_id = Triggers_loc_users.uniq_id )
$$
DELIMITER ;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `admin_coupon`
--
ALTER TABLE `admin_coupon`
  ADD PRIMARY KEY (`coupon_id`);

--
-- Indexes for table `admin_masterlist`
--
ALTER TABLE `admin_masterlist`
  ADD PRIMARY KEY (`masterlist_id`);

--
-- Indexes for table `admin_outlets`
--
ALTER TABLE `admin_outlets`
  ADD PRIMARY KEY (`loc_store_id`);

--
-- Indexes for table `loc_admin_category`
--
ALTER TABLE `loc_admin_category`
  ADD PRIMARY KEY (`category_id`);

--
-- Indexes for table `loc_admin_products`
--
ALTER TABLE `loc_admin_products`
  ADD PRIMARY KEY (`product_id`);

--
-- Indexes for table `loc_daily_transaction`
--
ALTER TABLE `loc_daily_transaction`
  ADD PRIMARY KEY (`transaction_id`);

--
-- Indexes for table `loc_daily_transaction_details`
--
ALTER TABLE `loc_daily_transaction_details`
  ADD PRIMARY KEY (`details_id`);

--
-- Indexes for table `loc_deposit`
--
ALTER TABLE `loc_deposit`
  ADD PRIMARY KEY (`dep_id`);

--
-- Indexes for table `loc_expenses_hold`
--
ALTER TABLE `loc_expenses_hold`
  ADD PRIMARY KEY (`expense_id`);

--
-- Indexes for table `loc_expense_details`
--
ALTER TABLE `loc_expense_details`
  ADD PRIMARY KEY (`expense_id`);

--
-- Indexes for table `loc_expense_list`
--
ALTER TABLE `loc_expense_list`
  ADD PRIMARY KEY (`expense_id`);

--
-- Indexes for table `loc_fm_stock`
--
ALTER TABLE `loc_fm_stock`
  ADD PRIMARY KEY (`fm_id`);

--
-- Indexes for table `loc_hold_inventory`
--
ALTER TABLE `loc_hold_inventory`
  ADD PRIMARY KEY (`hold_id`);

--
-- Indexes for table `loc_inbox_messages`
--
ALTER TABLE `loc_inbox_messages`
  ADD PRIMARY KEY (`inbox_id`);

--
-- Indexes for table `loc_inv_temp_data`
--
ALTER TABLE `loc_inv_temp_data`
  ADD PRIMARY KEY (`inventory_id`);

--
-- Indexes for table `loc_partners_transaction`
--
ALTER TABLE `loc_partners_transaction`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `loc_pending_orders`
--
ALTER TABLE `loc_pending_orders`
  ADD PRIMARY KEY (`order_id`);

--
-- Indexes for table `loc_pos_inventory`
--
ALTER TABLE `loc_pos_inventory`
  ADD PRIMARY KEY (`inventory_id`);

--
-- Indexes for table `loc_product_formula`
--
ALTER TABLE `loc_product_formula`
  ADD PRIMARY KEY (`formula_id`);

--
-- Indexes for table `loc_promo_coupon_list`
--
ALTER TABLE `loc_promo_coupon_list`
  ADD PRIMARY KEY (`coupon_id`);

--
-- Indexes for table `loc_refund_return_details`
--
ALTER TABLE `loc_refund_return_details`
  ADD PRIMARY KEY (`refret_id`);

--
-- Indexes for table `loc_settings`
--
ALTER TABLE `loc_settings`
  ADD PRIMARY KEY (`settings_id`);

--
-- Indexes for table `loc_transaction_mode_details`
--
ALTER TABLE `loc_transaction_mode_details`
  ADD PRIMARY KEY (`mode_id`);

--
-- Indexes for table `loc_updates`
--
ALTER TABLE `loc_updates`
  ADD PRIMARY KEY (`up_id`);

--
-- Indexes for table `loc_users`
--
ALTER TABLE `loc_users`
  ADD PRIMARY KEY (`user_id`);

--
-- Indexes for table `tbcoupon`
--
ALTER TABLE `tbcoupon`
  ADD PRIMARY KEY (`ID`);

--
-- Indexes for table `testcoupon`
--
ALTER TABLE `testcoupon`
  ADD PRIMARY KEY (`coupon_id`);

--
-- Indexes for table `triggers_loc_admin_products`
--
ALTER TABLE `triggers_loc_admin_products`
  ADD PRIMARY KEY (`product_id`);

--
-- Indexes for table `triggers_loc_users`
--
ALTER TABLE `triggers_loc_users`
  ADD PRIMARY KEY (`user_id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `admin_coupon`
--
ALTER TABLE `admin_coupon`
  MODIFY `coupon_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `admin_masterlist`
--
ALTER TABLE `admin_masterlist`
  MODIFY `masterlist_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `admin_outlets`
--
ALTER TABLE `admin_outlets`
  MODIFY `loc_store_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `loc_admin_category`
--
ALTER TABLE `loc_admin_category`
  MODIFY `category_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `loc_admin_products`
--
ALTER TABLE `loc_admin_products`
  MODIFY `product_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `loc_daily_transaction`
--
ALTER TABLE `loc_daily_transaction`
  MODIFY `transaction_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `loc_daily_transaction_details`
--
ALTER TABLE `loc_daily_transaction_details`
  MODIFY `details_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `loc_deposit`
--
ALTER TABLE `loc_deposit`
  MODIFY `dep_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `loc_expenses_hold`
--
ALTER TABLE `loc_expenses_hold`
  MODIFY `expense_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `loc_expense_details`
--
ALTER TABLE `loc_expense_details`
  MODIFY `expense_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `loc_expense_list`
--
ALTER TABLE `loc_expense_list`
  MODIFY `expense_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `loc_fm_stock`
--
ALTER TABLE `loc_fm_stock`
  MODIFY `fm_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `loc_hold_inventory`
--
ALTER TABLE `loc_hold_inventory`
  MODIFY `hold_id` int(255) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `loc_inbox_messages`
--
ALTER TABLE `loc_inbox_messages`
  MODIFY `inbox_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `loc_inv_temp_data`
--
ALTER TABLE `loc_inv_temp_data`
  MODIFY `inventory_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `loc_partners_transaction`
--
ALTER TABLE `loc_partners_transaction`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT for table `loc_pending_orders`
--
ALTER TABLE `loc_pending_orders`
  MODIFY `order_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `loc_pos_inventory`
--
ALTER TABLE `loc_pos_inventory`
  MODIFY `inventory_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `loc_product_formula`
--
ALTER TABLE `loc_product_formula`
  MODIFY `formula_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `loc_promo_coupon_list`
--
ALTER TABLE `loc_promo_coupon_list`
  MODIFY `coupon_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `loc_refund_return_details`
--
ALTER TABLE `loc_refund_return_details`
  MODIFY `refret_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `loc_settings`
--
ALTER TABLE `loc_settings`
  MODIFY `settings_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `loc_transaction_mode_details`
--
ALTER TABLE `loc_transaction_mode_details`
  MODIFY `mode_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `loc_updates`
--
ALTER TABLE `loc_updates`
  MODIFY `up_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `loc_users`
--
ALTER TABLE `loc_users`
  MODIFY `user_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `tbcoupon`
--
ALTER TABLE `tbcoupon`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT for table `testcoupon`
--
ALTER TABLE `testcoupon`
  MODIFY `coupon_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `triggers_loc_admin_products`
--
ALTER TABLE `triggers_loc_admin_products`
  MODIFY `product_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `triggers_loc_users`
--
ALTER TABLE `triggers_loc_users`
  MODIFY `user_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
