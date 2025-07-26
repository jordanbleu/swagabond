# SchemaDefinitionV1

> *Defines the entire structure of a schema used in an API. A SchemaDefinition can be a primitive type, complex object, enum, array, and more.* 


### `Api`

The API that this belongs to



* ⚒️ Underlying Type: ApiV1

* ℹ️ : [ApiV1 Properties...](./ApiV1.md)



### `Constraints`

A set of validation rules around the schema, generally used for client side validation.



* ⚒️ Underlying Type: PropertyConstraintsV1

* ℹ️ : [PropertyConstraintsV1 Properties...](./PropertyConstraintsV1.md)



### `DataType`

This is the underlying type of the schema, whether it be an object or a primitive type. For enums, this represents the backing value of the enum (usually an int).



* ⚒️ Underlying Type: DataTypeV1

* ℹ️ : [DataTypeV1 Properties...](./DataTypeV1.md)



### `Description`

A brief description of the schema definition.



* ⚒️ Underlying Type: String



### `EnumOptions`

For enums, this will contain a list of each enum name / value.  For non-enums, this will be empty.  Enum names are determined by adding an extension to the schema in your spec called x-enumNames, with each name corresponding to an enum value in the same order. If there's no enumNames extension, each enum name will be something dumb like Item1, Item2, etc.


* 📚 Collection - Collection of items; Requires iteration.

* ⚒️ Underlying Type: EnumOptionV1

* ℹ️ : [EnumOptionV1 Properties...](./EnumOptionV1.md)



### `Example`

an example value for the object.  If the spec doesn't provide an example, a dummy value will be generated from Swagabond.  The example values won't always be actual valid values for the api. One thing to note is that a generic fallback example will NOT be provided for complex objects, so, instead you should use the JsonExample property.



* ⚒️ Underlying Type: Object

* ℹ️ : [Object Properties...](./Object.md)



### `Extensions`

List of arbitrary extensions for this schema definition.


* 📚 Collection - Collection of items; Requires iteration.

* ⚒️ Underlying Type: ExtensionV1

* ℹ️ : [ExtensionV1 Properties...](./ExtensionV1.md)



### `IsArray`

If true, this schema defines an array of items rather than a single item.



* ⚒️ Underlying Type: Boolean



### `IsEmpty`

True if the schema definition is empty.



* ⚒️ Underlying Type: Boolean



### `IsEnum`

Whether this is an enum or not. An enum is a special type of schema that defines a set of named values.



* ⚒️ Underlying Type: Boolean



### `IsPrimitive`

Returns true if the schema type is a simple value (not a complex object)



* ⚒️ Underlying Type: Boolean



### `JsonExample`





* ⚒️ Underlying Type: String



### `Name`

A name for the schema definition, formatted as PascalCase. Suitable for code or filenames.



* ⚒️ Underlying Type: String



### `OriginalName`

The full / unfiltered schema name.  Useful in cases where the exact name must be used, including original casing (this does not format as pascal case). There's no guarantee that this is suitable for filenames / code, so only use if you know how the schema names will look for your api spec.



* ⚒️ Underlying Type: String



### `Properties`

For a complex object type, this contains a list of inner properties for this schema definition. Each item in this list contains the property name and its schema definition.


* 📚 Collection - Collection of items; Requires iteration.

* ⚒️ Underlying Type: SchemaReferenceV1

* ℹ️ : [SchemaReferenceV1 Properties...](./SchemaReferenceV1.md)



### `ReferenceId`

An identifier for this schema item.  For OpenAPI, this will point to the schema reference id.



* ⚒️ Underlying Type: String



### `Title`

A more beautiful and gorgeous title for the schema definition. Not suitable for filenames, or code.



* ⚒️ Underlying Type: String



___


# [🏠 Home](./ApiV1.md)


___

[Swagabond on GitHub](https://github.com/jordanbleu/swagabond)
