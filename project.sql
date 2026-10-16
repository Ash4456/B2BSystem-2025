-- phpMyAdmin SQL Dump
-- version 4.7.4
-- https://www.phpmyadmin.net/
--
-- 主機: 127.0.0.1
-- 產生時間： 2026-06-25 16:40:49
-- 伺服器版本: 10.1.30-MariaDB
-- PHP 版本： 5.6.33

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- 資料庫： `project`
--

-- --------------------------------------------------------

--
-- 資料表結構 `afterservicerequest`
--

CREATE TABLE `afterservicerequest` (
  `RequestID` int(11) NOT NULL,
  `OrderID` varchar(20) DEFAULT NULL,
  `RequestType` varchar(50) DEFAULT NULL,
  `Reason` text,
  `Status` varchar(50) DEFAULT 'Pending',
  `RequestDate` datetime DEFAULT NULL,
  `HandledBy` varchar(100) DEFAULT NULL,
  `ForwardTo` varchar(2) DEFAULT 'CS',
  `PhotoPath` varchar(500) CHARACTER SET utf8 DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- 資料表的匯出資料 `afterservicerequest`
--

INSERT INTO `afterservicerequest` (`RequestID`, `OrderID`, `RequestType`, `Reason`, `Status`, `RequestDate`, `HandledBy`, `ForwardTo`, `PhotoPath`) VALUES
(1, 'ORa4711', 'Return', 'gff', 'Confirmed', '2026-06-12 13:10:36', 'Joey Yung', 'CS', NULL),
(2, 'OR7d289', 'Return', 'GGG', 'Confirmed', '2026-06-21 15:09:30', 'Joey Yung', 'CS', NULL),
(3, 'OR8f4d3', 'Return', 'ggg', 'Confirmed', '2026-06-23 17:51:14', 'Joey Yung', 'SA', NULL),
(4, 'OR0d283', 'Replacement', 'ffff', 'Pending', '2026-06-23 17:52:48', 'Joey Yung', 'LO', NULL),
(5, 'OR4ad68', 'Return', 'dfghjkuio', 'Confirmed', '2026-06-23 19:43:56', 'Joey Yung', 'CS', NULL),
(6, 'OR8cd5d', 'Return', 'gfh', 'Confirmed', '2026-06-23 19:44:58', 'Joey Yung', 'CS', NULL),
(7, 'OR0d283', 'Return', 'dfghjkl;', 'Pending', '2026-06-25 13:08:55', 'Joey Yung', 'CS', 'RequestPhotos\\20260625_130855_AdorableCutieChiikawa (1).jpg'),
(8, 'OR0d283', 'Return', 'fthgh', 'Pending', '2026-06-25 13:44:22', 'Joey Yung', 'CS', 'RequestPhotos\\20260625_134422_AdorableCutieChiikawa (1).jpg');

-- --------------------------------------------------------

--
-- 資料表結構 `delivery`
--

CREATE TABLE `delivery` (
  `DeliveryID` varchar(20) NOT NULL,
  `OrderID` varchar(20) NOT NULL,
  `DeliveryDate` datetime DEFAULT NULL,
  `DeliveryStatus` varchar(20) DEFAULT 'Pending',
  `HandledBy` varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- 資料表的匯出資料 `delivery`
--

INSERT INTO `delivery` (`DeliveryID`, `OrderID`, `DeliveryDate`, `DeliveryStatus`, `HandledBy`) VALUES
('DV22969269', 'OR3c1ed', '2026-06-25 23:32:26', 'Pending', 'Anson Lo'),
('DV27541833', 'OR99300', '2026-06-24 02:37:16', 'Pending', 'Anson Lo'),
('DV28519082', 'OR92d11', '2026-07-01 20:19:57', 'Pending', 'Anson Lo'),
('DV41092667', 'OR1d544', '2026-07-05 19:04:08', 'Pending', 'Anson Lo'),
('DV43649550', 'OR8a670', '2026-06-25 18:46:39', 'Complete', 'Anson Lo'),
('DV45371653', 'ORa4711', '2026-06-25 18:54:17', 'Delivery', 'Anson Lo'),
('DV51288749', 'ORec13f', '2026-06-26 19:18:55', 'Pending', 'Anson Lo'),
('DV52308819', 'OR0d283', '2026-09-24 16:16:01', 'Pending', 'Anson Lo'),
('DV54099784', 'OR39d0d', '2026-06-23 14:43:40', 'Pending', 'Anson Lo'),
('DV56049381', 'OR7d289', '2026-06-25 18:51:03', 'Delivery', 'Anson Lo'),
('DV56898569', 'OR4ad68', '2026-06-23 14:52:47', 'Pending', 'Anson Lo'),
('DV63296356', 'OR451f5', '2026-06-27 23:34:27', 'Pending', 'Anson Lo'),
('DV66923634', 'OR95832', '2026-06-25 19:03:44', 'Delivery', 'Anson Lo'),
('DV71764079', 'OR8cd5d', '2026-07-07 18:24:57', 'Pending', 'Anson Lo'),
('DV85712326', 'OR8f4d3', '2026-06-25 18:46:52', 'Complete', 'Anson Lo');

-- --------------------------------------------------------

--
-- 資料表結構 `employee`
--

CREATE TABLE `employee` (
  `EmployeeID` char(5) NOT NULL,
  `EmployeeName` varchar(100) NOT NULL,
  `Password` varchar(20) NOT NULL,
  `PhoneNumber` char(8) NOT NULL,
  `Department` char(2) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- 資料表的匯出資料 `employee`
--

INSERT INTO `employee` (`EmployeeID`, `EmployeeName`, `Password`, `PhoneNumber`, `Department`) VALUES
('10001', 'Keung To', '12345', '94301999', 'SA'),
('10002', 'Joey Yung', '12345', '61619801', 'CS'),
('10003', 'Hins Cheung', '12345', '51980201', 'IV'),
('10004', 'Edan Lui', '12345', '91997121', 'FI'),
('10005', 'Anson Lo', '12345', '61995707', 'LO'),
('10006', 'Anson Kong', '12345', '99211016', 'PR'),
('10007', 'Ian Chan', '12345', '99310609', 'MA'),
('10008', 'Louis Chan', '12345', '91990614', 'MA'),
('10009', 'Alex Chan', '12345', '61234567', 'SA'),
('10010', 'Chris Wong', '12345', '62345678', 'CS'),
('10011', 'Jason Lee', '12345', '63456789', 'IV'),
('10012', 'Kelvin Ho', '12345', '64567890', 'LO'),
('10013', 'David Lam', '12345', '65678901', 'CS'),
('10014', 'Peter Ng', '12345', '66789012', 'PR'),
('10015', 'Eric Cheung', '12345', '67890123', 'FI'),
('10016', 'Michael Lau', '12345', '68901234', 'SA'),
('10017', 'Tony Yip', '12345', '69012345', 'CS'),
('10018', 'Samuel Kwok', '12345', '60123456', 'IV'),
('10019', 'Raymond Yeung', '12345', '61237890', 'LO'),
('10020', 'Andy Tsui', '12345', '62348901', 'CS'),
('10021', 'Wilson Chiu', '12345', '63459012', 'PR'),
('10022', 'Jacky Cheng', '12345', '64560123', 'FI'),
('10023', 'Bryan Mok', '12345', '65671234', 'SA'),
('10024', 'Leo Fung', '12345', '66782345', 'CS'),
('10025', 'Gary Poon', '12345', '67893456', 'IV'),
('10026', 'Martin Ko', '12345', '68904567', 'LO'),
('10027', 'Henry Tang', '12345', '69015678', 'PR'),
('10028', 'Ivan Lo', '12345', '60126789', 'FI'),
('10029', 'Daniel Hui', '12345', '61237891', 'SA'),
('10030', 'Victor So', '12345', '62348902', 'CS'),
('10031', 'Oscar Wu', '12345', '63459013', 'IV'),
('10032', 'Ben Ma', '12345', '64560124', 'LO'),
('10033', 'Kevin To', '12345', '65671235', 'PR'),
('10034', 'Simon Law', '12345', '66782346', 'FI'),
('10035', 'Tim Chan', '12345', '67893457', 'MA'),
('10036', 'Frank Yau', '12345', '68904568', 'CS'),
('10037', 'Jason Ng', '12345', '69015679', 'IV'),
('10038', 'Gary Lee', '12345', '60126780', 'LO'),
('10039', 'Aaron Chan', '12345', '61230001', 'SA'),
('10040', 'Brian Wong', '12345', '61230002', 'MA'),
('10041', 'Carlos Lee', '12345', '61230003', 'MA'),
('10042', 'Derek Ho', '12345', '61230004', 'LO'),
('10043', 'Ethan Lam', '12345', '61230005', 'PR'),
('10044', 'Felix Ng', '12345', '61230006', 'FI'),
('10045', 'George Cheung', '12345', '61230007', 'SA'),
('10046', 'Harold Lau', '12345', '61230008', 'CS'),
('10047', 'Ian Yip', '12345', '61230009', 'IV'),
('10048', 'Justin Kwok', '12345', '61230010', 'LO'),
('10049', 'Kenneth Yeung', '12345', '61230011', 'PR'),
('10050', 'Louis Tsui', '12345', '61230012', 'FI'),
('10051', 'Marcus Chiu', '12345', '61230013', 'SA'),
('10052', 'Nathan Cheng', '12345', '61230014', 'CS'),
('10053', 'Oliver Mok', '12345', '61230015', 'IV'),
('10054', 'Patrick Fung', '12345', '61230016', 'LO'),
('10055', 'Quincy Poon', '12345', '61230017', 'PR'),
('10056', 'Ryan Ko', '12345', '61230018', 'FI'),
('10057', 'Steven Tang', '12345', '61230019', 'SA'),
('10058', 'Thomas Lo', '12345', '61230020', 'CS'),
('10059', 'Arthur Chan', '12345', '61230021', 'IV'),
('10060', 'Barry Wong', '12345', '61230022', 'LO'),
('10061', 'Cyrus Lee', '12345', '61230023', 'SA'),
('10062', 'Dennis Ho', '12345', '61230024', 'CS'),
('10063', 'Eddie Lam', '12345', '61230025', 'PR'),
('10064', 'Freddy Ng', '12345', '61230026', 'FI'),
('10065', 'Gordon Cheung', '12345', '61230027', 'MA'),
('10066', 'Hugo Lau', '12345', '61230028', 'CS'),
('10067', 'Isaac Yip', '12345', '61230029', 'IV'),
('10068', 'Jason Kwok', '12345', '61230030', 'LO'),
('10069', 'Kyle Yeung', '12345', '61230031', 'PR'),
('10070', 'Leon Tsui', '12345', '61230032', 'FI'),
('10071', 'Mason Chiu', '12345', '61230033', 'SA'),
('10072', 'Neil Cheng', '12345', '61230034', 'CS'),
('10073', 'Owen Mok', '12345', '61230035', 'IV'),
('10074', 'Paul Fung', '12345', '61230036', 'LO'),
('10075', 'Quentin Poon', '12345', '61230037', 'PR'),
('10076', 'Ricky Ko', '12345', '61230038', 'FI'),
('10077', 'Shawn Tang', '12345', '61230039', 'SA'),
('10078', 'Timothy Lo', '12345', '61230040', 'CS'),
('10079', 'Ulysses Hui', '12345', '61230041', 'IV'),
('10080', 'Victor Chan', '12345', '61230042', 'LO'),
('10081', 'Wayne Wong', '12345', '61230043', 'PR'),
('10082', 'Xavier Lee', '12345', '61230044', 'FI'),
('10083', 'Yuri Ho', '12345', '61230045', 'SA'),
('10084', 'Zane Lam', '12345', '61230046', 'CS'),
('10085', 'Adrian Ng', '12345', '61230047', 'IV'),
('10086', 'Brandon Cheung', '12345', '61230048', 'LO'),
('10087', 'Calvin Lau', '12345', '61230049', 'MA'),
('10088', 'Dylan Yip', '12345', '61230050', 'FI'),
('10089', 'Evan Kwok', '12345', '61230051', 'MA'),
('10090', 'Felix Yeung', '12345', '61230052', 'CS'),
('10091', 'Gavin Tsui', '12345', '61230053', 'IV'),
('10092', 'Howard Chiu', '12345', '61230054', 'MA'),
('10093', 'Ivan Cheng', '12345', '61230055', 'PR'),
('10094', 'Jason Mok', '12345', '61230056', 'FI'),
('10095', 'Kevin Fung', '12345', '61230057', 'SA'),
('10096', 'Leon Poon', '12345', '61230058', 'CS'),
('10097', 'Marcus Ko', '12345', '61230059', 'IV'),
('10098', 'Nathan Tang', '12345', '61230060', 'LO'),
('10099', 'Oscar Lo', '12345', '61230061', 'PR'),
('10100', 'Patrick Hui', '12345', '61230062', 'FI');

-- --------------------------------------------------------

--
-- 資料表結構 `invoice`
--

CREATE TABLE `invoice` (
  `InvoiceID` varchar(20) NOT NULL,
  `OrderID` varchar(20) NOT NULL,
  `PaymentMethod` varchar(50) DEFAULT NULL,
  `TotalAmount` decimal(10,2) NOT NULL,
  `CreatedBy` varchar(50) DEFAULT NULL,
  `CreatedDate` datetime DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- 資料表的匯出資料 `invoice`
--

INSERT INTO `invoice` (`InvoiceID`, `OrderID`, `PaymentMethod`, `TotalAmount`, `CreatedBy`, `CreatedDate`) VALUES
('IV12910246', 'ORa4711', 'Cash', '10200.00', 'Ian Chan', '2026-06-18 18:24:44'),
('IV19332415', 'OR39d0d', 'Alipayhk', '6500.00', 'Finance Staff', '2026-06-17 00:22:01'),
('IV24505277', 'OR92d11', 'Cash', '21080.00', 'Edan Lui', '2026-06-25 19:11:19'),
('IV27586902', 'OR4ad68', 'Cash', '7780.00', 'Edan Lui', '2026-06-17 00:28:30'),
('IV27672353', 'OR8f4d3', 'Alipayhk', '1280.00', 'Edan Lui', '2026-06-17 00:25:46'),
('IV27730199', 'OR8a670', 'Alipayhk', '4980.00', 'Ian Chan', '2026-06-19 01:24:13'),
('IV64336873', 'OR95832', 'Cash', '6880.00', 'Edan Lui', '2026-06-19 01:25:26'),
('IV79230009', 'OR7d289', 'Cash', '12530.00', 'Ian Chan', '2026-06-18 17:41:08');

-- --------------------------------------------------------

--
-- 資料表結構 `orders`
--

CREATE TABLE `orders` (
  `OrderID` varchar(20) NOT NULL,
  `CustomerName` varchar(100) DEFAULT NULL,
  `CustomerPhone` char(8) DEFAULT NULL,
  `CustomerEmail` varchar(100) DEFAULT NULL,
  `DeliveryAddress` varchar(255) DEFAULT NULL,
  `OrderDate` datetime DEFAULT NULL,
  `DeliveryDate` datetime DEFAULT NULL,
  `TotalAmount` decimal(10,2) DEFAULT NULL,
  `Status` varchar(20) DEFAULT NULL,
  `CreatedBy` varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- 資料表的匯出資料 `orders`
--

INSERT INTO `orders` (`OrderID`, `CustomerName`, `CustomerPhone`, `CustomerEmail`, `DeliveryAddress`, `OrderDate`, `DeliveryDate`, `TotalAmount`, `Status`, `CreatedBy`) VALUES
('OR0d283', 'fff', '12345678', 'dddd@gmail.com', 'dddd', '2026-06-23 16:16:24', '2026-09-24 16:16:01', '11480.00', 'Pending', 'Keung To'),
('OR1d544', 'rfr', '12345678', 'dd@.', 'ghhk', '2026-06-25 19:04:34', '2026-07-05 19:04:08', '8180.00', 'Pending', 'Keung To'),
('OR39d0d', 'CC', '67890124', 'FFF@gmail.com', 'eee', '2026-06-13 14:44:08', '2026-06-23 14:43:40', '6500.00', 'Cancel', 'Keung To'),
('OR3c1ed', 'fff', '12345678', 'ggg@gmail.com', 'dddd', '2026-06-15 23:32:47', '2026-06-25 23:32:26', '4980.00', 'Cancel', 'Keung To'),
('OR451f5', 'ff', '12345678', 'hhh@gmail.com', 'dddd', '2026-06-15 23:34:57', '2026-06-27 23:34:27', '4980.00', 'Cancel', 'Keung To'),
('OR4ad68', 'ff', '12345678', 'fff@gmail.com', 'fff', '2026-06-13 14:53:06', '2026-06-23 14:52:47', '7780.00', 'Cancel', 'Keung To'),
('OR7d289', 'yyyy', '34567893', 'nnnnn@gmail.com', 'hhh', '2026-06-15 22:47:04', '2026-06-25 22:46:35', '12530.00', 'Delivery', 'Keung To'),
('OR8a670', 'CCC', '34567890', 'VVV@GMAIL.COM', 'rfff', '2026-06-14 13:47:40', '2026-06-24 13:47:23', '4980.00', 'Complete', 'Keung To'),
('OR8cd5d', 'sss', '12345678', 'ggg@gmail.com', 'jngjnng', '2026-06-15 18:25:45', '2026-07-07 18:24:57', '6100.00', 'Pending', 'Keung To'),
('OR8f4d3', 'TD', '34216789', 'KKK@gmail.com', 'ddd', '2026-06-13 15:08:52', '2026-06-23 15:08:32', '1280.00', 'Complete', 'Keung To'),
('OR92d11', 'AA', '12345678', 'DDD@gmail.com', 'ddddd', '2026-06-21 20:20:50', '2026-07-01 20:19:57', '21080.00', 'Paid', 'Keung To'),
('OR95832', 'jjnj', '12345678', 'ffff@g.com', 'gggg', '2026-06-15 22:52:51', '2026-06-25 22:52:12', '6880.00', 'Delivery', 'Keung To'),
('OR99300', 'FDD', '23456789', 'GGG@Gmail.com', 'eeeee', '2026-06-14 02:37:48', '2026-06-24 02:37:16', '2560.00', 'Pending', 'Keung To'),
('ORa4711', 'Chan', '93478321', 'happy@gmail.com', 'ABC buliding 6/F Room A', '2026-06-12 00:05:17', '2026-06-24 00:04:11', '10200.00', 'Delivery', 'Keung To'),
('ORec13f', 'CCC', '12345678', 'JJJ@gmail.com', 'ggggggg', '2026-06-16 19:19:38', '2026-06-26 19:18:55', '8180.00', 'Cancel', 'Keung To');

-- --------------------------------------------------------

--
-- 資料表結構 `order_items`
--

CREATE TABLE `order_items` (
  `OrderItemID` int(11) NOT NULL,
  `ProductID` char(10) DEFAULT NULL,
  `ProductName` varchar(100) DEFAULT NULL,
  `Qty` int(11) DEFAULT NULL,
  `UnitPrice` decimal(10,2) DEFAULT NULL,
  `SubTotal` decimal(10,2) DEFAULT NULL,
  `OrderID` varchar(20) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- 資料表的匯出資料 `order_items`
--

INSERT INTO `order_items` (`OrderItemID`, `ProductID`, `ProductName`, `Qty`, `UnitPrice`, `SubTotal`, `OrderID`) VALUES
(23, 'PD00000006', 'Sliding Door Wardrobe', 1, '5600.00', '5600.00', 'ORa4711'),
(24, 'PD00000008', 'L-Shaped Office Desk   ', 2, '2300.00', '4600.00', 'ORa4711'),
(25, 'PD00000002', 'Solid Wood Dining Table ', 1, '6500.00', '6500.00', 'OR39d0d'),
(26, 'PD00000003', 'Ergonomic Office Chair ', 1, '1280.00', '1280.00', 'OR4ad68'),
(27, 'PD00000002', 'Solid Wood Dining Table ', 1, '6500.00', '6500.00', 'OR4ad68'),
(28, 'PD00000003', 'Ergonomic Office Chair ', 1, '1280.00', '1280.00', 'OR8f4d3'),
(29, 'PD00000003', 'Ergonomic Office Chair ', 1, '1280.00', '1280.00', 'OR99300'),
(30, 'PD00000003', 'Ergonomic Office Chair ', 1, '1280.00', '1280.00', 'OR99300'),
(31, 'PD00000001', 'Modern Fabric Sofa', 1, '4980.00', '4980.00', 'OR8a670'),
(32, 'PD00000005', 'Queen Size Bed Frame ', 1, '3800.00', '3800.00', 'OR8cd5d'),
(33, 'PD00000008', 'L-Shaped Office Desk   ', 1, '2300.00', '2300.00', 'OR8cd5d'),
(34, 'PD00000001', 'Modern Fabric Sofa', 1, '4980.00', '4980.00', 'OR7d289'),
(35, 'PD00000006', 'Sliding Door Wardrobe', 1, '5600.00', '5600.00', 'OR7d289'),
(36, 'PD00000009', 'Wooden Bookshelf Unit ', 1, '1950.00', '1950.00', 'OR7d289'),
(37, 'PD00000003', 'Ergonomic Office Chair ', 1, '1280.00', '1280.00', 'OR95832'),
(38, 'PD00000006', 'Sliding Door Wardrobe', 1, '5600.00', '5600.00', 'OR95832'),
(42, 'PD00000001', 'Modern Fabric Sofa', 1, '4980.00', '4980.00', 'OR451f5'),
(43, 'PD00000002', 'Solid Wood Dining Table ', 1, '6500.00', '6500.00', 'OR451f5'),
(44, 'PD00000008', 'L-Shaped Office Desk   ', 1, '2300.00', '2300.00', 'OR451f5'),
(45, 'PD00000001', 'Modern Fabric Sofa', 1, '4980.00', '4980.00', 'ORec13f'),
(46, 'PD00000010', 'Leather Recliner Chair  ', 1, '3200.00', '3200.00', 'ORec13f'),
(47, 'PD00000001', 'Modern Fabric Sofa', 1, '4980.00', '4980.00', 'OR92d11'),
(48, 'PD00000002', 'Solid Wood Dining Table ', 1, '6500.00', '6500.00', 'OR92d11'),
(49, 'PD00000010', 'Leather Recliner Chair  ', 3, '3200.00', '9600.00', 'OR92d11'),
(50, 'PD00000002', 'Solid Wood Dining Table ', 1, '6500.00', '6500.00', 'OR0d283'),
(51, 'PD00000001', 'Modern Fabric Sofa', 1, '4980.00', '4980.00', 'OR0d283'),
(52, 'PD00000001', 'Modern Fabric Sofa', 1, '4980.00', '4980.00', 'OR1d544'),
(53, 'PD00000010', 'Leather Recliner Chair  ', 1, '3200.00', '3200.00', 'OR1d544');

-- --------------------------------------------------------

--
-- 資料表結構 `procurement`
--

CREATE TABLE `procurement` (
  `ProcurementID` varchar(20) NOT NULL,
  `EmployeeID` char(5) NOT NULL,
  `DeliveryDate` datetime DEFAULT NULL,
  `Status` varchar(20) DEFAULT 'Pending',
  `TotalItems` int(11) DEFAULT '0',
  `CreatedDate` datetime DEFAULT CURRENT_TIMESTAMP,
  `CreatedBy` varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- 資料表的匯出資料 `procurement`
--

INSERT INTO `procurement` (`ProcurementID`, `EmployeeID`, `DeliveryDate`, `Status`, `TotalItems`, `CreatedDate`, `CreatedBy`) VALUES
('PO33646565', '10003', '2026-06-28 15:06:07', 'Pending', 2, '2026-06-21 15:06:13', 'Hins Cheung'),
('PO38708889', '10003', '2026-06-29 15:21:36', 'Pending', 2, '2026-06-19 15:21:44', 'Hins Cheung'),
('PO64238521', '10003', '2026-07-02 22:18:12', 'Pending', 2, '2026-06-25 22:19:12', 'Hins Cheung');

-- --------------------------------------------------------

--
-- 資料表結構 `procurement_items`
--

CREATE TABLE `procurement_items` (
  `ProcurementItemID` int(11) NOT NULL,
  `ProcurementID` varchar(20) NOT NULL,
  `MaterialID` char(10) NOT NULL,
  `MaterialName` varchar(100) NOT NULL,
  `Quantity` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- 資料表的匯出資料 `procurement_items`
--

INSERT INTO `procurement_items` (`ProcurementItemID`, `ProcurementID`, `MaterialID`, `MaterialName`, `Quantity`) VALUES
(1, 'PO38708889', 'MT00000001', 'Solid Wood', 1),
(2, 'PO38708889', 'MT00000002', 'Plywood', 1),
(3, 'PO33646565', 'MT00000001', 'Solid Wood', 1),
(4, 'PO33646565', 'MT00000006', 'Aluminium', 1),
(5, 'PO64238521', 'MT00000002', 'Plywood', 1),
(6, 'PO64238521', 'MT00000001', 'Solid Wood', 1);

-- --------------------------------------------------------

--
-- 資料表結構 `product`
--

CREATE TABLE `product` (
  `ProductID` char(10) NOT NULL,
  `ProductName` varchar(100) NOT NULL,
  `unitPrice` decimal(10,2) NOT NULL,
  `Qty` int(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- 資料表的匯出資料 `product`
--

INSERT INTO `product` (`ProductID`, `ProductName`, `unitPrice`, `Qty`) VALUES
('PD00000001', 'Modern Fabric Sofa', '4980.00', 52),
('PD00000002', 'Solid Wood Dining Table ', '6500.00', 17),
('PD00000003', 'Ergonomic Office Chair ', '1280.00', 23),
('PD00000004', 'Glass Coffee Table', '1980.00', 66),
('PD00000005', 'Queen Size Bed Frame ', '3800.00', 42),
('PD00000006', 'Sliding Door Wardrobe', '5600.00', 15),
('PD00000007', 'Minimalist TV Cabinet  ', '2750.00', 22),
('PD00000008', 'L-Shaped Office Desk   ', '2300.00', 12),
('PD00000009', 'Wooden Bookshelf Unit ', '1950.00', 6),
('PD00000010', 'Leather Recliner Chair  ', '3200.00', 4);

-- --------------------------------------------------------

--
-- 資料表結構 `rawmaterialrequest`
--

CREATE TABLE `rawmaterialrequest` (
  `RequestID` varchar(20) NOT NULL,
  `EmployeeID` char(5) NOT NULL,
  `TakesTime` datetime DEFAULT NULL,
  `Status` varchar(20) DEFAULT 'Pending',
  `CreatedBy` varchar(50) DEFAULT NULL,
  `CreatedDate` datetime DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- 資料表的匯出資料 `rawmaterialrequest`
--

INSERT INTO `rawmaterialrequest` (`RequestID`, `EmployeeID`, `TakesTime`, `Status`, `CreatedBy`, `CreatedDate`) VALUES
('RQ16571243', '10006', '2026-06-25 19:50:28', 'Pending', 'Anson Kong', '2026-06-25 19:50:53'),
('RQ50337137', '10006', '2026-06-21 16:37:44', 'Pending', 'Anson Kong', '2026-06-21 16:37:49'),
('RQ65481695', '10006', '2026-06-18 01:21:42', 'Pending', 'Anson Kong', '2026-06-18 01:22:14'),
('RQ76805208', '10006', '2026-06-18 23:42:23', 'Pending', 'Anson Kong', '2026-06-18 23:42:33'),
('RQ81282469', '10006', '2026-06-17 19:17:11', 'Pending', 'Anson Kong', '2026-06-17 19:17:22'),
('RQ83618640', '10003', '2026-06-19 01:23:30', 'Pending-LO', 'Hins Cheung', '2026-06-19 01:23:30'),
('RQ89167928', '10006', '2026-06-25 19:50:28', 'Pending', 'Anson Kong', '2026-06-25 19:50:29'),
('RQ96534688', '10006', '2026-06-25 19:37:48', 'Pending', 'Anson Kong', '2026-06-25 19:37:50');

-- --------------------------------------------------------

--
-- 資料表結構 `rawmaterialrequestitem`
--

CREATE TABLE `rawmaterialrequestitem` (
  `RequestItemID` int(11) NOT NULL,
  `RequestID` varchar(20) NOT NULL,
  `MaterialID` char(10) NOT NULL,
  `MaterialName` varchar(100) NOT NULL,
  `Quantity` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- 資料表的匯出資料 `rawmaterialrequestitem`
--

INSERT INTO `rawmaterialrequestitem` (`RequestItemID`, `RequestID`, `MaterialID`, `MaterialName`, `Quantity`) VALUES
(1, 'RQ81282469', 'MT00000001', 'Solid Wood', 1),
(2, 'RQ81282469', 'MT00000002', 'Plywood', 1),
(3, 'RQ65481695', 'MT00000008', 'Copper', 4),
(4, 'RQ65481695', 'MT00000007', 'Rubber', 1),
(5, 'RQ65481695', 'MT00000001', 'Solid Wood', 1),
(6, 'RQ65481695', 'MT00000001', 'Solid Wood', 1),
(7, 'RQ76805208', 'MT00000001', 'Solid Wood', 1),
(8, 'RQ76805208', 'MT00000002', 'Plywood', 1),
(9, 'RQ83618640', 'MT00000004', 'Plastic', 100),
(10, 'RQ50337137', 'MT00000002', 'Plywood', 1),
(11, 'RQ96534688', 'MT00000001', 'Solid Wood', 6),
(12, 'RQ89167928', 'MT00000001', 'Solid Wood', 2),
(13, 'RQ16571243', 'MT00000001', 'Solid Wood', 27);

-- --------------------------------------------------------

--
-- 資料表結構 `rawmaterials`
--

CREATE TABLE `rawmaterials` (
  `MaterialID` char(10) NOT NULL,
  `MaterialName` varchar(100) NOT NULL,
  `Category` varchar(50) NOT NULL,
  `StockQty` int(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- 資料表的匯出資料 `rawmaterials`
--

INSERT INTO `rawmaterials` (`MaterialID`, `MaterialName`, `Category`, `StockQty`) VALUES
('MT00000001', 'Solid Wood', 'Wood', 20),
('MT00000002', 'Plywood', 'Wood', 200),
('MT00000003', 'Steel', 'Metal', 180),
('MT00000004', 'Plastic', 'Polymer', 300),
('MT00000005', 'Glass', 'Material', 80),
('MT00000006', 'Aluminium', 'Metal', 120),
('MT00000007', 'Rubber', 'Polymer', 90),
('MT00000008', 'Copper', 'Metal', 70);

-- --------------------------------------------------------

--
-- 資料表結構 `replyslip`
--

CREATE TABLE `replyslip` (
  `ReplyID` varchar(20) NOT NULL,
  `DeliveryID` varchar(20) NOT NULL,
  `OrderID` varchar(20) NOT NULL,
  `Remarks` text,
  `CreatedBy` varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- 資料表的匯出資料 `replyslip`
--

INSERT INTO `replyslip` (`ReplyID`, `DeliveryID`, `OrderID`, `Remarks`, `CreatedBy`) VALUES
('RS11395369', 'DV45371653', 'ORa4711', '', 'Anson Lo'),
('RS11660294', 'DV56898569', 'OR4ad68', '', 'Anson Lo'),
('RS18845762', 'DV56898569', 'OR4ad68', '', 'Staff'),
('RS22532168', 'DV51288749', 'ORec13f', '', 'Anson Lo'),
('RS25257534', 'DV43649550', 'OR8a670', '', 'Anson Lo'),
('RS28560996', 'DV63296356', 'OR451f5', '', 'Anson Lo'),
('RS32857784', 'DV28519082', 'OR92d11', '', 'Anson Lo'),
('RS35239761', 'DV71764079', 'OR8cd5d', '', 'Anson Lo'),
('RS40147753', 'DV52308819', 'OR0d283', '', 'Anson Lo'),
('RS43150942', 'DV85712326', 'OR8f4d3', '', 'Anson Lo'),
('RS44254157', 'DV27541833', 'OR99300', '', 'Anson Lo'),
('RS59000382', 'DV54099784', 'OR39d0d', '', 'Anson Lo'),
('RS61045570', 'DV66923634', 'OR95832', '', 'Anson Lo'),
('RS86341970', 'DV41092667', 'OR1d544', '', 'Anson Lo'),
('RS91000262', 'DV56049381', 'OR7d289', '', 'Anson Lo'),
('RS93937890', 'DV22969269', 'OR3c1ed', '', 'Anson Lo');

--
-- 已匯出資料表的索引
--

--
-- 資料表索引 `afterservicerequest`
--
ALTER TABLE `afterservicerequest`
  ADD PRIMARY KEY (`RequestID`),
  ADD KEY `fk_order_af` (`OrderID`);

--
-- 資料表索引 `delivery`
--
ALTER TABLE `delivery`
  ADD PRIMARY KEY (`DeliveryID`),
  ADD KEY `fk_delivery_order` (`OrderID`);

--
-- 資料表索引 `employee`
--
ALTER TABLE `employee`
  ADD PRIMARY KEY (`EmployeeID`),
  ADD UNIQUE KEY `EmployeeID` (`EmployeeID`);

--
-- 資料表索引 `invoice`
--
ALTER TABLE `invoice`
  ADD PRIMARY KEY (`InvoiceID`),
  ADD KEY `fk_invoice_order` (`OrderID`);

--
-- 資料表索引 `orders`
--
ALTER TABLE `orders`
  ADD PRIMARY KEY (`OrderID`);

--
-- 資料表索引 `order_items`
--
ALTER TABLE `order_items`
  ADD PRIMARY KEY (`OrderItemID`),
  ADD KEY `fk_product` (`ProductID`),
  ADD KEY `fk_order` (`OrderID`);

--
-- 資料表索引 `procurement`
--
ALTER TABLE `procurement`
  ADD PRIMARY KEY (`ProcurementID`),
  ADD KEY `fk_procurement_employee` (`EmployeeID`);

--
-- 資料表索引 `procurement_items`
--
ALTER TABLE `procurement_items`
  ADD PRIMARY KEY (`ProcurementItemID`),
  ADD KEY `fk_procurement_items_procurement` (`ProcurementID`),
  ADD KEY `fk_procurement_items_material` (`MaterialID`);

--
-- 資料表索引 `product`
--
ALTER TABLE `product`
  ADD PRIMARY KEY (`ProductID`),
  ADD UNIQUE KEY `ProductID` (`ProductID`);

--
-- 資料表索引 `rawmaterialrequest`
--
ALTER TABLE `rawmaterialrequest`
  ADD PRIMARY KEY (`RequestID`),
  ADD KEY `fk_request_employee` (`EmployeeID`);

--
-- 資料表索引 `rawmaterialrequestitem`
--
ALTER TABLE `rawmaterialrequestitem`
  ADD PRIMARY KEY (`RequestItemID`),
  ADD KEY `fk_detail_request` (`RequestID`),
  ADD KEY `fk_detail_material` (`MaterialID`);

--
-- 資料表索引 `rawmaterials`
--
ALTER TABLE `rawmaterials`
  ADD PRIMARY KEY (`MaterialID`),
  ADD UNIQUE KEY `MaterialID` (`MaterialID`);

--
-- 資料表索引 `replyslip`
--
ALTER TABLE `replyslip`
  ADD PRIMARY KEY (`ReplyID`),
  ADD KEY `fk_reply_delivery` (`DeliveryID`),
  ADD KEY `fk_reply_order` (`OrderID`);

--
-- 在匯出的資料表使用 AUTO_INCREMENT
--

--
-- 使用資料表 AUTO_INCREMENT `afterservicerequest`
--
ALTER TABLE `afterservicerequest`
  MODIFY `RequestID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- 使用資料表 AUTO_INCREMENT `order_items`
--
ALTER TABLE `order_items`
  MODIFY `OrderItemID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=54;

--
-- 使用資料表 AUTO_INCREMENT `procurement_items`
--
ALTER TABLE `procurement_items`
  MODIFY `ProcurementItemID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- 使用資料表 AUTO_INCREMENT `rawmaterialrequestitem`
--
ALTER TABLE `rawmaterialrequestitem`
  MODIFY `RequestItemID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=14;

--
-- 已匯出資料表的限制(Constraint)
--

--
-- 資料表的 Constraints `afterservicerequest`
--
ALTER TABLE `afterservicerequest`
  ADD CONSTRAINT `fk_order_af` FOREIGN KEY (`OrderID`) REFERENCES `orders` (`OrderID`);

--
-- 資料表的 Constraints `delivery`
--
ALTER TABLE `delivery`
  ADD CONSTRAINT `fk_delivery_order` FOREIGN KEY (`OrderID`) REFERENCES `orders` (`OrderID`);

--
-- 資料表的 Constraints `invoice`
--
ALTER TABLE `invoice`
  ADD CONSTRAINT `fk_invoice_order` FOREIGN KEY (`OrderID`) REFERENCES `orders` (`OrderID`);

--
-- 資料表的 Constraints `order_items`
--
ALTER TABLE `order_items`
  ADD CONSTRAINT `fk_order` FOREIGN KEY (`OrderID`) REFERENCES `orders` (`OrderID`),
  ADD CONSTRAINT `fk_product` FOREIGN KEY (`ProductID`) REFERENCES `product` (`ProductID`);

--
-- 資料表的 Constraints `procurement`
--
ALTER TABLE `procurement`
  ADD CONSTRAINT `fk_procurement_employee` FOREIGN KEY (`EmployeeID`) REFERENCES `employee` (`EmployeeID`);

--
-- 資料表的 Constraints `procurement_items`
--
ALTER TABLE `procurement_items`
  ADD CONSTRAINT `fk_procurement_items_material` FOREIGN KEY (`MaterialID`) REFERENCES `rawmaterials` (`MaterialID`),
  ADD CONSTRAINT `fk_procurement_items_procurement` FOREIGN KEY (`ProcurementID`) REFERENCES `procurement` (`ProcurementID`);

--
-- 資料表的 Constraints `rawmaterialrequest`
--
ALTER TABLE `rawmaterialrequest`
  ADD CONSTRAINT `fk_request_employee` FOREIGN KEY (`EmployeeID`) REFERENCES `employee` (`EmployeeID`);

--
-- 資料表的 Constraints `rawmaterialrequestitem`
--
ALTER TABLE `rawmaterialrequestitem`
  ADD CONSTRAINT `fk_detail_material` FOREIGN KEY (`MaterialID`) REFERENCES `rawmaterials` (`MaterialID`),
  ADD CONSTRAINT `fk_detail_request` FOREIGN KEY (`RequestID`) REFERENCES `rawmaterialrequest` (`RequestID`);

--
-- 資料表的 Constraints `replyslip`
--
ALTER TABLE `replyslip`
  ADD CONSTRAINT `fk_reply_delivery` FOREIGN KEY (`DeliveryID`) REFERENCES `delivery` (`DeliveryID`),
  ADD CONSTRAINT `fk_reply_order` FOREIGN KEY (`OrderID`) REFERENCES `orders` (`OrderID`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
