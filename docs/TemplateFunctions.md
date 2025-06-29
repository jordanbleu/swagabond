# Template Functions

Swagabond provides a list of template functions that can be used.  This is a definitive list 
of each and everyone of them.

>
> ℹ️ Note - By convention, each scriban provided function will always be prefixed with 'f_'. 
>


## f_Log
Used for debugging. Will log a message to the console or the configured logger.

### Parameters:

* `message` - The message to log.


### Examples
#### Scriban
```
{{f_Log message }}
```




## f_Upper
Converts the input string to UPPERCASE

### Parameters:

* `input` - input


### Examples
#### Scriban
```
{{f_Upper input }}
```




## f_Lower
Returns the input string in lowercase

### Parameters:

* `input` - input


### Examples
#### Scriban
```
{{f_Lower input }}
```




## f_UrlEncode
URL Encodes the input string. 

### Parameters:

* `input` - input


### Examples
#### Scriban
```
{{f_UrlEncode input }}
```




## f_Coalesce
If the input value is null or whitespace, returns the default value.

### Parameters:

* `input` - input value

* `defaultValue` - the fallback value if input is null


### Examples
#### Scriban
```
{{f_Coalesce input defaultValue }}
```




## f_StripNewlines
Replaces new lines with spaces (environment agnostic).

### Parameters:

* `input` - input string


### Examples
#### Scriban
```
{{f_StripNewlines input }}
```




## f_JsonString
This takes an input as json, and returns it escaped and all on one line.
Quotes will be escaped, " will become \".
Special Characters will be escaped per json rules.
Newlines will be stripped.
Input will be automatically wrapped in quotes.
So for example, input `{ "hello": "world" }` becomes `"{ \"hello\": \"world\" }"` (including outer quotes)

### Parameters:

* `input` - Your JSON formatted input


### Examples
#### Scriban
```
{{f_JsonString input }}
```




## f_Substring
Returns a substring of your input string. 

### Parameters:

* `input` - input string

* `start` - which character to begin output

* `length` - how many characters after start to include


### Examples
#### Scriban
```
{{f_Substring input start length }}
```




## f_PascalCase
Splits your string on any non alpha numeric tokens and combines them using PascalCase notation.

### Parameters:

* `input` - input string


### Examples
#### Scriban
```
{{f_PascalCase input }}
```




## f_CamelCase
Splits your string on any non alpha numeric tokens and combines them using PascalCase notation.

### Parameters:

* `input` - input string


### Examples
#### Scriban
```
{{f_CamelCase input }}
```




## f_LastDottedSegment
Given an input string such as this.is.a.test, will return the final token when splitting by the '.' character.
In that case, it would return 'test'.

### Parameters:

* `input` - input string with one or more dots


### Examples
#### Scriban
```
{{f_LastDottedSegment input }}
```




## f_PrefixNewlines
Inserts the prefix value before each newline character, while still preserving the newline characters.
This can be used for situations where you'd like to preserve newlines but need each newline to have something
in front of it, such as for block comments in code, etc.
            
A great example is when you are generating .net summary tags, this method can be used to insert a '///' in front
of each newline.

### Parameters:

* `input` - input string, with newlines

* `prefix` - what you want to insert in front of each newline


### Examples
#### Scriban
```
{{f_PrefixNewlines input prefix }}
```




## f_IsBlank
returns true if the input string is null or whitespace

### Parameters:

* `input` - input string


### Examples
#### Scriban
```
{{f_IsBlank input }}
```




## f_RandomGuid
Returns a random guid each time it is called.

### Examples
#### Scriban
```
{{f_RandomGuid }}
```




## f_L33t
Converts your input to 1337.
Not very useful.

### Parameters:

* `input` - 


### Examples
#### Scriban
```
{{f_L33t input }}
```




## f_GetDate
Returns the current date as a long string.
Example = Saturday, May 31, 2025

### Parameters:

* `utc` - if true, will use utc timezone


### Examples
#### Scriban
```
{{f_GetDate utc }}
```




## f_GetTime
Returns the current time.
Example: 3:16:56 PM 

### Parameters:

* `utc` - if true, will use utc timezone


### Examples
#### Scriban
```
{{f_GetTime utc }}
```




## f_GetFormattedDateTime
returns the current date using a custom format string, following .net's conventions. IF an invalid format string
is passed in, a big angry error will be returned instead of your date / time. 

### Parameters:

* `utc` - if true, will use utc timezone

* `format` - The .net format specifier.  See: https://learn.microsoft.com/en-us/dotnet/standard/base-types/standard-date-and-time-format-strings


### Examples
#### Scriban
```
{{f_GetFormattedDateTime utc format }}
```




## f_FirstItem
Given a list of inputs, will return the first item in the collection.
If the list is empty will return an empty string.

### Parameters:

* `inputs` - A collection of inputs


### Examples
#### Scriban
```
{{f_FirstItem inputs }}
```




## f_LastItem
Given a list of inputs, will return the last item in the collection.
If the list is empty will return an empty string.

### Parameters:

* `inputs` - A collection of inputs


### Examples
#### Scriban
```
{{f_LastItem inputs }}
```




## f_WrapQuotes
Wraps the input with double quotes.  inputString because "inputString".

### Parameters:

* `input` - input string


### Examples
#### Scriban
```
{{f_WrapQuotes input }}
```




## f_Wq
Shorthand for the WrapQuotes function

### Parameters:

* `input` - input string


### Examples
#### Scriban
```
{{f_Wq input }}
```




## f_WrapDoubleBraces
Wraps the input in double braces without evaluating it's input.
inputString becomes {{inputString}}. Can help avoid awkward template
syntax when you want to literally output double braces..

### Parameters:

* `input` - input to wrap


### Examples
#### Scriban
```
{{f_WrapDoubleBraces input }}
```




## f_Wdb
Shorthand for the WrapBraces function.

### Parameters:

* `input` - input string


### Examples
#### Scriban
```
{{f_Wdb input }}
```





___

[Swagabond on GitHub](https://github.com/jordanbleu/swagabond)