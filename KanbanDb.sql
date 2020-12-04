/*
* FILE: KanbanDb.sql
* PROJECT: PROG3070 - Project Milestone 02
* PROGRAMMERS: TRAN PHUOC NGUYEN LAI, SON PHAM HOANG
* FIRST VERSION: 12/03/2020
* DESCRIPTION: This file includes the sql script for the creation of
*				KanbanDatabase.
*/

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'KanbanDatabase')
BEGIN
	CREATE DATABASE KanbanDatabase;
END

USE KanbanDatabase;


DROP TABLE IF EXISTS Configuration;
DROP TABLE IF EXISTS Test_Lamp;
DROP TABLE IF EXISTS Test_Tray;
DROP TABLE IF EXISTS Worker;
DROP TABLE IF EXISTS WorkStation;


CREATE TABLE Configuration (
	ConfigID int NOT NULL PRIMARY KEY,
    HarnessQty int NOT NULL,
	ReflectorQty int NOT NULL,
	HousingQty int NOT NULL,
	LensQty int NOT NULL,
	BulbQty int NOT NULL,
	BezelQty int NOT NULL,
	TimeScale int NOT NULL,
	AssemblyStationQty int NOT NULL,
	TestTrayQty int NOT NULL,
	NoOfRookie int NOT NULL,
	NoOfExperienced int NOT NULL,
	NoOfSuper int NOT NULL
);

-- 1
CREATE TABLE WorkStation (
	WorkStationID INT NOT NULL PRIMARY KEY,
	NoOfWorker INT 
);

-- 2
CREATE TABLE Worker (
	WorkerID VARCHAR(25) NOT NULL PRIMARY KEY,
	WorkStationID INT NOT NULL,
	WorkerExp VARCHAR(25),
	FOREIGN KEY (WorkStationID) REFERENCES WorkStation(WorkStationID)
);

-- 3
CREATE TABLE Test_Tray (
	TestTrayID VARCHAR(50) NOT NULL PRIMARY KEY,
	TestUnitNo VARCHAR(50)
);

-- 4
CREATE TABLE Test_Lamp (
	LampID int IDENTITY(1,1) PRIMARY KEY,
	LampNumber VARCHAR(50) NOT NULL,
	Workstation INT NOT NULL,
	TestTrayID VARCHAR(50) NOT NULL,
	WorkerID VARCHAR(25) NOT NULL,
	CompletedStatus VARCHAR(25),
	FOREIGN KEY (Workstation) REFERENCES WorkStation(WorkStationID),
	FOREIGN KEY (TestTrayID) REFERENCES Test_Tray(TestTrayID),
	FOREIGN KEY (WorkerID) REFERENCES Worker(WorkerID),
);

INSERT INTO Configuration
VALUES (1, 55, 35, 24, 40, 60, 75, 0, 3, 60, 1, 2, 1);


SELECT * FROM Configuration
SELECT * FROM WorkStation
SELECT * FROM Worker
SELECT * FROM Test_Lamp
SELECT * FROM Test_Tray