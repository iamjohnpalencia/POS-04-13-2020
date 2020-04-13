Imports System.Data.SqlClient
Imports System.Data
Imports System.IO
Module SqlModule
    Dim myConnectDB As System.Data.SqlClient.SqlConnection
    Dim myConnectStr As String
    Dim mySQLCommand As System.Data.SqlClient.SqlCommand
    Dim myReader As System.Data.SqlClient.SqlDataReader
    Dim SqlQuery As String
    Public Sub Main()
        'Connection to my SQL Server database
        myConnectStr = "Server=<HOSTNAME\INSTANCE>;Database=<DBNAME>;User Id=<USER>;Password=<PASSWORD>;"
        myConnectDB = New System.Data.SqlClient.SqlConnection(myConnectStr)
        myConnectDB.Open()
        ' SQL Select statement to read data from the desired table
        mySQLCommand = New System.Data.SqlClient.SqlCommand("Select * FROM STW_USERS;", myConnectDB)
        Dim myReader As SqlDataReader = mySQLCommand.ExecuteReader()
        Dim fileName As String = "C:\MyQuery.txt"
        'create a stream object which can write text to a file
        Dim outputStream As StreamWriter = New StreamWriter(fileName)
        Do While myReader.Read
            Dim values(myReader.FieldCount - 1) As Object
            'get all the field values
            myReader.GetValues(values)
            'write the text of each value to a comma seperated string
            Dim line As String = String.Join(",", values)
            outputStream.WriteLine(line)
        Loop
        myReader.Close()
        outputStream.Close()
        myConnectDB.Close()
    End Sub
    Public Function MysqlTables()
        SqlQuery = "
CREATE DATABASE IF NOT EXISTS `pos` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
USE `pos`;

-- --------------------------------------------------------

--
-- Table structure for table `admin_coupon`
--

