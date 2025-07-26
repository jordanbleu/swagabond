# SchemaConstraintsV1

> *List of validation rules around a property* 


### `HasMaxLength`

For string based values, this will be true if the value has a maximum length requirement.



* ⚒️ Underlying Type: Boolean



### `HasMaxValue`

For numeric types, this will be true if the property specifies a maximum value, which is indicated by the MaxValue property.



* ⚒️ Underlying Type: Boolean



### `HasMinLength`

For string based values, this will be true if the value has a minimum length requirement.



* ⚒️ Underlying Type: Boolean



### `HasMinValue`

For numeric types, this will be true if the property specifies a minimum value, which is indicated by the MinValue property.



* ⚒️ Underlying Type: Boolean



### `IsEmpty`

Whether there are any constraints specified on this property at all



* ⚒️ Underlying Type: Boolean



### `IsMaxValueInclusive`





* ⚒️ Underlying Type: Boolean



### `IsMinValueInclusive`

If true, the value of MinValue itself is allowed for this value.  So `value >= MinValue`. If false, MinValue itself is NOT allowed, so `value > MinValue`



* ⚒️ Underlying Type: Boolean



### `IsNullable`

If true, this value can be null, even if the underlying type is a value type. For a string, this does not include empty strings. This is slightly different from IsRequired = false in the sense that the value can be present on the request and explicitly set to null.



* ⚒️ Underlying Type: Boolean



### `MaxLength`

For string based values, this will be the max length of the string. Before using this value you may want to check if HasMaxLength is true.
> [!IMPORTANT]  
> Check HasMaxLength before using this





* ⚒️ Underlying Type: Int32



### `MaxValue`

For numeric types, this will be the max value allowed. Whether this is inclusive or exclusive depends on the "IsMaxValueInclusive" property. Before using this value, you may want to check the `HasMaxValue` property which specifies if a max value check is needed.
> [!IMPORTANT]  
> Check HasMaxValue before using this





* ⚒️ Underlying Type: Decimal



### `MinLength`

For string based values, this will be the minimum length of the string. Before using this value you may want to check if HasMinLength is true.
> [!IMPORTANT]  
> Check HasMinLength before using this





* ⚒️ Underlying Type: Int32



### `MinValue`

For numeric types, this will be the minimum value allowed. Whether this is inclusive or exclusive depends on the "IsMinValueInclusive" property. Before using this value, you may want to check the `HasMinValue` property which specifies if a min value check is needed.
> [!IMPORTANT]  
> Check HasMinValue before using this





* ⚒️ Underlying Type: Decimal



### `Pattern`

A RegEx pattern following ECMA-262 syntax for this field. This can be used to evaluate input on the client before sending to the server.
> [!IMPORTANT]  
> This will be an empty string if not specified.





* ⚒️ Underlying Type: String



___


# [🏠 Home](./ApiV1.md)


___

[Swagabond on GitHub](https://github.com/jordanbleu/swagabond)
