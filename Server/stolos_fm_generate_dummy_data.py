import random
import datetime
import math
import string

AMOUNT = 60
PERCENTAGE = int(math.floor(AMOUNT/4))

def random_date(start, end):
  return start + datetime.timedelta(
      seconds=random.randint(0, int((end - start).total_seconds())))

file = open("STOLOSDB_INSERT_DUMMYDATA.sql", "w")

#start loop DRIVERS
# INSERT INTO Driver(FirstName, LastName, Address, BirthDate, NationalRegistrationNumber, DriversLicenses, Deleted) VALUES ('John', 'Doe', '123 Lexington Av., NYC, NY, USA', '1985-12-31', '85.12.31-123.40', 'AM,B,C1E', 0);
DRIVING_LICENSES = ["AM", "A1", "A2", "A", "B", "C1", "C", "D1", "D", "BE", "C1E", "CE", "D1E", "DE", "G"]

ALL_GIRLS_FIRST = ["Emma", "Louise", "Alice", "Lucie", "Olivia", "Camille", "Giulia", "Mila", "Mia", "Sofia", "Victoria", "Juliette", "Jade", "Eva", "Lina", "Rose", "Julia", "Elise", "Elena", "Anna", "Nina", "Charlotte", "Manon", "Margaux", "Lena", "Adele", "Lou", "Clara", "Jeanne", "Ambre", "Luna", "Lily", "Mya", "Lola", "Sarah", "Marion", "Livia", "Emy", "Romane", "Alix", "Amelia", "Eleonore", "Inaya", "Nora", "Mila", "Olivia", "Marie", "Ella", "Noor", "Elena", "Juliette", "Anna", "Nora", "Elise", "Liv", "Lena", "Alice", "Julie", "Lina", "Lotte", "Camille", "Charlotte", "Nina", "Ellie", "Mona", "Sofia", "Amelie", "Manon", "Sara", "Amber", "Fien", "Mia", "Billie", "Helena", "Oona", "Lucie", "Luna", "Janne", "Lou", "Axelle", "Ellis", "Nore", "Fleur", "Emilia", "Rosalie", "Laura", "Amira", "Julia", "Lara", "Kato", "Lore", "Eva", "Sophia", "Stella"]
ALL_GUYS_FIRST = ["Gabriel", "Louis", "Hugo", "Jules", "Arthur", "Liam", "Victor", "Noah", "Tom", "Nathan", "Lucas", "Eden", "Adam", "Ethan", "Sacha", "Diego", "Baptiste", "William", "Robin", "Alexandre", "Eliott", "Martin", "Aaron", "Antoine", "Maxime", "Oscar", "Basile", "Augustin", "Enzo", "Tiago", "Milo", "Valentin", "Mathis", "Samuel", "Gaspard", "Yanis", "Alexis", "Achille", "Marius", "Maxence", "Romain", "Alessio", "Luca", "Lyam", "Mathys", "Simon", "Adrien", "Charly", "Mohamed", "Rayan", "Ezio", "Arthur", "Noah", "Lucas", "Liam", "Leon", "Jules", "Finn", "Louis", "Victor", "Lewis", "Adam", "Lars", "Vic", "Vince", "Matteo", "Mathis", "Mohamed", "Emiel", "Jack", "Elias", "Alexander", "Gust", "Oscar", "Stan", "Maurice", "Lou", "Lowie", "Marcel", "Milan", "Mats", "Felix", "Wout", "Kobe", "Nathan", "Tuur", "Warre", "Viktor", "Seppe", "Lukas", "Rayan", "Cas", "Eden", "Ferre", "David", "Daan", "Amir", "Otis", "Thomas", "Miel", "Luca", "Bas", "Mauro", "Oliver", "Emile", "Georges", "Jayden", "Sem", "Cyriel", "Lenn", "Maxim", "Mil", "Henri", "Siebe", "Jef", "Lex", "Sam", "Juul", "Senne", "Loïc", "Alex", "Remi", "Levi", "Max", "Kamiel", "Ali", "Milo", "Youssef"]
ALL_LASTNAMES = ["Peeters", "Maes", "Claes", "Goossens", "De Smet", "Vermeulen", "Lambert", "Michiels", "Martens", "Smets", "Van de Velde", "Hendrickx", "Van Damme", "Janssen", "Dumont", "De backer", "Lemmens", "Van den Broeck", "Laurent", "Renard", "Verhoeven", "Cools", "De cock", "Petit", "Lemaire", "emmerman", "De meyer", "Vandenberghe", "De wilde", "Bauwens", "Lefebvre", "Mathieu", "Boghaert", "Geerts", "Bosmans", "Bernard", "Moens", "Baert", "Carlier", "Van den Bossche", "Verheyen", "De pauw", "Cornelis", "De Ridder", "Thys", "Michel", "De smedt", "Smet", "Charlier", "Declerq", "Le Clercq", "Lejeune", "Denis", "Wauters", "Coppens", "Leroy", "Segers", "Stevens", "De vos", "Dupoint", "Aerts", "Hermans", "Pauwels", "Dubois", "Wouters", "Willems", "Jacobs", "Janssens"]
ADDRESSES = ["NULL", "WVL", "OVL", "ANT", "LIM", "VBR", "BRU", "WBR", "HAI", "NAM", "LIE", "LUX"]
for x in range(1, AMOUNT+1):
  
  isMan = random.randint(0, 1)

  rrnDayCounter = 0
  rrnControlNum = 0
  rrnAllButControl = 0

  rdat = random_date(datetime.datetime(1968, 1, 1), datetime.datetime(datetime.datetime.now().year, datetime.datetime.now().month, datetime.datetime.now().day))
  year = rdat.strftime("%Y")
  month = rdat.strftime("%m")
  day = rdat.strftime("%d")

  if isMan == 1:
    fname = random.choice(ALL_GUYS_FIRST)
    rrnDayCounter = random.randint(0, 499)*2+1
    ALL_GUYS_FIRST.remove(fname)
  else:
    fname = random.choice(ALL_GIRLS_FIRST)
    rrnDayCounter = random.randint(0, 499)*2
    ALL_GIRLS_FIRST.remove(fname)

  rrnDayCounter = str(rrnDayCounter).zfill(3)
  lname = random.choice(ALL_LASTNAMES)
  ALL_LASTNAMES.remove(lname)

  address = random.choice(ADDRESSES)

  birthdate = datetime.date(int(year), int(month), int(day))

  rrnAllButControl = str(year)[2:]+str(month)+str(day)+str(rrnDayCounter)
  rrnAllButControl = int(rrnAllButControl)
  if int(year) >= 2000:
    rrnAllButControl = 2000000000 + rrnAllButControl

  rrnControlNum = 97 - (rrnAllButControl % 97)
  rrnControlNum = str(rrnControlNum).zfill(2)

  rrn = str(year)[2:]+"."+str(month)+"."+str(day)+"-"+str(rrnDayCounter)+"."+str(rrnControlNum)

  licenses = [DRIVING_LICENSES[i] for i in sorted(random.sample(range(len(DRIVING_LICENSES)), random.randint(1, 3)))]
  
  seperator = ","
  if address == "NULL":
    sqlinsert = f"INSERT INTO Driver(FirstName, LastName, BirthDate, NationalRegistrationNumber, DriversLicenses, Deleted) VALUES ('{fname}', '{lname}', '{birthdate}', '{rrn}', '{seperator.join(licenses)}', 0);\n"
  else:
    sqlinsert = f"INSERT INTO Driver(FirstName, LastName, Address, BirthDate, NationalRegistrationNumber, DriversLicenses, Deleted) VALUES ('{fname}', '{lname}', '{address}', '{birthdate}', '{rrn}', '{seperator.join(licenses)}', 0);\n"
  print(f"{rrn} {fname} {lname}")
  file.write(sqlinsert)

