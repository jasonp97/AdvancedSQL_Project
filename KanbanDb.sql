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
	HarnessQty INT NOT NULL,
	ReflectorQty INT NOT NULL,
	HousingQty INT NOT NULL,
	LensQty INT NOT NULL,
	BulbQty INT NOT NULL,
	BezelQty INT NOT NULL,
	LampQty INT NOT NULL
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


----------------------------------------------- Stored Procedure ------------------------------------------------------

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'Workstation_Modifier') AND type in (N'P', N'PC'))
	DROP PROCEDURE Workstation_Modifier
GO
CREATE PROCEDURE Workstation_Modifier
@StationID INT,
@ActionFlag INT

AS
BEGIN 
		DECLARE @Harness INT
		DECLARE @Reflector INT 
		DECLARE @Housing INT 
		DECLARE @Lens INT 
		DECLARE @Bulb INT 
		DECLARE @Bezel INT
		DECLARE @NoOfLamp INT

		IF @ActionFlag = 0 -- Insert new workstation into database 
			BEGIN
				-- Retrieve the default bins' capacities
				SELECT @Harness = HarnessQty FROM Configuration WHERE ConfigID = 1
				SELECT @Reflector = ReflectorQty FROM Configuration WHERE ConfigID = 1
				SELECT @Housing = HousingQty FROM Configuration WHERE ConfigID = 1
				SELECT @Lens = LensQty FROM Configuration WHERE ConfigID = 1
				SELECT @Bulb = BulbQty FROM Configuration WHERE ConfigID = 1
				SELECT @Bezel = BezelQty FROM Configuration WHERE ConfigID = 1

				INSERT INTO WorkStation
				VALUES(@StationID, @Harness, @Reflector, @Housing, @Lens, @Bulb, @Bezel, 0); 
			END
		ELSE -- Update bins' capacities
			BEGIN
				-- Retrieve the current bins' capacities
				SELECT @Harness = HarnessQty FROM WorkStation WHERE WorkStationID = @StationID
				SELECT @Reflector = ReflectorQty FROM WorkStation WHERE WorkStationID = @StationID
				SELECT @Housing = HousingQty FROM WorkStation WHERE WorkStationID = @StationID
				SELECT @Lens = LensQty FROM WorkStation WHERE WorkStationID = @StationID
				SELECT @Bulb = BulbQty FROM WorkStation WHERE WorkStationID = @StationID
				SELECT @Bezel = BezelQty FROM WorkStation WHERE WorkStationID = @StationID
				SELECT @NoOfLamp = LampQty FROM WorkStation WHERE WorkStationID = @StationID

				-- Decrease the bins's capacities by 1 accordingly
				IF @Harness > 0
				BEGIN
					SET @Harness = @Harness - 1
				END

				IF @Reflector > 0
				BEGIN
					SET @Reflector = @Reflector - 1
				END

				IF @Housing > 0
				BEGIN
					SET @Housing = @Housing - 1
				END

				IF @Lens > 0
				BEGIN
					SET @Lens = @Lens - 1
				END

				IF @Bulb > 0
				BEGIN
					SET @Bulb = @Bulb - 1
				END

				IF @Bezel > 0
				BEGIN
					SET @Bezel = @Bezel - 1
				END

				SET @NoOfLamp = @NoOfLamp + 1

				UPDATE WorkStation
				SET HarnessQty = @Harness, ReflectorQty = @Reflector, HousingQty = @Housing, LensQty = @Lens, BulbQty = @Bulb, BezelQty = @Bezel, LampQty = @NoOfLamp
				WHERE WorkStationID = @StationID;
			END
			
END
GO



IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'Materials_Refill') AND type in (N'P', N'PC'))
	DROP PROCEDURE Materials_Refill
