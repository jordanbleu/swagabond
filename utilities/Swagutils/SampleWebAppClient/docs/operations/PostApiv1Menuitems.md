
# SampleWebApi

## POST /api/v1/menuitems


## Payloads

### Request Body

```json

{
  "FranchiseId": "f9a7b8c4-0c4f-46e1-b4f5-d2fe9e5b35bb",
  "Name": "Example String",
  "Description": "Example String",
  "Calories": 123,
  "ProteinGrams": 123
}

```

### Default Response Body
```json

{
  "Id": "f9a7b8c4-0c4f-46e1-b4f5-d2fe9e5b35bb",
  "FranchiseId": "f9a7b8c4-0c4f-46e1-b4f5-d2fe9e5b35bb",
  "Name": "Example String",
  "Description": "Example String",
  "NutritionFacts": {
    "Calories": 123,
    "ProteinNutritionFacts": {
      "ProteinGrams": 123,
      "ProteinMilligrams": 123,
      "SugarGrams": 123,
      "SugarMilligrams": 123
    }
  }
}

```










## Request Body


* **Title:** POST /api/v1/menuitems RequestBody
* **Name:** Apiv1MenuitemsPostRequest
* **Description:** A post request for menu items
 
* **Properties:**

    * *FranchiseId*
        * **Description:** [N/A]
        * **Schema Name:** Guid
        * **Schema:** 
            * **Schema Description:** The franchise ID this menu item is offered by
 
            * **Type:** Guid
            * **Example:** `f9a7b8c4-0c4f-46e1-b4f5-d2fe9e5b35bb`
            * **Is Enum?:** false
            * **Is Array?:** false
        

    * *Name*
        * **Description:** [N/A]
        * **Schema Name:** String
        * **Schema:** 
            * **Schema Description:** The name of the menu item
 
            * **Type:** String
            * **Example:** `Example String`
            * **Is Enum?:** false
            * **Is Array?:** false
        

    * *Description*
        * **Description:** [N/A]
        * **Schema Name:** String
        * **Schema:** 
            * **Schema Description:** The menu item's description in english
 
            * **Type:** String
            * **Example:** `Example String`
            * **Is Enum?:** false
            * **Is Array?:** false
        

    * *Calories*
        * **Description:** [N/A]
        * **Schema Name:** Int32
        * **Schema:** 
            * **Schema Description:** How many calories is this menu item?
 
            * **Type:** Int32
            * **Example:** `123`
            * **Is Enum?:** false
            * **Is Array?:** false
        

    * *ProteinGrams*
        * **Description:** [N/A]
        * **Schema Name:** Int32
        * **Schema:** 
            * **Schema Description:** How many grams of protein does the item contain?
 
            * **Type:** Int32
            * **Example:** `123`
            * **Is Enum?:** false
            * **Is Array?:** false
        
 
 


## Responses

* *200 Response*
    * **Title:** 200 Response - Apiv1Menuitems200PostResponse
    * **Name:** Apiv1Menuitems200PostResponse
    * **Description:** OK
     
        * **Properties:**
        
            * *Id*
                * **Description:** [N/A]
                * **Schema Name:** Guid
                * **Schema:** 
                    * **Schema Description:** The menu item's ID
 
                * **Type:** Guid
                * **Example:** `f9a7b8c4-0c4f-46e1-b4f5-d2fe9e5b35bb`
                * **Is Enum?:** false
                * **Is Array?:** false
            
        
            * *FranchiseId*
                * **Description:** [N/A]
                * **Schema Name:** Guid
                * **Schema:** 
                    * **Schema Description:** The franchises unique ID
 
                * **Type:** Guid
                * **Example:** `f9a7b8c4-0c4f-46e1-b4f5-d2fe9e5b35bb`
                * **Is Enum?:** false
                * **Is Array?:** false
            
        
            * *Name*
                * **Description:** [N/A]
                * **Schema Name:** String
                * **Schema:** 
                    * **Schema Description:** The menu item name
 
                * **Type:** String
                * **Example:** `Example String`
                * **Is Enum?:** false
                * **Is Array?:** false
            
        
            * *Description*
                * **Description:** [N/A]
                * **Schema Name:** String
                * **Schema:** 
                    * **Schema Description:** The menu item description (english)
 
                * **Type:** String
                * **Example:** `Example String`
                * **Is Enum?:** false
                * **Is Array?:** false
            
        
            * *NutritionFacts*
                * **Description:** [N/A]
                * **Schema Name:** SampleWebApiControllersMenutItemNutritionFacts
                * **Schema:** 
                    * **Schema Description:** A set of menu nutrition facts
 
                * **Reference ID:** SampleWebApi.Controllers.MenutItemNutritionFacts
                * [Object Details...](../schema/SampleWebApiControllersMenutItemNutritionFacts.md)
            
         
         

* *Default / Fallback Response (for any status code that is not explicitly defined, this response can be assumed)*
    * **Title:** 200 Response - Apiv1Menuitems200PostResponse
    * **Name:** Apiv1Menuitems200PostResponse
    * **Description:** OK
     
        * **Properties:**
        
            * *Id*
                * **Description:** [N/A]
                * **Schema Name:** Guid
                * **Schema:** 
                    * **Schema Description:** The menu item's ID
 
                * **Type:** Guid
                * **Example:** `f9a7b8c4-0c4f-46e1-b4f5-d2fe9e5b35bb`
                * **Is Enum?:** false
                * **Is Array?:** false
            
        
            * *FranchiseId*
                * **Description:** [N/A]
                * **Schema Name:** Guid
                * **Schema:** 
                    * **Schema Description:** The franchises unique ID
 
                * **Type:** Guid
                * **Example:** `f9a7b8c4-0c4f-46e1-b4f5-d2fe9e5b35bb`
                * **Is Enum?:** false
                * **Is Array?:** false
            
        
            * *Name*
                * **Description:** [N/A]
                * **Schema Name:** String
                * **Schema:** 
                    * **Schema Description:** The menu item name
 
                * **Type:** String
                * **Example:** `Example String`
                * **Is Enum?:** false
                * **Is Array?:** false
            
        
            * *Description*
                * **Description:** [N/A]
                * **Schema Name:** String
                * **Schema:** 
                    * **Schema Description:** The menu item description (english)
 
                * **Type:** String
                * **Example:** `Example String`
                * **Is Enum?:** false
                * **Is Array?:** false
            
        
            * *NutritionFacts*
                * **Description:** [N/A]
                * **Schema Name:** SampleWebApiControllersMenutItemNutritionFacts
                * **Schema:** 
                    * **Schema Description:** A set of menu nutrition facts
 
                * **Reference ID:** SampleWebApi.Controllers.MenutItemNutritionFacts
                * [Object Details...](../schema/SampleWebApiControllersMenutItemNutritionFacts.md)
            
         
         


## Extensions
* x-operationExtension = `hello world`





### [< Back to Path](../Paths/Apiv1Menuitems.md)
### [<< Back to API](../SampleWebApi.Readme.md)

*** 

*[Documentation generated with Swagabond](https://github.com/jordanbleu/swagabond)*