#end loop

file.write("\n\n\n")
print("\n\n\n")

#start loop VEHICLES
#INSERT INTO Vehicle (VIN, BrandModel, LicensePlate, FuelType, VehicleType, Color, Doors, DriverID, Deleted) VALUES ('01234567890000000', 'VW Polo', '1-ABC-123', 1, 1, 'black', 3, NULL, 0);
#fuels: 0=unknown, 1=Bezine, 2=Diesel, 3=Electric, 4=LPG, 5=BenzineHybrid, 6=DieselHybrid
BRAND_MODELS = {"VW Polo":"Petrol", "VW Golf":"Petrol", "VW Jetta":"Petrol", "Renault Kangoo":"Diesel", "Kia EV6":"Electric", "Tesla Model 3":"Electric", "Audi A4":"Petrol", "Mercedes EQA":"Electric"}
COLORS = ["black", "red", "gray", "blue", "white"]
existingDrivers = []
for x in range(1, AMOUNT+1):
  
  vin = "".join(random.choices(string.ascii_uppercase + string.digits, k=17))
  bm, f = random.choice(list(BRAND_MODELS.items()))
  brandModel = bm
  licensePlate = str(str(random.randint(1, 3)) + "-" + str("".join(random.choices(string.ascii_uppercase, k=3))) + "-" + str(random.randint(0, 999)).zfill(3)) #"1-ABC-123"
  fueltype = f
  vehicleType = "Car"
  color = str(random.choice(COLORS))
  doors = random.randint(0, 2)*2+1
  if doors == 0 or doors == 1:
    doors = "NULL"

  randId = random.randint(0, AMOUNT-1)
  while (randId in existingDrivers):
    randId = random.randint(0, AMOUNT-1)
  if randId != 0:
    existingDrivers.append(randId)
  driverId = randId
  if driverId == 0:
    driverId = "NULL"

  
  sqlinsert = f"INSERT INTO Vehicle (VIN, BrandModel, LicensePlate, FuelType, VehicleType, Color, Doors, DriverID, Deleted) VALUES ('{vin}', '{brandModel}', '{licensePlate}', '{fueltype}', '{vehicleType}', '{color}', {doors}, {driverId}, 0);\n"
  print(f"{vin} {brandModel}")
  file.write(sqlinsert)