GO
CREATE PROCEDURE Materials_Refill
@StationID INT
AS
BEGIN 
		DECLARE @Harness INT
		DECLARE @Reflector INT 
		DECLARE @Housing INT 
		DECLARE @Lens INT 
		DECLARE @Bulb INT 
		DECLARE @Bezel INT
		DECLARE @NoOfLamp INT

		-- Retrieve the current bins' capacities
		SELECT @Harness = HarnessQty FROM WorkStation WHERE WorkStationID = @StationID
		SELECT @Reflector = ReflectorQty FROM WorkStation WHERE WorkStationID = @StationID
		SELECT @Housing = HousingQty FROM WorkStation WHERE WorkStationID = @StationID
		SELECT @Lens = LensQty FROM WorkStation WHERE WorkStationID = @StationID
		SELECT @Bulb = BulbQty FROM WorkStation WHERE WorkStationID = @StationID
		SELECT @Bezel = BezelQty FROM WorkStation WHERE WorkStationID = @StationID
		SELECT @NoOfLamp = LampQty FROM WorkStation WHERE WorkStationID = @StationID

		-- Refill the bins if the capacity falls below 5
		IF @Harness <= 5
			BEGIN
				SELECT @Harness = @Harness + HarnessQty FROM Configuration WHERE ConfigID = 1
			END

		IF @Reflector <= 5
			BEGIN
				SELECT @Reflector = @Reflector + ReflectorQty FROM Configuration WHERE ConfigID = 1
			END

		IF @Housing <= 5
			BEGIN
				SELECT @Housing = @Housing + HousingQty FROM Configuration WHERE ConfigID = 1
			END

		IF @Lens <= 5
			BEGIN
				SELECT @Lens = @Lens + LensQty FROM Configuration WHERE ConfigID = 1
			END

		IF @Bulb <= 5
			BEGIN
				SELECT @Bulb = @Bulb + BulbQty FROM Configuration WHERE ConfigID = 1
			END

		IF @Bezel <= 5
			BEGIN
				SELECT @Bezel = @Bezel + BezelQty FROM Configuration WHERE ConfigID = 1
			END
		
		UPDATE WorkStation
		SET HarnessQty = @Harness, ReflectorQty = @Reflector, HousingQty = @Housing, LensQty = @Lens, BulbQty = @Bulb, BezelQty = @Bezel
		WHERE WorkStationID = @StationID;
	
END
GO


IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'CheckEmptyBin') AND type in (N'P', N'PC'))
	DROP PROCEDURE CheckEmptyBin
GO
CREATE PROCEDURE CheckEmptyBin
@StationID INT,
@EmptyFlag INT OUTPUT
AS
BEGIN 
		DECLARE @Harness INT
		DECLARE @Reflector INT 
		DECLARE @Housing INT 
		DECLARE @Lens INT 
		DECLARE @Bulb INT 
		DECLARE @Bezel INT
		DECLARE @NoOfLamp INT

		SET @EmptyFlag = 0

		-- Retrieve the current bins' capacities
		SELECT @Harness = HarnessQty FROM WorkStation WHERE WorkStationID = @StationID
		SELECT @Reflector = ReflectorQty FROM WorkStation WHERE WorkStationID = @StationID
		SELECT @Housing = HousingQty FROM WorkStation WHERE WorkStationID = @StationID
		SELECT @Lens = LensQty FROM WorkStation WHERE WorkStationID = @StationID
		SELECT @Bulb = BulbQty FROM WorkStation WHERE WorkStationID = @StationID
		SELECT @Bezel = BezelQty FROM WorkStation WHERE WorkStationID = @StationID
		SELECT @NoOfLamp = LampQty FROM WorkStation WHERE WorkStationID = @StationID

		-- Check if any bin is empty.
		IF @Harness <= 0 OR @Reflector <= 0 OR  @Housing <= 0 OR @Lens <= 0 OR  @Bulb <= 0 OR @Bezel <= 0 
		BEGIN
			SET @EmptyFlag = 1
		END
	
END
GO


SELECT * FROM Configuration
SELECT * FROM WorkStation
SELECT * FROM Worker
SELECT * FROM Test_Lamp
SELECT * FROM Test_Tray

EXECUTE Workstation_Modifier 1, 0
EXECUTE Workstation_Modifier 1, 1
EXECUTE Workstation_Modifier 2, 0
EXECUTE Workstation_Modifier 1, 1


INSERT INTO WorkStation
VALUES(3, 5, 4, 3, 2, 1, 0, 7);

UPDATE WorkStation
SET  HousingQty = 1
WHERE WorkStationID = 3;

EXECUTE Materials_Refill 3


INSERT INTO WorkStation
VALUES(4, 0, 4, 3, 2, 1, 7, 7);

DECLARE @TestOutput INT;
EXECUTE CheckEmptyBin 1, @TestOutput OUTPUT;
PRINT @TestOutput


SELECT CONVERT (VARCHAR(50), SYSDATETIME())  
SELECT SYSDATETIME()  


