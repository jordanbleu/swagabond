
# SampleWebApi

## GET /api/v1/restaurants/{id}


## Payloads

### Request Body

```json

{}

```

### Default Response Body
```json

{
  "Id": "f9a7b8c4-0c4f-46e1-b4f5-d2fe9e5b35bb",
  "FranchiseId": "f9a7b8c4-0c4f-46e1-b4f5-d2fe9e5b35bb",
  "StoreNumber": 123,
  "Address": "Example String",
  "Zip": "Example String",
  "City": "Example String",
  "State": "1 (Alabama)"
}

```



## Path Parameters

* {Id} 
    * **Schema Name:** Guid 
    * **Schema Description:**  A globally unique identifier, often used for unique keys or identifiers in systems.
    * **Type:** Guid
    * **Example:** `f9a7b8c4-0c4f-46e1-b4f5-d2fe9e5b35bb`
    * **Is Enum?:** false
    * **Is Array?:** false









## Request Body


* N/A - This endpoint does not have a request body


## Responses

* *200 Response*
    * **Title:** 200 Response - 200GetResponse
    * **Name:** 200GetResponse
    * **Description:** OK
     
        * **Properties:**
        
            * *Id*
                * **Description:** [N/A]
                * **Schema Name:** Guid
                * **Schema:** 
                    * **Schema Description:** The unique identifier for the restaurant.
 
                * **Type:** Guid
                * **Example:** `f9a7b8c4-0c4f-46e1-b4f5-d2fe9e5b35bb`
                * **Is Enum?:** false
                * **Is Array?:** false
            
        
            * *FranchiseId*
                * **Description:** [N/A]
                * **Schema Name:** Guid
                * **Schema:** 
                    * **Schema Description:** The Franchise ID for the restaurant.
 
                * **Type:** Guid
                * **Example:** `f9a7b8c4-0c4f-46e1-b4f5-d2fe9e5b35bb`
                * **Is Enum?:** false
                * **Is Array?:** false
            
        
            * *StoreNumber*
                * **Description:** [N/A]
                * **Schema Name:** Int32
                * **Schema:** 
                    * **Schema Description:** Unique number franchises use to identify the restaurant.
 
                * **Type:** Int32
                * **Example:** `123`
                * **Is Enum?:** false
                * **Is Array?:** false
            
        
            * *Address*
                * **Description:** [N/A]
                * **Schema Name:** String
                * **Schema:** 
                    * **Schema Description:** The physical address of the restaurant
 
                * **Type:** String
                * **Example:** `Example String`
                * **Is Enum?:** false
                * **Is Array?:** false
            
        
            * *Zip*
                * **Description:** [N/A]
                * **Schema Name:** String
                * **Schema:** 
                    * **Schema Description:** The 5 digit postal code of the restaurant.
 
                * **Type:** String
                * **Example:** `Example String`
                * **Is Enum?:** false
                * **Is Array?:** false
            
        
            * *City*
                * **Description:** [N/A]
                * **Schema Name:** String
                * **Schema:** 
                    * **Schema Description:** The city the restaurant is located in.
 
                * **Type:** String
                * **Example:** `Example String`
                * **Is Enum?:** false
                * **Is Array?:** false
            
        
            * *State*
                * **Description:** [N/A]
                * **Schema Name:** SampleWebApiEntitiesState
                * **Schema:** 
                    * **Schema Description:** [N/A]
 
                * **Type:** Int32
                * **Example:** `1 (Alabama)`
                * **Is Enum?:** true
                * **Enum Options:**
                    * Alabama = 1
                    * Alaska = 2
                    * Arizona = 3
                    * Arkansas = 4
                    * California = 5
                    * Colorado = 6
                    * Connecticut = 7
                    * Delaware = 8
                    * Florida = 9
                    * Georgia = 10
                    * Hawaii = 11
                    * Idaho = 12
                    * Illinois = 13
                    * Indiana = 14
                    * Iowa = 15
                    * Kansas = 16
                    * Kentucky = 17
                    * Louisiana = 18
                    * Maine = 19
                    * Maryland = 20
                    * Massachusetts = 21
                    * Michigan = 22
                    * Minnesota = 23
                    * Mississippi = 24
                    * Missouri = 25
                    * Montana = 26
                    * Nebraska = 27
                    * Nevada = 28
                    * NewHampshire = 29
                    * NewJersey = 30
                    * NewMexico = 31
                    * NewYork = 32
                    * NorthCarolina = 33
                    * NorthDakota = 34
                    * Ohio = 35
                    * Oklahoma = 36
                    * Oregon = 37
                    * Pennsylvania = 38
                    * RhodeIsland = 39
                    * SouthCarolina = 40
                    * SouthDakota = 41
                    * Tennessee = 42
                    * Texas = 43
                    * Utah = 44
                    * Vermont = 45
                    * Virginia = 46
                    * Washington = 47
                    * WestVirginia = 48
                    * Wisconsin = 49
                    * Wyoming = 50
                    * AmericanSamoa = 101
                    * Guam = 102
                    * NorthernMarianaIslands = 103
                    * PuertoRico = 104
                    * USMinorOutlyingIslands = 105
                    * USVirginIslands = 106
                    * DistrictOfColumbia = 107

                * **Is Array?:** false
            
         
         

