# PathV1

> *A path is simply a route which is based on the API's base URL. A path contains a list of operations (such as GET, POST, PUT, etc.) that can be performed against that path.* 


### `Api`

The API this path belongs to



* ⚒️ Underlying Type: ApiV1

* ℹ️ : [ApiV1 Properties...](./ApiV1.md)



### `Description`

A brief description of the path



* ⚒️ Underlying Type: String



### `ExtensionDictionary`

A dictionary of extensions where the key is the extension name and the value is its value.  This allows you to bind directly to known keys instead of iterating over the list of extensions. Values can be accessed via `ExtensionDictionary["x-myKey"]`


* 📖 Dictionary - Can have any number of keys / values depending on context.

* ⚒️ Underlying Type: Dynamic



### `Extensions`

List of arbitrary extensions for this path


* 📚 Collection - Collection of items; Requires iteration.

* ⚒️ Underlying Type: ExtensionV1

* ℹ️ : [ExtensionV1 Properties...](./ExtensionV1.md)



### `IsEmpty`

If true, this path is empty.



* ⚒️ Underlying Type: Boolean



### `Name`

A cleanly formatted name for this path in PascalCase.



* ⚒️ Underlying Type: String



### `Operations`

List of all endpoints (and their details) for this path


* 📚 Collection - Collection of items; Requires iteration.

* ⚒️ Underlying Type: OperationV1

* ℹ️ : [OperationV1 Properties...](./OperationV1.md)



### `Route`

This is the actual route of the path.  This is safe to use for actual routing logic in generated code.



* ⚒️ Underlying Type: String



### `Title`

The unfiltered path, not suitable for code or filenames.  This is actually the same thing as the 'route' property because I don't really know a prettier way to name a path. Despite that fun fact, this should NOT be used for anything other than display purposes.



* ⚒️ Underlying Type: String



___


# [🏠 Home](./ApiV1.md)


___

[Swagabond on GitHub](https://github.com/jordanbleu/swagabond)
