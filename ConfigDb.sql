/*
* FILE: tlai_sphamhoang_M1.sql
* PROJECT: PROG3070 - Project Milestone 01
* PROGRAMMERS: TRAN PHUOC NGUYEN LAI, SON PHAM HOANG
* FIRST VERSION: 11/13/2020
* DESCRIPTION: This file includes the script for the creation of
*				KanbanDatabase and configuration table
*/ 


-- Check if database does not exist, then creates accordingly.
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'KanbanDatabase')
BEGIN
	CREATE DATABASE KanbanDatabase;
END

USE KanbanDatabase;


DROP TABLE IF EXISTS Configuration;

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
	NoOfSuper int NOT NULL,
);

-- Insert initial data / default settings for configuration table.
INSERT INTO Configuration
VALUES (1,55, 35, 24, 40, 60, 75, 0, 3, 60, 1, 2, 1);


SELECT * FROM Configuration