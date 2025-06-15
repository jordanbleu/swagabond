
# SampleWebApi

## GET /api/v1/restaurants


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
    "StoreNumber": 123,
    "Address": "Example String",
    "Zip": "Example String",
    "City": "Example String",
    "State": "1 (Alabama)"
  }
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
                * **Schema Name:** SampleWebApiControllersRestaurantGetResponseItem[]
                * **Schema:** 
                    * **Schema Description:** A single restaurant item, which includes the restaurant's name, address, phone number, website, and description.
 
                * **Reference ID:** SampleWebApi.Controllers.RestaurantGetResponseItem
                * [Object Details...](../schema/SampleWebApiControllersRestaurantGetResponseItem.md)
            
         
         

* *Default / Fallback Response (for any status code that is not explicitly defined, this response can be assumed)*
    * **Title:** 200 Response - 200GetResponse
    * **Name:** 200GetResponse
    * **Description:** OK
     
        * **Properties:**
        
            * *Items*
                * **Description:** [N/A]
                * **Schema Name:** SampleWebApiControllersRestaurantGetResponseItem[]
                * **Schema:** 
                    * **Schema Description:** A single restaurant item, which includes the restaurant's name, address, phone number, website, and description.
 
                * **Reference ID:** SampleWebApi.Controllers.RestaurantGetResponseItem
                * [Object Details...](../schema/SampleWebApiControllersRestaurantGetResponseItem.md)
            
         
         


## Extensions
* x-operationExtension = `hello world`





### [< Back to Path](../Paths/Apiv1Restaurants.md)
### [<< Back to API](../SampleWebApi.Readme.md)

*** 

*[Documentation generated with Swagabond](https://github.com/jordanbleu/swagabond)*

