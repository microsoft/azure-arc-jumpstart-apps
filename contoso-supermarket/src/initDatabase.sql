USE contososupermarket;

CREATE TABLE `Stocks` (
	`ProductID` INT NOT NULL AUTO_INCREMENT,
	`Name` TEXT,
	`Sold` INT DEFAULT '10000',
	`Stock` INT zerofill,
	PRIMARY KEY (`ProductID`)
);

INSERT INTO `Stocks`
VALUES
(1, "Red apple", 10000, 0),
(2, "Banana", 10000, 0),
(3, "Avocado", 10000, 0),
(4, "Bread", 10000, 0),
(5, "Milk", 10000, 0),
(6, "Orange juice", 10000, 0),
(7, "Chips", 10000, 0),
(8, "Red bell pepper", 10000, 0),
(9, "Lettuce", 10000, 0);
