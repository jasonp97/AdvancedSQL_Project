IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'KanbanDatabase')
BEGIN
	CREATE DATABASE KanbanDatabase;
END

USE KanbanDatabase;


DROP TABLE IF EXISTS Configuration;
DROP TABLE IF EXISTS WorkStation;
DROP TABLE IF EXISTS Worker;
DROP TABLE IF EXISTS Test_Tray;
DROP TABLE IF EXISTS Test_Lamp;

CREATE TABLE Configuration (
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

CREATE TABLE WorkStation (
	WorkStationID INT NOT NULL PRIMARY KEY,
	NoOfWorker INT 
);

CREATE TABLE Worker (
	WorkerID INT NOT NULL PRIMARY KEY,
	WorkStationID INT NOT NULL,
	WorkerExp VARCHAR(25),
	FOREIGN KEY (WorkStationID) REFERENCES WorkStation(WorkStationID)
);

CREATE TABLE Test_Tray (
	TestUnitNo VARCHAR(50) NOT NULL PRIMARY KEY,
);

CREATE TABLE Test_Lamp (
	LampID VARCHAR(50) NOT NULL PRIMARY KEY,
	TestUnitNo VARCHAR(50) NOT NULL,
	WorkerID INT NOT NULL,
	CompletedStatus VARCHAR(25),
	FOREIGN KEY (TestUnitNo) REFERENCES Test_Tray(TestUnitNo),
	FOREIGN KEY (WorkerID) REFERENCES Worker(WorkerID),
);


INSERT INTO Configuration
VALUES (55, 35, 24, 40, 60, 75, 0, 3, 60, 1, 2, 1);

SELECT * FROM Configuration
SELECT * FROM WorkStation
SELECT * FROM Worker
SELECT * FROM Test_Lamp
SELECT * FROM Test_Tray