# ServerV1

> *Represents a 'server' that the API is hosted at.* 


### `Description`

A description for the server



* ⚒️ Underlying Type: String



### `ExtensionDictionary`

A dictionary of extensions where the key is the extension name and the value is its value.  This allows you to bind directly to known keys instead of iterating over the list of extensions. Values can be accessed via `ExtensionDictionary["x-myKey"]`


* 📖 Dictionary - Can have any number of keys / values depending on context.

* ⚒️ Underlying Type: Dynamic



### `Extensions`

A list of arbitrary extensions for the server


* 📚 Collection - Collection of items; Requires iteration.

* ⚒️ Underlying Type: ExtensionV1

* ℹ️ : [ExtensionV1 Properties...](./ExtensionV1.md)



### `Url`

The server's base URL.  This can either be a relative or direct URL, based on the original API spec.  The URL can also contain variables, but Swagabond does not currently support variable definitions for server URLs.



* ⚒️ Underlying Type: String



___


# [🏠 Home](./ApiV1.md)


___

[Swagabond on GitHub](https://github.com/jordanbleu/swagabond)
