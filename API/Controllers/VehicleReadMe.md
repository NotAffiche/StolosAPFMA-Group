# ReadMe file voor Vehicle API service
## JSON-object met de minimum ingevulde parameters voor een Vehicle toe te voegen

    {  
      "vinNumber": "string",  
      "licensePlate": "string",  
      "brandModel": "string",  
      "category": 0,  
      "fuel": 0,  
      "color": "string",  
      "doors": 0,  
    }

## Volledig JSON-object dat meegegeven kan worden

    {
      "vinNumber": "string",
      "licensePlate": "string",
      "brandModel": "string",
      "category": 0,
      "fuel": 0,
      "color": "string",
      "doors": 0,
      // driver is optioneel
      "driver": {
        "id": 0,
        "lastName": "string",
        "firstName": "string",
        "address": "string",
        "birthDate": "2023-03-24T15:13:23.261Z",
        "natRegNumber": "string",
        "licenses": [0],
        // GasCard is optioneel
        "gasCard": {
          "cardNumber": "string",
          "expiringDate": "2023-03-24T15:13:23.261Z",
          "pincode": 0,
          "blocked": true,
          "fuel": [0],
        }
      }
    }