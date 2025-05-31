# OperationV1

> *An operation is a type of request that can be made against a particular path. An example would be "GET /restaurants/{restaurantId}/menu", which is a GET operation against the '/restaurants/{restaurantId}/menu' path.  Another operation would be "POST /restaurants/{restaurantId}/menu", which is a POST operation against the same path.* 


### `Api`

The API this operation belongs to.



* ⚒️ Underlying Type: ApiV1

* ℹ️ : [ApiV1 Properties...](./ApiV1.md)



### `CookieParameters`

List of cookie parameters that can (or should) be sent with the request.


* 📚 Collection - Collection of items; Requires iteration.

* ⚒️ Underlying Type: SchemaReferenceV1

* ℹ️ : [SchemaReferenceV1 Properties...](./SchemaReferenceV1.md)



### `DefaultResponseBody`

A fallback response body for status codes that don't have a dedicated response body. This will sometimes be the ONLY response body if only one is defined.  Otherwise, more specific response bodies should be considered, and this one should be the fallback.



* ⚒️ Underlying Type: ResponseBodyV1

* ℹ️ : [ResponseBodyV1 Properties...](./ResponseBodyV1.md)



### `Description`

A brief description of this operation.



* ⚒️ Underlying Type: String



### `ErrorResponseBody`

Assuming response body will be the same for any error http status (300+) this will be the first error response body or the default fallback.              If your api returns different response shapes for different error status codes, you shouldn't use this, and instead you'll need more nuanced logic.



* ⚒️ Underlying Type: ResponseBodyV1

* ℹ️ : [ResponseBodyV1 Properties...](./ResponseBodyV1.md)



### `Extensions`

List of arbitrary extensions for this operation


* 📚 Collection - Collection of items; Requires iteration.

* ⚒️ Underlying Type: ExtensionV1

* ℹ️ : [ExtensionV1 Properties...](./ExtensionV1.md)



### `HeaderParameters`

List of header parameters for this operation.  Header parameters go in http headers.


* 📚 Collection - Collection of items; Requires iteration.

* ⚒️ Underlying Type: SchemaReferenceV1

* ℹ️ : [SchemaReferenceV1 Properties...](./SchemaReferenceV1.md)



### `IsEmpty`

If true, this is an empty operation.



* ⚒️ Underlying Type: Boolean



### `Method`

The Http Method for the operation



* ⚒️ Underlying Type: String



### `Name`

The name of the operation, formatted as PascalCase in a way that is suitable for use in code or filenames. The name will always be unique for each operation.



* ⚒️ Underlying Type: String



### `Path`

The Path this operation belongs to.



* ⚒️ Underlying Type: PathV1

* ℹ️ : [PathV1 Properties...](./PathV1.md)



### `PathParameters`

List of path parameters for this operation.  Path parameters are integrated into the url path, and are typically used to identify a specific resource. For example, in the url '/restaurants/{restaurantId}/menu', the {restaurantId} is a path parameter.


* 📚 Collection - Collection of items; Requires iteration.

* ⚒️ Underlying Type: SchemaReferenceV1

* ℹ️ : [SchemaReferenceV1 Properties...](./SchemaReferenceV1.md)



### `QueryParameters`

A list of query parameters for this operation.  Query parameters are placed in the URL after the '?' character.


* 📚 Collection - Collection of items; Requires iteration.

* ⚒️ Underlying Type: SchemaReferenceV1

* ℹ️ : [SchemaReferenceV1 Properties...](./SchemaReferenceV1.md)



### `RequestBody`

Information about the request Body (payload) for the operation, typically in Json or XML format.



* ⚒️ Underlying Type: RequestBodyV1

* ℹ️ : [RequestBodyV1 Properties...](./RequestBodyV1.md)



### `ResponseBodies`

List of each documented response type for this operation, and the expected response body shape that goes along with it.


* 📚 Collection - Collection of items; Requires iteration.

* ⚒️ Underlying Type: ResponseBodyV1

* ℹ️ : [ResponseBodyV1 Properties...](./ResponseBodyV1.md)



### `SuccessResponseBody`

Assuming response body will be the same for any success http status (200 - 299) this will be the first success response body or the default fallback.              If your api returns different response shapes for different success status codes, you shouldn't use this, and instead you'll need more nuanced logic.



* ⚒️ Underlying Type: ResponseBodyV1

* ℹ️ : [ResponseBodyV1 Properties...](./ResponseBodyV1.md)



### `Title`

A more beautiful and gorgeous title for the operation.  Not suitable for filenames.



* ⚒️ Underlying Type: String



___


# [🏠 Home](./ApiV1.md)


___

[Swagabond on GitHub](https://github.com/jordanbleu/swagabond)

*Last updated: Saturday, May 31, 2025 at 4:26:14 PM*
