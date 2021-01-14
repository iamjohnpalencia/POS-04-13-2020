ALTER TABLE `loc_price_request_change` ADD `store_name` TEXT NOT NULL AFTER `request_id`;

ALTER TABLE `loc_settings` ADD `printreceipt` TEXT NOT NULL AFTER `S_Layout`, ADD `reprintreceipt` TEXT NOT NULL AFTER `printreceipt`, ADD `printxzread` TEXT NOT NULL AFTER `reprintreceipt`;

UPDATE `loc_settings` SET `printreceipt`= "YES",`reprintreceipt`="YES",`printxzread`= "YES" WHERE settings_id = 1;

