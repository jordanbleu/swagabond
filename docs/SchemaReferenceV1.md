# SchemaReferenceV1

> *a Name and reference to a schema definition. Used for named properties that refer to a specific schema.* 


### `Description`

A brief description of the schema reference.



* ⚒️ Underlying Type: String



### `IsEmpty`

If true, the schema reference is empty.



* ⚒️ Underlying Type: Boolean



### `Name`

The name of the property formatted as PascalCase.  Suitable for code / filenames.



* ⚒️ Underlying Type: String



### `OriginalName`

The full / unfiltered name.  Useful in cases where the exact name must be used, including original casing.  (this does not format as pascal case). /// There's no guarantee that this is suitable for filenames / code, so only use if you know /// how the schema names will look for your api spec.



* ⚒️ Underlying Type: String



### `Schema`

The schema definition that this reference points to.



* ⚒️ Underlying Type: SchemaDefinitionV1

* ℹ️ : [SchemaDefinitionV1 Properties...](./SchemaDefinitionV1.md)



### `Title`

Currently...this is always empty.



* ⚒️ Underlying Type: String



___


# [🏠 Home](./ApiV1.md)


___

[Swagabond on GitHub](https://github.com/jordanbleu/swagabond)
