
```
 █▀     █ █ █     ▄▀█     █▀▀     
 ▄█     ▀▄▀▄▀     █▀█     █▄█    

             ▄▀█  
             █▀█

 █▄▄     █▀█     █▄ █     █▀▄
 █▄█     █▄█     █ ▀█     █▄▀
 ```

[![NuGet Version](https://img.shields.io/nuget/v/Swagabond.Cli)](https://www.nuget.org/packages/Swagabond.Cli)
[![GitHub Actions Workflow Status](https://img.shields.io/github/actions/workflow/status/jordanbleu/swagabond/dotnet.yaml)](https://github.com/jordanbleu/swagabond/actions/workflows/dotnet.yaml)
[![GitHub commit activity](https://img.shields.io/github/commit-activity/t/jordanbleu/swagabond)](https://github.com/jordanbleu/swagabond/commits/main/)
[![Static Badge](https://img.shields.io/badge/Read_the_Wiki-red?style=flat&logo=GitHub&logoColor=black)](https://github.com/jordanbleu/swagabond/wiki)
![GitHub Repo stars](https://img.shields.io/github/stars/jordanbleu/swagabond)

Swagabond is a catch-all solution for generating code or text output from an [OpenAPI spec](https://swagger.io/specification/).

## How it works

* You write your own templates alongside an "Instruction Set" (yaml file) that tells Swagabond how to process them
* You configure Swagabond in your build pipeline to automatically pull down your openAPI spec and process your templates
* That's it.

## Swagabond vs OpenAPI Generator

Swagabond is like a glass of plain Jack Daniels, OpenAPI Generator is like a whiskey old fashioned. Sometimes you just 
don't have time to muddle the cherries, and just wanna get drunk.

|          | Swagabond | OpenAPI Generator |
| -------- | -------   | ------- |
| Overall Vibe | Dumb, simple, easy to work with | Smart, powerful, higher learning curve |
| Best for | Prototyping, Simple APIs, Microservices, Internal tools, people with ADHD | Enterprise-scale applications, More nuanced OpenAPI Specs, People with lots of time |
| Learning Curve | Tiny, Can start writing a template instantly | Complicated, lots to learn |
| Template writing | Fewer options and features | Complex, more fully featured |
| Template syntax | Scriban, with more planned later | Mustache by default, but can write custom template engines |
| Object model | Every template gets the same exact object model | Varies based on each generator |
| Code-Gen Logic | Template driven | Varies, but each code generator exposes more code-specific nuances specific to the programming language |

## Getting started

Check out the wiki for a full in-depth tutorial!

## Template Documentation

* [All the data](./docs/ApiV1.md) - A full list of all the data available to your templates.
* [Template Functions](./docs/TemplateFunctions.md) - A list of custom functions 

## Author

Swagabond was written originally by [Jordan Bleu](https://linktr.ee/jordanbleu)
