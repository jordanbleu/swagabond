# ResponseBodyV1

> *Defines a response shape for a particular HTTP status code in an API operation.* 


### `Api`

The API that this response body belongs to.



* ⚒️ Underlying Type: ApiV1

* ℹ️ : [ApiV1 Properties...](./ApiV1.md)



### `Description`

A brief description of the response body.



* ⚒️ Underlying Type: String



### `IsEmpty`

If true, there is no response body.



* ⚒️ Underlying Type: Boolean



### `Name`

A cleanly formatted name for this response body in PascalCase. Suitable for code or filenames. Something to note, if your API spec has explicit schema definitions for each response (which is common), you can actually use that instead if you prefer.  Simply grab the info you need from the .Schema property instead.



* ⚒️ Underlying Type: String



### `Operation`

The operation that this response body belongs to.



* ⚒️ Underlying Type: OperationV1

* ℹ️ : [OperationV1 Properties...](./OperationV1.md)



### `ResponseId`

A unique identifier for this individual response shape.  Used to differentiate each response body.



* ⚒️ Underlying Type: String



### `Schema`

The schema definition of the response body.



* ⚒️ Underlying Type: SchemaDefinitionV1

* ℹ️ : [SchemaDefinitionV1 Properties...](./SchemaDefinitionV1.md)



### `StatusCode`

The status code that this response shape is for



* ⚒️ Underlying Type: Int32



### `Title`

A more beautiful and gorgeous title for the response body. Not suitable for filenames, only for display purposes.



* ⚒️ Underlying Type: String



___


# [🏠 Home](./ApiV1.md)


___

[Swagabond on GitHub](https://github.com/jordanbleu/swagabond)

*Last updated: Saturday, May 31, 2025 at 4:26:14 PM*
