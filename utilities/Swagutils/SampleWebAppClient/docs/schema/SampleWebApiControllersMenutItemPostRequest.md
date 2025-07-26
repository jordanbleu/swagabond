
# SampleWebApi

## SampleWebApi.Controllers.MenutItemPostRequest

SampleWebApiControllersMenutItemPostRequest

A post request for menu items


`Object`

```

```

### Properties


* *FranchiseId*
    * **Description:** [N/A]
    * **Schema Name:** Guid
    * **Schema:** 
        * **Schema Description:** The franchise ID this menu item is offered by
 
        * **Type:** Guid
        * **Example:** `f9a7b8c4-0c4f-46e1-b4f5-d2fe9e5b35bb`
        * **Is Enum?:** false
        * **Is Array?:** false
    
    * Schema Constraints
        * **Is Nullable:** false

* *Name*
    * **Description:** [N/A]
    * **Schema Name:** String
    * **Schema:** 
        * **Schema Description:** The name of the menu item
 
        * **Type:** String
        * **Example:** `Example String`
        * **Is Enum?:** false
        * **Is Array?:** false
    
    * Schema Constraints
        * **Is Nullable:** true
        * **RegEx Pattern:**  ^[A-Z][a-zA-Z]{0,14}$

        * **Minimum Length:** 0

        * **Maximum Length:** 15

* *Description*
    * **Description:** [N/A]
    * **Schema Name:** String
    * **Schema:** 
        * **Schema Description:** The menu item's description in english
 
        * **Type:** String
        * **Example:** `Example String`
        * **Is Enum?:** false
        * **Is Array?:** false
    
    * Schema Constraints
        * **Is Nullable:** true

* *Calories*
    * **Description:** [N/A]
    * **Schema Name:** Int32
    * **Schema:** 
        * **Schema Description:** How many calories is this menu item?
 
        * **Type:** Int32
        * **Example:** `123`
        * **Is Enum?:** false
        * **Is Array?:** false
    
    * Schema Constraints
        * **Is Nullable:** false

        * **Minimum Value:** 1
        * **Inclusive:** false

        * **Maximum Value:** 3000
        * **Inclusive:** true

* *ProteinGrams*
    * **Description:** [N/A]
    * **Schema Name:** Int32
    * **Schema:** 
        * **Schema Description:** How many grams of protein does the item contain?
 
        * **Type:** Int32
        * **Example:** `123`
        * **Is Enum?:** false
        * **Is Array?:** false
    
    * Schema Constraints
        * **Is Nullable:** false




## Extensions
* x-schemaExtension = `hello world`


### [<< Back to API](../SampleWebApi.Readme.md)

*** 

*[Documentation generated with Swagabond](https://github.com/jordanbleu/swagabond)*