DROP TABLE IF EXISTS `admin_coupon`;
CREATE TABLE IF NOT EXISTS `admin_coupon` (
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

-- --------------------------------------------------------

--
-- Table structure for table `admin_masterlist`
--

DROP TABLE IF EXISTS `admin_masterlist`;
CREATE TABLE IF NOT EXISTS `admin_masterlist` (
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

-- --------------------------------------------------------

--
-- Table structure for table `admin_outlets`
--

DROP TABLE IF EXISTS `admin_outlets`;
CREATE TABLE IF NOT EXISTS `admin_outlets` (
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

-- --------------------------------------------------------

--
-- Table structure for table `loc_admin_category`
--

DROP TABLE IF EXISTS `loc_admin_category`;
CREATE TABLE IF NOT EXISTS `loc_admin_category` (
  `category_id` int(11) NOT NULL AUTO_INCREMENT,
  `category_name` varchar(50) NOT NULL,
  `brand_name` varchar(255) NOT NULL,
  `updated_at` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `origin` varchar(50) NOT NULL,
  `status` tinyint(2) NOT NULL,
  PRIMARY KEY (`category_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `loc_admin_products`
--

DROP TABLE IF EXISTS `loc_admin_products`;
CREATE TABLE IF NOT EXISTS `loc_admin_products` (
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

-- --------------------------------------------------------

--
-- Table structure for table `loc_daily_transaction`
--

DROP TABLE IF EXISTS `loc_daily_transaction`;
CREATE TABLE IF NOT EXISTS `loc_daily_transaction` (
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
  `ip_address` varchar(50) NOT NULL,
  `active` varchar(2) NOT NULL,
  `store_id` varchar(11) NOT NULL,
  `date` date NOT NULL,
  `time` time NOT NULL,
  `transaction_type` varchar(50) NOT NULL,
  `shift` varchar(255) NOT NULL,
  `zreading` date NOT NULL,
  `synced` varchar(255) NOT NULL,
  PRIMARY KEY (`transaction_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `loc_daily_transaction_details`
--

DROP TABLE IF EXISTS `loc_daily_transaction_details`;
CREATE TABLE IF NOT EXISTS `loc_daily_transaction_details` (
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
  `synced` varchar(255) NOT NULL,
  PRIMARY KEY (`details_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `loc_deposit`
--

DROP TABLE IF EXISTS `loc_deposit`;
CREATE TABLE IF NOT EXISTS `loc_deposit` (
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

-- --------------------------------------------------------

--
-- Table structure for table `loc_expenses_hold`
--

DROP TABLE IF EXISTS `loc_expenses_hold`;
CREATE TABLE IF NOT EXISTS `loc_expenses_hold` (
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

-- --------------------------------------------------------

--
-- Table structure for table `loc_expense_details`
--

DROP TABLE IF EXISTS `loc_expense_details`;
CREATE TABLE IF NOT EXISTS `loc_expense_details` (
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
  `synced` varchar(255) NOT NULL,
  PRIMARY KEY (`expense_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `loc_expense_list`
--

DROP TABLE IF EXISTS `loc_expense_list`;
CREATE TABLE IF NOT EXISTS `loc_expense_list` (
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
  `synced` varchar(255) NOT NULL,
  PRIMARY KEY (`expense_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `loc_fm_stock`
--

DROP TABLE IF EXISTS `loc_fm_stock`;
CREATE TABLE IF NOT EXISTS `loc_fm_stock` (
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

-- --------------------------------------------------------

--
-- Table structure for table `loc_hold_inventory`
--

DROP TABLE IF EXISTS `loc_hold_inventory`;
CREATE TABLE IF NOT EXISTS `loc_hold_inventory` (
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

-- --------------------------------------------------------

--
-- Table structure for table `loc_inbox_messages`
--

DROP TABLE IF EXISTS `loc_inbox_messages`;
CREATE TABLE IF NOT EXISTS `loc_inbox_messages` (
  `inbox_id` int(11) NOT NULL AUTO_INCREMENT,
  `crew_id` int(11) NOT NULL,
  `message` varchar(255) NOT NULL,
  `type` varchar(20) NOT NULL,
  `date_time` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `origin` varchar(20) NOT NULL,
  `active` tinyint(2) NOT NULL,
  PRIMARY KEY (`inbox_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `loc_inv_temp_data`
--

DROP TABLE IF EXISTS `loc_inv_temp_data`;
CREATE TABLE IF NOT EXISTS `loc_inv_temp_data` (
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

-- --------------------------------------------------------

--
-- Table structure for table `loc_partners_transaction`
--

DROP TABLE IF EXISTS `loc_partners_transaction`;
CREATE TABLE IF NOT EXISTS `loc_partners_transaction` (
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
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `loc_pending_orders`
--

DROP TABLE IF EXISTS `loc_pending_orders`;
CREATE TABLE IF NOT EXISTS `loc_pending_orders` (
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

-- --------------------------------------------------------

--
-- Table structure for table `loc_pos_client`
--

DROP TABLE IF EXISTS `loc_pos_client`;
CREATE TABLE IF NOT EXISTS `loc_pos_client` (
  `pos_client_id` int(11) NOT NULL AUTO_INCREMENT,
  `client_number` int(11) NOT NULL,
  `client_date_time` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `active` tinyint(2) NOT NULL,
  PRIMARY KEY (`pos_client_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `loc_pos_inventory`
--

DROP TABLE IF EXISTS `loc_pos_inventory`;
CREATE TABLE IF NOT EXISTS `loc_pos_inventory` (
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
  `synced` varchar(255) NOT NULL,
  PRIMARY KEY (`inventory_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `loc_product_formula`
--

DROP TABLE IF EXISTS `loc_product_formula`;
CREATE TABLE IF NOT EXISTS `loc_product_formula` (
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
  `created_at` timestamp NOT NULL DEFAULT current_timestamp(),
  `unit_cost` decimal(11,2) NOT NULL,
  `store_id` varchar(50) NOT NULL,
  `guid` varchar(255) NOT NULL,
  PRIMARY KEY (`formula_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `loc_promo_coupon_list`
--

DROP TABLE IF EXISTS `loc_promo_coupon_list`;
CREATE TABLE IF NOT EXISTS `loc_promo_coupon_list` (
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

-- --------------------------------------------------------

--
-- Table structure for table `loc_refund_return_details`
--

DROP TABLE IF EXISTS `loc_refund_return_details`;
CREATE TABLE IF NOT EXISTS `loc_refund_return_details` (
  `refret_id` int(11) NOT NULL AUTO_INCREMENT,
  `transaction_number` varchar(255) NOT NULL,
  `crew_id` varchar(50) NOT NULL,
  `reason` text NOT NULL,
  `total` decimal(11,2) NOT NULL,
  `guid` varchar(255) NOT NULL,
  `ipaddress` varchar(255) NOT NULL,
  `store_id` int(11) NOT NULL,
  `datestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `synced` varchar(255) NOT NULL,
  PRIMARY KEY (`refret_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `loc_settings`
--

DROP TABLE IF EXISTS `loc_settings`;
CREATE TABLE IF NOT EXISTS `loc_settings` (
  `settings_id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(255) NOT NULL,
  `value` varchar(255) NOT NULL,
  `date` date NOT NULL,
  `datestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`settings_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `loc_system_logs`
--

DROP TABLE IF EXISTS `loc_system_logs`;
CREATE TABLE IF NOT EXISTS `loc_system_logs` (
  `crew_id` varchar(50) NOT NULL,
  `log_type` varchar(255) NOT NULL,
  `log_description` text NOT NULL,
  `log_date_time` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `log_store` varchar(20) NOT NULL,
  `guid` varchar(255) NOT NULL,
  `ip_address` varchar(255) NOT NULL,
  `loc_systemlog_id` varchar(255) NOT NULL,
  `synced` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `loc_transaction_mode_details`
--

DROP TABLE IF EXISTS `loc_transaction_mode_details`;
CREATE TABLE IF NOT EXISTS `loc_transaction_mode_details` (
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

-- --------------------------------------------------------

--
-- Table structure for table `loc_users`
--

DROP TABLE IF EXISTS `loc_users`;
CREATE TABLE IF NOT EXISTS `loc_users` (
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

-- --------------------------------------------------------

--
-- Table structure for table `tbcoupon`
--

DROP TABLE IF EXISTS `tbcoupon`;
CREATE TABLE IF NOT EXISTS `tbcoupon` (
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
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `triggers_loc_admin_products`
--

DROP TABLE IF EXISTS `triggers_loc_admin_products`;
CREATE TABLE IF NOT EXISTS `triggers_loc_admin_products` (
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

--
-- Triggers `triggers_loc_admin_products`
--
DROP TRIGGER IF EXISTS `Copy_To_Loc_admin_products`;
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

DROP TABLE IF EXISTS `triggers_loc_users`;
CREATE TABLE IF NOT EXISTS `triggers_loc_users` (
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

--
-- Triggers `triggers_loc_users`
--
DROP TRIGGER IF EXISTS `Copy_To_Loc_Users`;
DELIMITER $$
CREATE TRIGGER `Copy_To_Loc_Users` AFTER INSERT ON `triggers_loc_users` FOR EACH ROW INSERT INTO loc_users(`user_level`, `full_name`, `username`, `password`, `contact_number`, `email`, `position`, `gender`, `created_at`, `updated_at`, `active`, `guid`, `store_id`, `uniq_id`)
SELECT `user_level`, `full_name`, `username`, `password`, `contact_number`, `email`, `position`, `gender`, `created_at`, `updated_at`, `active`, `guid`, `store_id`, `uniq_id`
  FROM Triggers_loc_users
 WHERE NOT EXISTS(SELECT `user_level`, `full_name`, `username`, `password`, `contact_number`, `email`, `position`, `gender`, `created_at`, `updated_at`, `active`, `guid`, `store_id`, `uniq_id`
                    FROM loc_users
                   WHERE loc_users.uniq_id = Triggers_loc_users.uniq_id )
$$
DELIMITER ;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
"
        Return SqlQuery
    End Function
End Module