* *Default / Fallback Response (for any status code that is not explicitly defined, this response can be assumed)*
    * **Title:** 200 Response - 200GetResponse
    * **Name:** 200GetResponse
    * **Description:** OK
     
        * **Properties:**
        
            * *Id*
                * **Description:** [N/A]
                * **Schema Name:** Guid
                * **Schema:** 
                    * **Schema Description:** The unique identifier for the restaurant.
 
                * **Type:** Guid
                * **Example:** `f9a7b8c4-0c4f-46e1-b4f5-d2fe9e5b35bb`
                * **Is Enum?:** false
                * **Is Array?:** false
            
        
            * *FranchiseId*
                * **Description:** [N/A]
                * **Schema Name:** Guid
                * **Schema:** 
                    * **Schema Description:** The Franchise ID for the restaurant.
 
                * **Type:** Guid
                * **Example:** `f9a7b8c4-0c4f-46e1-b4f5-d2fe9e5b35bb`
                * **Is Enum?:** false
                * **Is Array?:** false
            
        
            * *StoreNumber*
                * **Description:** [N/A]
                * **Schema Name:** Int32
                * **Schema:** 
                    * **Schema Description:** Unique number franchises use to identify the restaurant.
 
                * **Type:** Int32
                * **Example:** `123`
                * **Is Enum?:** false
                * **Is Array?:** false
            
        
            * *Address*
                * **Description:** [N/A]
                * **Schema Name:** String
                * **Schema:** 
                    * **Schema Description:** The physical address of the restaurant
 
                * **Type:** String
                * **Example:** `Example String`
                * **Is Enum?:** false
                * **Is Array?:** false
            
        
            * *Zip*
                * **Description:** [N/A]
                * **Schema Name:** String
                * **Schema:** 
                    * **Schema Description:** The 5 digit postal code of the restaurant.
 
                * **Type:** String
                * **Example:** `Example String`
                * **Is Enum?:** false
                * **Is Array?:** false
            
        
            * *City*
                * **Description:** [N/A]
                * **Schema Name:** String
                * **Schema:** 
                    * **Schema Description:** The city the restaurant is located in.
 
                * **Type:** String
                * **Example:** `Example String`
                * **Is Enum?:** false
                * **Is Array?:** false
            
        
            * *State*
                * **Description:** [N/A]
                * **Schema Name:** SampleWebApiEntitiesState
                * **Schema:** 
                    * **Schema Description:** [N/A]
 
                * **Type:** Int32
                * **Example:** `1 (Alabama)`
                * **Is Enum?:** true
                * **Enum Options:**
                    * Alabama = 1
                    * Alaska = 2
                    * Arizona = 3
                    * Arkansas = 4
                    * California = 5
                    * Colorado = 6
                    * Connecticut = 7
                    * Delaware = 8
                    * Florida = 9
                    * Georgia = 10
                    * Hawaii = 11
                    * Idaho = 12
                    * Illinois = 13
                    * Indiana = 14
                    * Iowa = 15
                    * Kansas = 16
                    * Kentucky = 17
                    * Louisiana = 18
                    * Maine = 19
                    * Maryland = 20
                    * Massachusetts = 21
                    * Michigan = 22
                    * Minnesota = 23
                    * Mississippi = 24
                    * Missouri = 25
                    * Montana = 26
                    * Nebraska = 27
                    * Nevada = 28
                    * NewHampshire = 29
                    * NewJersey = 30
                    * NewMexico = 31
                    * NewYork = 32
                    * NorthCarolina = 33
                    * NorthDakota = 34
                    * Ohio = 35
                    * Oklahoma = 36
                    * Oregon = 37
                    * Pennsylvania = 38
                    * RhodeIsland = 39
                    * SouthCarolina = 40
                    * SouthDakota = 41
                    * Tennessee = 42
                    * Texas = 43
                    * Utah = 44
                    * Vermont = 45
                    * Virginia = 46
                    * Washington = 47
                    * WestVirginia = 48
                    * Wisconsin = 49
                    * Wyoming = 50
                    * AmericanSamoa = 101
                    * Guam = 102
                    * NorthernMarianaIslands = 103
                    * PuertoRico = 104
                    * USMinorOutlyingIslands = 105
                    * USVirginIslands = 106
                    * DistrictOfColumbia = 107

                * **Is Array?:** false
            
         
         


## Extensions
* x-operationExtension = `hello world`





### [< Back to Path](../Paths/Apiv1Restaurantsid.md)
### [<< Back to API](../SampleWebApi.Readme.md)

*** 

*[Documentation generated with Swagabond](https://github.com/jordanbleu/swagabond)*

