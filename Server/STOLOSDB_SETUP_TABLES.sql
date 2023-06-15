/* DROP IF EXISTS*/
DROP TABLE IF EXISTS Driver;
DROP TABLE IF EXISTS Vehicle;
DROP TABLE IF EXISTS GasCard;

/* CREATE TABLES */
CREATE TABLE `Driver` (
	`DriverID` INT(255) NOT NULL AUTO_INCREMENT,
	`FirstName` VARCHAR(20) NOT NULL,
	`LastName` VARCHAR(20) NOT NULL,
	`Address` VARCHAR(50),
	`BirthDate` DATE NOT NULL,
	`NationalRegistrationNumber` VARCHAR(15) NOT NULL,
	`DriversLicenses` SET('AM', 'A1', 'A2', 'A', 'B', 'C1', 'C', 'D1', 'D', 'BE', 'C1E', 'CE', 'D1E', 'DE', 'G') NOT NULL,
	`Deleted` INT(1) NOT NULL,
	PRIMARY KEY (`DriverID`)
);
CREATE TABLE `Vehicle` (
	`VIN` VARCHAR(17) NOT NULL,
	`BrandModel` VARCHAR(50) NOT NULL,
	`LicensePlate` VARCHAR(25) NOT NULL,
	`FuelType` ENUM('Unknown', 'Petrol', 'Diesel', 'Electric', 'LPG', 'PetrolHybrid', 'DieselHybrid') NOT NULL,
	`VehicleType` ENUM('Unknown', 'Car', 'Van', 'Truck', 'Bus') NOT NULL,
	`Color` VARCHAR(25),
	`Doors` INT(10),
	`DriverID` INT(255),
	`Deleted` INT(1) NOT NULL,
	PRIMARY KEY (`VIN`)
);
CREATE TABLE `GasCard` (
	`CardNumber` VARCHAR(20) NOT NULL,
	`ExpiringDate` DATE NOT NULL,
	`Pincode` INT(4),
	`FuelTypes` SET('Unknown', 'Petrol', 'Diesel', 'Electric', 'LPG', 'PetrolHybrid', 'DieselHybrid'),
	`DriverID` INT(255),
	`Blocked` INT(1) NOT NULL,
	`Deleted` INT(1) NOT NULL,
	PRIMARY KEY (`CardNumber`)
);

/* ADD CONSTRAINTS*/
/* FK */
ALTER TABLE Vehicle ADD CONSTRAINT fk_vehicle_driverid_driverid FOREIGN KEY (DriverID) REFERENCES Driver(DriverID) ON DELETE SET NULL;
ALTER TABLE GasCard ADD CONSTRAINT fk_gascard_driverid_driverid FOREIGN KEY (DriverID) REFERENCES Driver(DriverID) ON DELETE SET NULL;

/* UQ */
ALTER TABLE Driver ADD CONSTRAINT uc_driver_natregnum UNIQUE (NationalRegistrationNumber);

ALTER TABLE Vehicle ADD CONSTRAINT uc_vehicle_licenseplate UNIQUE (LicensePlate);
ALTER TABLE Vehicle ADD CONSTRAINT uc_vehicle_driverid UNIQUE (DriverID);


ALTER TABLE GasCard ADD CONSTRAINT uc_gascard_cardnum UNIQUE (CardNumber);
ALTER TABLE GasCard ADD CONSTRAINT uc_gascard_driverid UNIQUE (DriverID);

/* CHK */
ALTER TABLE Driver ADD CONSTRAINT chk_natregnum CHECK (((97-MOD(concat(substring(NationalRegistrationNumber, 1, 2), substring(NationalRegistrationNumber, 4, 2), substring(NationalRegistrationNumber, 7, 2), substring(NationalRegistrationNumber, 10, 3)), 97))=substring(NationalRegistrationNumber, 14, 2)) OR ((97-MOD(concat(2, substring(NationalRegistrationNumber, 1, 2), substring(NationalRegistrationNumber, 4, 2), substring(NationalRegistrationNumber, 7, 2), substring(NationalRegistrationNumber, 10, 3)), 97)=substring(NationalRegistrationNumber, 14, 2))));


/* To drop tables: DROP TABLE Driver, Vehicle, GasCard; */
