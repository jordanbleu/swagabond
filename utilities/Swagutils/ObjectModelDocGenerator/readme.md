# ObjectModelDocGenerator

This project basically scrapes xml summary / etc tags from 
the object model classes and then generates markdown based
on those. That way we don't need to keep documentation
up to date, we just need to make sure we leave good 
comments.

By default the `<summary>` tag is used as the description.

### Remarks 

The `<remarks>` tag is special and can be used to convey
specific points about a property (this is only for the 
object model, not template functions). Each specific callout
should be separated by a semi colon.

These can be used to point out super important info that 
will be given a dedicated 'note' box. An example of when to 
use this would be if a property can be null.

Example:

```
<remarks>This value can be null;Blah blah blah</remarks>
```

Should output something like this:

> [!NOTE]  
> This value can be null

> [!NOTE]  
> Blah blah blah

