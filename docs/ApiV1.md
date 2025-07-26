# ApiV1

> *The Root object for the entire API.* 


### `BaseUrls`

List of BaseUrls registered for this server.  Based on the 'servers' that are defined by your API spec.


* 📚 Collection - Collection of items; Requires iteration.

* ⚒️ Underlying Type: String



### `Description`

A brief description of the API



* ⚒️ Underlying Type: String



### `ExtensionDictionary`

A dictionary of extensions where the key is the extension name and the value is its value.  This allows you to bind directly to known keys instead of iterating over the list of extensions. Values can be accessed via `ExtensionDictionary["x-myKey"]`


* 📖 Dictionary - Can have any number of keys / values depending on context.

* ⚒️ Underlying Type: Dynamic



### `Extensions`

List of extensions on the API. Extensions can contain any arbitrary data.


* 📚 Collection - Collection of items; Requires iteration.

* ⚒️ Underlying Type: ExtensionV1

* ℹ️ : [ExtensionV1 Properties...](./ExtensionV1.md)



### `ExternalDocumentationLink`

Generally, an outside link to external documentation



* ⚒️ Underlying Type: HrefV1

* ℹ️ : [HrefV1 Properties...](./HrefV1.md)



### `Info`

Various extra information about the API such as ToS, Contact Info, etc



* ⚒️ Underlying Type: InfoV1

* ℹ️ : [InfoV1 Properties...](./InfoV1.md)



### `IsEmpty`

This should never be true.  Feel free to ignore :)



* ⚒️ Underlying Type: Boolean



### `Metadata`

This is populated via Swagabond itself, generally via Template Instructions. The data here isn't necessarily related to the spec itself.


* 📖 Dictionary - Can have any number of keys / values depending on context.

* ⚒️ Underlying Type: Dynamic



### `Name`

The name of the API, formatted a PascalCase string with no spaces or special characters.



* ⚒️ Underlying Type: String



### `Operations`

A flattened list of all operations that are defined in the API, by any path.


* 📚 Collection - Collection of items; Requires iteration.

* ⚒️ Underlying Type: OperationV1

* ℹ️ : [OperationV1 Properties...](./OperationV1.md)



### `Paths`

List of each path (aka route) exposed by the API.  Each path item also contains a list of operations that can be performed on that route.


* 📚 Collection - Collection of items; Requires iteration.

* ⚒️ Underlying Type: PathV1

* ℹ️ : [PathV1 Properties...](./PathV1.md)



### `Schemas`

All the schema definitions referenced by the entire API.  A schema definition defines the properties of a complex object.


* 📚 Collection - Collection of items; Requires iteration.

* ⚒️ Underlying Type: SchemaDefinitionV1

* ℹ️ : [SchemaDefinitionV1 Properties...](./SchemaDefinitionV1.md)



### `Servers`

List of base or direct URLs that host the API.


* 📚 Collection - Collection of items; Requires iteration.

* ⚒️ Underlying Type: ServerV1

* ℹ️ : [ServerV1 Properties...](./ServerV1.md)



### `SpecType`

The type of API spec the model was mapped from.



* ⚒️ Underlying Type: SpecTypeV1

* ℹ️ : [SpecTypeV1 Properties...](./SpecTypeV1.md)



### `SpecVersion`

The version of the current API spec that this object model is based on.  This will vary based on what type of API spec is being mapped.



* ⚒️ Underlying Type: String



### `Title`

The original, raw name of the API.  May contain special characters or spaces, so not great for generating code or filenames.



* ⚒️ Underlying Type: String



### `Version`

A string representing the version of your API



* ⚒️ Underlying Type: String



___



___

[Swagabond on GitHub](https://github.com/jordanbleu/swagabond)