#end loop

file.write("\n\n\n")
print("\n\n\n")

#start loop GASCARDS
#INSERT INTO GasCard (CardNumber, ExpiringDate, Pincode, FuelTypes, DriverID, Blocked, Deleted) VALUES ('12345678900000000000', '2023-12-31', NULL, NULL, NULL, 0, 0);
#fuels: 0=unknown, 1=Bezine, 2=Diesel, 3=Electric, 4=LPG, 5=BenzineHybrid, 6=DieselHybrid
#FUELS = ["Unknown", "Petrol", "Diesel", "Electric", "LPG", "PetrolHybrid", "DieselHybrid"]
FUELS = ["Petrol", "Diesel", "Electric"]
existingDrivers = []
for x in range(1, AMOUNT+1):
  
  cardnum = "".join(random.choices(string.digits, k=20))
  expiringDate = random_date(datetime.datetime(datetime.datetime.now().year, datetime.datetime.now().month, datetime.datetime.now().day), datetime.datetime(2030, 12, 31))
  pincode = random.randint(0, 9999)
  if pincode == 0:
    pincode = "NULL"
  pincode = str(pincode).zfill(4)
  fueltypes = [FUELS[i] for i in sorted(random.sample(range(len(FUELS)), random.randint(1, len(FUELS))))]
  if len(fueltypes) == 0:
    fueltypes = "NULL"

  randId = random.randint(0, AMOUNT-1)
  while (randId in existingDrivers):
    randId = random.randint(0, AMOUNT-1)
  if randId != 0:
    existingDrivers.append(randId)
  driverId = randId
  if driverId == 0:
    driverId = "NULL"
  
  blocked = random.randint(0, 1)

  seperator = ","
  if fueltypes != "NULL":
    fueltypes = seperator.join(fueltypes)

  sqlinsert = f"INSERT INTO GasCard (CardNumber, ExpiringDate, Pincode, FuelTypes, DriverID, Blocked, Deleted) VALUES ('{cardnum}', '{expiringDate.strftime('%Y-%m-%d')}', {pincode}, '{fueltypes}', {driverId}, {blocked}, 0);\n"
  print(f"{cardnum}")
  file.write(sqlinsert)

#end loop

file.close()