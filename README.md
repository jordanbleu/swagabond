
```
 █▀     █ █ █     ▄▀█     █▀▀     ▄▀█    
 ▄█     ▀▄▀▄▀     █▀█     █▄█     █▀█    

 █▄▄     █▀█     █▄ █     █▀▄
 █▄█     █▄█     █ ▀█     █▄▀
 ```



 > [!IMPORTANT]  
> This project is still a prototype.  A lot of functionality isn't there yet.

Swagabond is a catch-all solution for generating code or plain text from any [OpenAPI spec](https://swagger.io/specification/). Rather than existing solutions, Swagabond is fully template driven, allowing for complete control of your output.

## How it works 

1. You write up your templates and InstructionSet file (see below)
1. You pass this the Swagabond CLI along with a url / file path to your swagger file (json or yaml)
1. Swagabond's core logic will map your swagger file to an internal object model
1. This object model is then passed along to your templates.  What you do with that is up to you!

## The ObjectModel

The standard OpenAPI spec is somewhat flattened in it's hierarchy.  It uses things like references to other components for efficiency.  

However, writing templates is clunky enough without having to traverse back and forth across multiple objects, and resolve all sorts of references.  Therefore, the object model was designed to simplify this process (even at the cost of efficiency).

Because of this, templates naturally won't have 1 to 1 access to everything in your swagger spec.

### Structure

The object model creates a pseudo-hierarchy out of your API.  

Here's a rough example illustration of how it looks:
```
API
\_ Path - /foo
  \_ Operation - GET /foo
    \_ RequestBody
      \_ Property1 - id
      \_ Property2 - some other property
    \_ ResponseBody
      \_ Property1 - some sorta response property
    \_ QueryParameters 
  \_ Operation - POST /foo
    \_ ...etc
\_ Path - /foo/bar
  \_ Operation - Get /foo/bar
    \_ ...etc
```

So, in a sense, one API has many paths, One path has many operations, an operation has a request body, response body, and multiple query parameters.

Each item in the hierarchy also has access to its parent directly, which is useful for writing templates.  For example, if you are writing a template against an operation, you can get to the Path's route from a template with `Path.Route`.


## InstructionSet

The instruction set is a yaml file that tells Swagabond how to process your api and what templates to use. 

Templates can be written against a variety of 'scopes':
* API
* Path
* Operation
* Schema Object

For example, if you want to output a a separate file for each operation on your api, you can write a template that is scoped to a single operation, then use the instruction set to tell Swagabond to run your template for each operation.

Here's an example yaml file:
```
template_base_directory: .
output_base_directory: ../../template-output/markdown

for_api:
- use_template_file: api.scriban
  write_output_to: '{{Info.Title}}.Readme.md'

for_paths:
- use_template_file: path.scriban
  write_output_to: 'paths/{{Name}}.md'

for_operations:
- use_template_file: operation.scriban
  write_output_to: 'operations/{{Path.Name}}__{{Method}}.md'

metadata:
  Company: Jordan Bleu
```

* **template_base_directory:** This specifies the base path for your template files
* **output_base_directory:** This specifies the base directory for output
* **for_api:** Specifies to run the instructions below under the 'api' scope
   * **use_template_file:** The specified template will be run once for the entire API 
   * **write_output_to:** This determines the output file name.  The text here will be run through the templating engine as well, with the exact same syntax as your template itself.  Make sure each instance of write_output_to resolves to a unique file name.
* **for_paths:** This specifies path scope.  The instruction will be run for each path on your API
   * **use_template_file:** Since we are under path scope, this template file will be run once per path, with the path itself as the root object.
   * **write_output_to:** Similar to the previous setting will be used to determine a filename based on a path.  Just like your template itself this will have the path as the root object when rendering via the template engine.
* **for_operations:** same deal as above settings but with operations :) 
* **metadata:** Used to pass any static arbitrary data that will be made available to bind to via templates.

## Templates

Templates use [Scriban](https://github.com/scriban/scriban/tree/master) by default.

### Custom Functions

All template engines get a large chunk of free functions added alongside the respective built-in functions.  These can be found in the TemplateFunctions class under the Swagabond.Templates project.

By convention, these functions are prefixed by `f_`.  For example, to execute the `Upper` function, in your scriban template you'd write:
```
{{ f_Upper jordanbleu }}
```

## Author

Swagabond was written originally by [Jordan Bleu](https://linktr.ee/jordanbleu)# swagabond
