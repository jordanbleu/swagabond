
# SampleWebApi

## GET /api/v1/menuitems/{id}/full



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
    * **Title:** 200 Response - Get200Response
    * **Name:** Get200Response
    * **Description:** OK
     
        * **Properties:**
        
            * *Item*
                * **Description:** [N/A]
                * **Schema Name:** SamplewebapiControllersMenuitemresponseitem
                * **Schema:** 
                    * **Schema Description:** A single menu item
 
                * **Reference ID:** SampleWebApi.Controllers.MenuItemResponseItem
                * [Object Details...](../schema/SamplewebapiControllersMenuitemresponseitem.md)
            
        
            * *Franchise*
                * **Description:** [N/A]
                * **Schema Name:** SamplewebapiControllersFranchiseinformation
                * **Schema:** 
                    * **Schema Description:** An object containing franchise information
 
                * **Reference ID:** SampleWebApi.Controllers.FranchiseInformation
                * [Object Details...](../schema/SamplewebapiControllersFranchiseinformation.md)
            
         
         

* *Default / Fallback Response (for any status code that is not explicitly defined, this response can be assumed)*
    * **Title:** 200 Response - Get200Response
    * **Name:** Get200Response
    * **Description:** OK
     
        * **Properties:**
        
            * *Item*
                * **Description:** [N/A]
                * **Schema Name:** SamplewebapiControllersMenuitemresponseitem
                * **Schema:** 
                    * **Schema Description:** A single menu item
 
                * **Reference ID:** SampleWebApi.Controllers.MenuItemResponseItem
                * [Object Details...](../schema/SamplewebapiControllersMenuitemresponseitem.md)
            
        
            * *Franchise*
                * **Description:** [N/A]
                * **Schema Name:** SamplewebapiControllersFranchiseinformation
                * **Schema:** 
                    * **Schema Description:** An object containing franchise information
 
                * **Reference ID:** SampleWebApi.Controllers.FranchiseInformation
                * [Object Details...](../schema/SamplewebapiControllersFranchiseinformation.md)
            
         
         


### [< Back to Path](../Paths/ApiV1MenuitemsidFull.md)
### [<< Back to API](../SampleWebApi.Readme.md)

*** 

*[Documentation generated with Swagabond](https://github.com/jordanbleu/swagabond)*

