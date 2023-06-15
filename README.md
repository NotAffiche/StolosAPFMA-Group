# Readme voor het fleetmanagement voor AllPhi

## Backend API Requests

### Driver

- /Driver (GET)
  - Geen waarden of object nodig
- /Driver (POST)
  - Firstname
  - Lastname
  - Address
  - BirthDate
  - Nationalregistrationnumber
- /Driver (PUT)
  - Id
  - Firstname
  - Lastname
  - Address
  - BirthDate
  - Nationalregistrationnumber
- /Driver (DELETE)
  - Id
- /Driver/{id}
  - Id

### GasCard

- /GasCard (GET)
  - Geen waarden of object nodig
- /GasCard (POST)
  - Cardnumber
  - ExpiringDate
  - Pincode
  - Blocked
  - DriverId
  - An array of FuelTypes
- /GasCard (PUT)
  - Cardnumber
  - ExpiringDate
  - Pincode
  - Blocked
  - DriverId
  - An array of FuelTypes
- /GasCard (DELETE)
  - Cardnumber
- /GasCard/{cardnum}
  - Cardnumber

### Vehicle

- /Vehicle (GET)
  - Geen waarden of object nodig
- /Vehicle (POST)
  - Vinnumber
  - BrandModel
  - LicensePlate
  - Fueltype
  - VehicleType
  - Color
  - Doors
  - DriverId
- /Vehicle (PUT)
  - Vinnumber
  - BrandModel
  - LicensePlate
  - Fueltype
  - VehicleType
  - Color
  - Doors
  - DriverId
- /Vehicle (DELETE)
  - Vinnumber
- /Vehicle/{vin}
  - Vinnumber


## Server Setup

### Required Packages
- dotnet-sdk-7.0
- mysql-server-8.0

### Setup Server

#### MySql Setup
mysql> CREATE DATABASE db_allphifm;
mysql> CREATE USER '{username}' IDENTIFIED BY '{password}';
mysql> GRANT ALL PRIVILEGES ON db_allphifm.* TO '{username}';
mysql> FLUSH PRIVILEGES;

#### API Setup
Run Api InDev:
- Clone repo into desired folder.
- Adjust appsettings.json (connectionstring) to match MySql Setup.
- CD to API.
stolos@server:~$ dotnet build
stolos@server:~$ dotnet run

If you want to keep the InDev running on ssh close; use tmux
stolos@server:~$ tmux new -s {tmuxsessionname}
stolos@server:~$ dotnet run
CTRL+B, D: detach from tmux session
to reattach:
stolos@server:~$ tmux a -t {tmuxsessionname}