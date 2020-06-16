-- MariaDB dump 10.17  Distrib 10.4.11-MariaDB, for Win64 (AMD64)
--
-- Host: localhost    Database: pos
-- ------------------------------------------------------
-- Server version	10.4.11-MariaDB

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Current Database: `pos`
--

CREATE DATABASE /*!32312 IF NOT EXISTS*/ `pos` /*!40100 DEFAULT CHARACTER SET utf8mb4 */;

USE `pos`;

--
-- Table structure for table `admin_coupon`
--

DROP TABLE IF EXISTS `admin_coupon`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `admin_coupon` (
  `coupon_id` int(11) NOT NULL AUTO_INCREMENT,
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
  `active` tinyint(2) NOT NULL,
  PRIMARY KEY (`coupon_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `admin_coupon`
--

LOCK TABLES `admin_coupon` WRITE;
/*!40000 ALTER TABLE `admin_coupon` DISABLE KEYS */;
/*!40000 ALTER TABLE `admin_coupon` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `admin_masterlist`
--

DROP TABLE IF EXISTS `admin_masterlist`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `admin_masterlist` (
  `masterlist_id` int(11) NOT NULL AUTO_INCREMENT,
  `masterlist_username` varchar(255) NOT NULL,
  `masterlist_password` varchar(255) NOT NULL,
  `client_ipadd` varchar(50) NOT NULL,
  `client_guid` varchar(255) NOT NULL,
  `client_product_key` varchar(255) NOT NULL,
  `user_id` varchar(11) NOT NULL,
  `active` tinyint(2) NOT NULL,
  `created_at` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `client_store_id` int(11) NOT NULL,
  PRIMARY KEY (`masterlist_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `admin_masterlist`
--

--
-- Table structure for table `admin_outlets`
--

DROP TABLE IF EXISTS `admin_outlets`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `admin_outlets` (
  `loc_store_id` int(11) NOT NULL AUTO_INCREMENT,
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
  `PTUN` varchar(255) NOT NULL,
  PRIMARY KEY (`loc_store_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `admin_outlets`
--


--
-- Table structure for table `loc_admin_category`
--

DROP TABLE IF EXISTS `loc_admin_category`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `loc_admin_category` (
  `category_id` int(11) NOT NULL AUTO_INCREMENT,
  `category_name` varchar(50) NOT NULL,
  `brand_name` varchar(255) NOT NULL,
  `updated_at` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `origin` varchar(50) NOT NULL,
  `status` tinyint(2) NOT NULL,
  PRIMARY KEY (`category_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `loc_admin_category`
--

--
-- Table structure for table `loc_admin_products`
--

DROP TABLE IF EXISTS `loc_admin_products`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `loc_admin_products` (
  `product_id` int(11) NOT NULL AUTO_INCREMENT,
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
  `store_id` int(11) NOT NULL,
  `crew_id` varchar(50) NOT NULL,
  `synced` varchar(50) NOT NULL,
  `server_product_id` int(11) NOT NULL,
  PRIMARY KEY (`product_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `loc_admin_products`
--

--
-- Table structure for table `loc_coupon_data`
--

DROP TABLE IF EXISTS `loc_coupon_data`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `loc_coupon_data` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `transaction_number` text NOT NULL,
  `coupon_name` text NOT NULL,
  `coupon_desc` text NOT NULL,
  `coupon_type` text NOT NULL,
  `coupon_line` text NOT NULL,
  `coupon_total` text NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `loc_coupon_data`
--
--
-- Table structure for table `loc_daily_transaction`
--

DROP TABLE IF EXISTS `loc_daily_transaction`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `loc_daily_transaction` (
  `transaction_id` int(11) NOT NULL AUTO_INCREMENT,
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
  `active` varchar(2) NOT NULL,
  `store_id` varchar(11) NOT NULL,
  `date` date NOT NULL,
  `time` time NOT NULL,
  `transaction_type` varchar(50) NOT NULL,
  `shift` varchar(255) NOT NULL,
  `zreading` date NOT NULL,
  `synced` varchar(255) NOT NULL,
  `discount_type` varchar(50) NOT NULL,
  PRIMARY KEY (`transaction_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `loc_daily_transaction`
--

--
-- Table structure for table `loc_daily_transaction_details`
--

DROP TABLE IF EXISTS `loc_daily_transaction_details`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `loc_daily_transaction_details` (
  `details_id` int(11) NOT NULL AUTO_INCREMENT,
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
  `synced` varchar(255) NOT NULL,
  PRIMARY KEY (`details_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `loc_daily_transaction_details`
--

--
-- Table structure for table `loc_deposit`
--

DROP TABLE IF EXISTS `loc_deposit`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `loc_deposit` (
  `dep_id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(255) NOT NULL,
  `crew_id` varchar(50) NOT NULL,
  `transaction_number` varchar(255) NOT NULL,
  `amount` decimal(11,2) NOT NULL,
  `bank` varchar(255) NOT NULL,
  `transaction_date` varchar(255) NOT NULL,
  `store_id` varchar(11) NOT NULL,
  `guid` varchar(255) NOT NULL,
  `date_created` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `synced` varchar(50) NOT NULL,
  PRIMARY KEY (`dep_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `loc_deposit`
--

LOCK TABLES `loc_deposit` WRITE;
/*!40000 ALTER TABLE `loc_deposit` DISABLE KEYS */;
/*!40000 ALTER TABLE `loc_deposit` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `loc_expense_details`
--

DROP TABLE IF EXISTS `loc_expense_details`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `loc_expense_details` (
  `expense_id` int(11) NOT NULL AUTO_INCREMENT,
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
  `synced` varchar(255) NOT NULL,
  PRIMARY KEY (`expense_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `loc_expense_details`
--

LOCK TABLES `loc_expense_details` WRITE;
/*!40000 ALTER TABLE `loc_expense_details` DISABLE KEYS */;
/*!40000 ALTER TABLE `loc_expense_details` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `loc_expense_list`
--

DROP TABLE IF EXISTS `loc_expense_list`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `loc_expense_list` (
  `expense_id` int(11) NOT NULL AUTO_INCREMENT,
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
  `synced` varchar(255) NOT NULL,
  PRIMARY KEY (`expense_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `loc_expense_list`
--

LOCK TABLES `loc_expense_list` WRITE;
/*!40000 ALTER TABLE `loc_expense_list` DISABLE KEYS */;
/*!40000 ALTER TABLE `loc_expense_list` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `loc_expenses_hold`
--

DROP TABLE IF EXISTS `loc_expenses_hold`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `loc_expenses_hold` (
  `expense_id` int(11) NOT NULL AUTO_INCREMENT,
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
  `active` tinyint(2) NOT NULL,
  PRIMARY KEY (`expense_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `loc_expenses_hold`
--

LOCK TABLES `loc_expenses_hold` WRITE;
/*!40000 ALTER TABLE `loc_expenses_hold` DISABLE KEYS */;
/*!40000 ALTER TABLE `loc_expenses_hold` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `loc_fm_stock`
--

DROP TABLE IF EXISTS `loc_fm_stock`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `loc_fm_stock` (
  `fm_id` int(11) NOT NULL AUTO_INCREMENT,
  `formula_id` varchar(255) NOT NULL,
  `stock_quantity` int(11) NOT NULL,
  `stock_total` int(11) NOT NULL,
  `crew_id` varchar(255) NOT NULL,
  `store_id` varchar(11) NOT NULL,
  `guid` varchar(255) NOT NULL,
  `date` date NOT NULL,
  `time` time NOT NULL,
  `status` tinyint(2) NOT NULL,
  PRIMARY KEY (`fm_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `loc_fm_stock`
--

--
-- Table structure for table `loc_hold_inventory`
--

DROP TABLE IF EXISTS `loc_hold_inventory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `loc_hold_inventory` (
  `hold_id` int(255) NOT NULL AUTO_INCREMENT,
  `sr_total` int(255) NOT NULL,
  `f_id` int(255) NOT NULL,
  `qty` int(255) NOT NULL,
  `id` int(255) NOT NULL,
  `nm` varchar(255) NOT NULL,
  `org_serve` int(255) NOT NULL,
  `name` varchar(255) NOT NULL,
  `cog` decimal(11,2) NOT NULL,
  `ocog` decimal(11,2) NOT NULL,
  `prd.addid` int(11) NOT NULL,
  PRIMARY KEY (`hold_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `loc_hold_inventory`
--

LOCK TABLES `loc_hold_inventory` WRITE;
/*!40000 ALTER TABLE `loc_hold_inventory` DISABLE KEYS */;
/*!40000 ALTER TABLE `loc_hold_inventory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `loc_inbox_messages`
--

DROP TABLE IF EXISTS `loc_inbox_messages`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `loc_inbox_messages` (
  `inbox_id` int(11) NOT NULL AUTO_INCREMENT,
  `crew_id` int(11) NOT NULL,
  `message` varchar(255) NOT NULL,
  `type` varchar(20) NOT NULL,
  `date_time` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `origin` varchar(20) NOT NULL,
  `active` tinyint(2) NOT NULL,
  PRIMARY KEY (`inbox_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `loc_inbox_messages`
--

LOCK TABLES `loc_inbox_messages` WRITE;
/*!40000 ALTER TABLE `loc_inbox_messages` DISABLE KEYS */;
/*!40000 ALTER TABLE `loc_inbox_messages` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `loc_inv_temp_data`
--

DROP TABLE IF EXISTS `loc_inv_temp_data`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `loc_inv_temp_data` (
  `inventory_id` int(11) NOT NULL AUTO_INCREMENT,
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
  `date_created` varchar(255) NOT NULL,
  PRIMARY KEY (`inventory_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `loc_inv_temp_data`
--

--
-- Table structure for table `loc_partners_transaction`
--

DROP TABLE IF EXISTS `loc_partners_transaction`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `loc_partners_transaction` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `arrid` int(11) NOT NULL,
  `bankname` varchar(255) NOT NULL,
  `date_modified` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `crew_id` varchar(55) NOT NULL,
  `store_id` varchar(55) NOT NULL,
  `guid` varchar(255) NOT NULL,
  `active` tinyint(2) NOT NULL,
  `synced` varchar(55) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `loc_partners_transaction`
--

LOCK TABLES `loc_partners_transaction` WRITE;
/*!40000 ALTER TABLE `loc_partners_transaction` DISABLE KEYS */;
INSERT INTO `loc_partners_transaction` VALUES (1,4,'BDO','2020-03-21 05:49:58','FBW5-6967','5','eb475414-7efd-1bb3-51b8-1b029a94cd1f',1,'Unsynced'),(2,3,'BPI','2020-03-16 10:49:38','FBW5-6967','5','eb475414-7efd-1bb3-51b8-1b029a94cd1f',1,'Unsynced'),(3,1,'METRO BANK','2020-06-11 03:27:34','FBW5-6967','5','eb475414-7efd-1bb3-51b8-1b029a94cd1f',1,'Unsynced'),(4,2,'CHINA BANK','2020-03-24 06:29:56','FBW5-6967','5','eb475414-7efd-1bb3-51b8-1b029a94cd1f',1,'Unsynced'),(5,5,'LAND BANK','2020-06-11 03:27:34','FBW5-6967','5','eb475414-7efd-1bb3-51b8-1b029a94cd1f',1,'Unsynced');
/*!40000 ALTER TABLE `loc_partners_transaction` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `loc_pending_orders`
--

DROP TABLE IF EXISTS `loc_pending_orders`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `loc_pending_orders` (
  `order_id` int(11) NOT NULL AUTO_INCREMENT,
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
  `increment` varchar(11) NOT NULL,
  PRIMARY KEY (`order_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `loc_pending_orders`
--

LOCK TABLES `loc_pending_orders` WRITE;
/*!40000 ALTER TABLE `loc_pending_orders` DISABLE KEYS */;
/*!40000 ALTER TABLE `loc_pending_orders` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `loc_pos_inventory`
--

DROP TABLE IF EXISTS `loc_pos_inventory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `loc_pos_inventory` (
  `inventory_id` int(11) NOT NULL AUTO_INCREMENT,
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
  `synced` varchar(255) NOT NULL,
  `server_date_modified` text NOT NULL,
  `server_inventory_id` int(11) NOT NULL,
  PRIMARY KEY (`inventory_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `loc_pos_inventory`
--

--
-- Table structure for table `loc_product_formula`
--

DROP TABLE IF EXISTS `loc_product_formula`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `loc_product_formula` (
  `formula_id` int(11) NOT NULL AUTO_INCREMENT,
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
  `crew_id` varchar(50) NOT NULL,
  `origin` varchar(255) NOT NULL,
  `server_formula_id` int(11) NOT NULL,
  `server_date_modified` varchar(50) NOT NULL,
  PRIMARY KEY (`formula_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `loc_product_formula`
--

--
-- Table structure for table `loc_promo_coupon_list`
--

DROP TABLE IF EXISTS `loc_promo_coupon_list`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `loc_promo_coupon_list` (
  `coupon_id` int(11) NOT NULL AUTO_INCREMENT,
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
  `created_at` date NOT NULL,
  PRIMARY KEY (`coupon_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `loc_promo_coupon_list`
--

LOCK TABLES `loc_promo_coupon_list` WRITE;
/*!40000 ALTER TABLE `loc_promo_coupon_list` DISABLE KEYS */;
/*!40000 ALTER TABLE `loc_promo_coupon_list` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `loc_refund_return_details`
--

DROP TABLE IF EXISTS `loc_refund_return_details`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `loc_refund_return_details` (
  `refret_id` int(11) NOT NULL AUTO_INCREMENT,
  `transaction_number` varchar(255) NOT NULL,
  `crew_id` varchar(50) NOT NULL,
  `reason` text NOT NULL,
  `total` decimal(11,2) NOT NULL,
  `guid` varchar(255) NOT NULL,
  `ipaddress` varchar(255) NOT NULL,
  `store_id` int(11) NOT NULL,
  `datestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `zreading` date NOT NULL,
  `synced` varchar(255) NOT NULL,
  PRIMARY KEY (`refret_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `loc_refund_return_details`
--

LOCK TABLES `loc_refund_return_details` WRITE;
/*!40000 ALTER TABLE `loc_refund_return_details` DISABLE KEYS */;
/*!40000 ALTER TABLE `loc_refund_return_details` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `loc_settings`
--

DROP TABLE IF EXISTS `loc_settings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `loc_settings` (
  `settings_id` int(11) NOT NULL AUTO_INCREMENT,
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
  PRIMARY KEY (`settings_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `loc_settings`
--

--
-- Table structure for table `loc_system_logs`
--

DROP TABLE IF EXISTS `loc_system_logs`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `loc_system_logs` (
  `crew_id` varchar(50) NOT NULL,
  `log_type` varchar(255) NOT NULL,
  `log_description` text NOT NULL,
  `log_date_time` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `log_store` varchar(20) NOT NULL,
  `guid` varchar(255) NOT NULL,
  `loc_systemlog_id` varchar(255) NOT NULL,
  `zreading` varchar(255) NOT NULL,
  `synced` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `loc_system_logs`
--

--
-- Table structure for table `loc_transaction_mode_details`
--

DROP TABLE IF EXISTS `loc_transaction_mode_details`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `loc_transaction_mode_details` (
  `mode_id` int(11) NOT NULL AUTO_INCREMENT,
  `transaction_type` varchar(255) NOT NULL,
  `transaction_number` varchar(255) NOT NULL,
  `fullname` varchar(255) NOT NULL,
  `reference` varchar(255) NOT NULL,
  `markup` varchar(255) NOT NULL,
  `date_time_created` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `status` tinyint(4) NOT NULL,
  `store_id` varchar(255) NOT NULL,
  `guid` varchar(255) NOT NULL,
  `synced` varchar(50) NOT NULL,
  PRIMARY KEY (`mode_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `loc_transaction_mode_details`
--

LOCK TABLES `loc_transaction_mode_details` WRITE;
/*!40000 ALTER TABLE `loc_transaction_mode_details` DISABLE KEYS */;
/*!40000 ALTER TABLE `loc_transaction_mode_details` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `loc_updates`
--

DROP TABLE IF EXISTS `loc_updates`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `loc_updates` (
  `up_id` int(11) NOT NULL AUTO_INCREMENT,
  `up_version` varchar(255) NOT NULL,
  `up_timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`up_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `loc_updates`
--

LOCK TABLES `loc_updates` WRITE;
/*!40000 ALTER TABLE `loc_updates` DISABLE KEYS */;
/*!40000 ALTER TABLE `loc_updates` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `loc_users`
--

DROP TABLE IF EXISTS `loc_users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `loc_users` (
  `user_id` int(11) NOT NULL AUTO_INCREMENT,
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
  `synced` varchar(255) NOT NULL,
  PRIMARY KEY (`user_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `loc_users`
--

--
-- Table structure for table `tbcoupon`
--

DROP TABLE IF EXISTS `tbcoupon`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `tbcoupon` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
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
  `Expirydate` text NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tbcoupon`
--

LOCK TABLES `tbcoupon` WRITE;
/*!40000 ALTER TABLE `tbcoupon` DISABLE KEYS */;
INSERT INTO `tbcoupon` VALUES (1,'Senior/PWD Discount 20%','Senior/PWD Discount 20% - Standard National Discount','20','','Percentage','','','','','01/01/2020','31/12/9998'),(2,'100 OFF GC - Chinese New Year','100 OFF Gift Certificate for the celebration of Chinese New Year','100','','Fix-1','','','','','25/01/2020','31/01/2020'),(3,'50 OFF on your next waffle','50 OFF on your next waffle if you buy with minimum amount of 500','50','500','Fix-2','','','','','01/01/2020','31/01/2020'),(4,'Free Choco Waffle','Free Choco Waffle if you buy Peanut Butter waffle','','','Bundle-1(Fix)','5','1','3','1','01/01/2020','31/12/2020'),(5,'10 OFF ON DRINKS','P10 OFF on Drinks if you buy 2 choco waffles\r\n','10','','Bundle-2(Fix)','3','2','37,36','1','01/01/2020','31/12/2020'),(6,'10% OFF ON YOUR 3RD WAFFLE','10% OFF for the 3rd Waffle if you buy 2 Perfect Combination Waffle','10','','Bundle-3(%)','2,13,14,15,16,17,18,19,20,21,22,23,24,25,26','2','1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35','1','01/01/2020','31/12/2020'),(8,'Free Custard Waffle','Free Custart Waffle if you buy 2 chocolate waffle','N/A','N/A','Bundle-1(Fix)','3','2','9','1','1/5/2020','1/5/2020'),(9,'20 OFF ON DRINKS','20 OFF on Drinks if you buy 2 cheddar cheese waffle','20','N/A','Bundle-2(Fix)','4','2','37,36','1','1/5/2020','1/5/2020');
/*!40000 ALTER TABLE `tbcoupon` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `testcoupon`
--

DROP TABLE IF EXISTS `testcoupon`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `testcoupon` (
  `coupon_id` int(11) NOT NULL AUTO_INCREMENT,
  `coupon_type` varchar(255) NOT NULL,
  `coupon_desc` text NOT NULL,
  `coupon_reference_val` int(11) NOT NULL,
  `coupon_disc_val` int(11) NOT NULL,
  PRIMARY KEY (`coupon_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `testcoupon`
--

LOCK TABLES `testcoupon` WRITE;
/*!40000 ALTER TABLE `testcoupon` DISABLE KEYS */;
/*!40000 ALTER TABLE `testcoupon` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `triggers_loc_admin_products`
--

DROP TABLE IF EXISTS `triggers_loc_admin_products`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `triggers_loc_admin_products` (
  `product_id` int(11) NOT NULL AUTO_INCREMENT,
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
  `synced` varchar(50) NOT NULL,
  PRIMARY KEY (`product_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `triggers_loc_admin_products`
--

LOCK TABLES `triggers_loc_admin_products` WRITE;
/*!40000 ALTER TABLE `triggers_loc_admin_products` DISABLE KEYS */;
/*!40000 ALTER TABLE `triggers_loc_admin_products` ENABLE KEYS */;
UNLOCK TABLES;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_AUTO_VALUE_ON_ZERO' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `Copy_To_Loc_admin_products` AFTER INSERT ON `triggers_loc_admin_products` FOR EACH ROW INSERT INTO loc_admin_products(`product_sku`, `product_name`, `formula_id`, `product_barcode`, `product_category`, `product_price`, `product_desc`, `product_image`, `product_status`, `origin`, `date_modified`, `guid`, `ip_address`, `store_id`, `crew_id`, `synced`)
SELECT `product_sku`, `product_name`, `formula_id`, `product_barcode`, `product_category`, `product_price`, `product_desc`, `product_image`, `product_status`, `origin`, `date_modified`, `guid`, `ip_address`, `store_id`, `crew_id`, `synced`
  FROM triggers_loc_admin_products
 WHERE NOT EXISTS(SELECT `product_sku`, `product_name`, `formula_id`, `product_barcode`, `product_category`, `product_price`, `product_desc`, `product_image`, `product_status`, `origin`, `date_modified`, `guid`, `ip_address`, `store_id`, `crew_id`, `synced`
                    FROM loc_admin_products
                   WHERE loc_admin_products.product_sku = triggers_loc_admin_products.product_sku ) */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `triggers_loc_users`
--

DROP TABLE IF EXISTS `triggers_loc_users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `triggers_loc_users` (
  `user_id` int(11) NOT NULL AUTO_INCREMENT,
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
  `synced` varchar(255) NOT NULL,
  PRIMARY KEY (`user_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `triggers_loc_users`
--

/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_AUTO_VALUE_ON_ZERO' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `Copy_To_Loc_Users` AFTER INSERT ON `triggers_loc_users` FOR EACH ROW INSERT INTO loc_users(`user_level`, `full_name`, `username`, `password`, `contact_number`, `email`, `position`, `gender`, `created_at`, `updated_at`, `active`, `guid`, `store_id`, `uniq_id`)
SELECT `user_level`, `full_name`, `username`, `password`, `contact_number`, `email`, `position`, `gender`, `created_at`, `updated_at`, `active`, `guid`, `store_id`, `uniq_id`
  FROM Triggers_loc_users
 WHERE NOT EXISTS(SELECT `user_level`, `full_name`, `username`, `password`, `contact_number`, `email`, `position`, `gender`, `created_at`, `updated_at`, `active`, `guid`, `store_id`, `uniq_id`
                    FROM loc_users
                   WHERE loc_users.uniq_id = Triggers_loc_users.uniq_id ) */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2020-06-16  9:42:52
