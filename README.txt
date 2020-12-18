Instruction on how to set up the simulation for electronic Kanban solution

* For Kanban Database
  1. Run the KanbanDb.sql script to create the system database

* For Configuration Tool:
  1. Fill in all the configuration fields.
	e.g: Sample data:
		Harness Qty: 55
		Reflector Qty: 35
		Housing Qty: 24
		Lens Qty: 40
		Bulb Qty: 60
		Bezel Qty: 75
		New Workers: 1
		Experienced Workers: 2
		Super Experienced Workers: 1
		Assembly Station Qty: 3
		Test Tray Qty: 60
		Order Qty: 1000
		Time Scale: 1:1 (1s of real time = 1s of simulated time)
			    1:5 (1s of real time = 5s of simulated time)
  2. Click Save button to save config to database, Cancel button to cancel the action

* For Workstation Simulation and Andon Display programs
  1. Run the Workstation Simulation
  2. Specify workstation ID (e.g: 1, 2, 3)
  3. Turn on Andon Display program
  4. Within Andon, specify workstation IP address and workstation ID we want to connect to
  5. Within Andon, click Connection button to connect to the specified simulated Workstation
  6. Within Workstation Simulation, press Enter to start simulating the workstation

* For Kanban Display program
  1. Run the Kanban Display program
  2. The Kanban program will display active simulated-workstations

NOTE: We can run multiple Workstation simulation and Andon Display instances, but the number of
intances must match between the two programs.