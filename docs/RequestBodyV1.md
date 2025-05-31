# RequestBodyV1

> *Contains information about a request body for an operation in an API. A request body is a payload sent to an API endpoint, typically as Json or XML.* 


### `Api`

The API that this request body belongs to.



* ⚒️ Underlying Type: ApiV1

* ℹ️ : [ApiV1 Properties...](./ApiV1.md)



### `Description`

A description of the request body.



* ⚒️ Underlying Type: String



### `IsEmpty`

If this is true, there is no response body.



* ⚒️ Underlying Type: Boolean



### `Name`

A cleanly formatted name for this request body in PascalCase. Suitable for code or filenames. Something to note, if your API spec has explicit schema definitions for each request (which is common), you can actually use that instead if you prefer.  Simply grab the info you need from the .Schema property instead.



* ⚒️ Underlying Type: String



### `Operation`

The operation that this request body belongs to



* ⚒️ Underlying Type: OperationV1

* ℹ️ : [OperationV1 Properties...](./OperationV1.md)



### `Schema`

The schema definition of the request body



* ⚒️ Underlying Type: SchemaDefinitionV1

* ℹ️ : [SchemaDefinitionV1 Properties...](./SchemaDefinitionV1.md)



### `Title`

a more beautiful and gorgeous title for the request body. Not suitable for filenames, only for display purposes.



* ⚒️ Underlying Type: String



___


# [🏠 Home](./ApiV1.md)


___

[Swagabond on GitHub](https://github.com/jordanbleu/swagabond)

*Last updated: Saturday, May 31, 2025 at 4:26:14 PM*
