
# SampleWebApi

## GET /api/v1/menuitems


## Payloads

### Request Body

```json

{}

```

### Default Response Body
```json

{
  "Items": {
    "Id": "f9a7b8c4-0c4f-46e1-b4f5-d2fe9e5b35bb",
    "FranchiseId": "f9a7b8c4-0c4f-46e1-b4f5-d2fe9e5b35bb",
    "Name": "Example String",
    "Description": "Example String",
    "NutritionFacts": {
      "Calories": 123,
      "ProteinNutritionFacts": {
        "ProteinGrams": 123,
        "ProteinMilligrams": 123
      }
    }
  },
  "TotalCount": 123
}

```










## Request Body


* N/A - This endpoint does not have a request body


## Responses

* *200 Response*
    * **Title:** 200 Response - 200GetResponse
    * **Name:** 200GetResponse
    * **Description:** OK
     
        * **Properties:**
        
            * *Items*
                * **Description:** [N/A]
                * **Schema Name:** SampleWebApiControllersMenuItemResponseItem[]
                * **Schema:** 
                    * **Schema Description:** A single menu item
 
                * **Reference ID:** SampleWebApi.Controllers.MenuItemResponseItem
                * [Object Details...](../schema/SampleWebApiControllersMenuItemResponseItem.md)
            
        
            * *TotalCount*
                * **Description:** [N/A]
                * **Schema Name:** Int32
                * **Schema:** 
                    * **Schema Description:** The total count of menu items
 
                * **Type:** Int32
                * **Example:** `123`
                * **Is Enum?:** false
                * **Is Array?:** false
            
         
         

* *Default / Fallback Response (for any status code that is not explicitly defined, this response can be assumed)*
    * **Title:** 200 Response - 200GetResponse
    * **Name:** 200GetResponse
    * **Description:** OK
     
        * **Properties:**
        
            * *Items*
                * **Description:** [N/A]
                * **Schema Name:** SampleWebApiControllersMenuItemResponseItem[]
                * **Schema:** 
                    * **Schema Description:** A single menu item
 
                * **Reference ID:** SampleWebApi.Controllers.MenuItemResponseItem
                * [Object Details...](../schema/SampleWebApiControllersMenuItemResponseItem.md)
            
        
            * *TotalCount*
                * **Description:** [N/A]
                * **Schema Name:** Int32
                * **Schema:** 
                    * **Schema Description:** The total count of menu items
 
                * **Type:** Int32
                * **Example:** `123`
                * **Is Enum?:** false
                * **Is Array?:** false
            
         
         


## Extensions
* x-operationExtension = `hello world`





### [< Back to Path](../Paths/Apiv1Menuitems.md)
### [<< Back to API](../SampleWebApi.Readme.md)

*** 

*[Documentation generated with Swagabond](https://github.com/jordanbleu/swagabond)*

