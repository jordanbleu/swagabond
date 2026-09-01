# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What this is

Swagabond is a language-agnostic, template-driven code/doc generator for OpenAPI specs (think a much simpler,
Scriban-templated alternative to NSwag/OpenAPI Generator). Users write their own Scriban templates plus a YAML
"instruction set" describing what to render and where; this repo builds the CLI, the object model the templates
run against, and ships a few example template sets (`templates/csharp-flurl`, `templates/markdown`,
`templates/postman`).

## Development principles

- Keep changes minimal and scoped to what was actually asked — don't do unrequested extra work.
- Exception: if while working you learn something about the codebase that should change this file, update it and
  call out that you did so.
- Never hand-edit anything under `docs/` — it's generated (see "regenerate the docs" below).
- After changing `Swagabond.ObjectModelV1` classes (properties, XML doc comments), run `./update-docs.sh`.
- Add unit/integration tests for changes where feasible.
- Keep comments minimal. A comment should describe only the code's current behavior — no rationale, history, or
  justification for why it's written that way, and nothing that's already obvious from the code.
- Don't put implementation details in comments on interfaces or abstract classes.
- Developers have existing templates written against this object model and CLI. If a change could break an
  existing template, call that out explicitly. Prefer backwards-compatible changes when possible.
- Don't reference OpenAPI in anything under `templates/` (Scriban logic, generated output, comments). Templates
  are meant to stay spec-format-agnostic so other spec formats besides OpenAPI can target them later.
- After adding code, build and check for new compiler warnings — especially nullable reference warnings — and
  address them rather than leaving them.

## Solutions and layout

There are two independent solutions — don't assume a single `dotnet build` at the repo root covers everything:

- `src/Swagabond.sln` — the actual library/CLI (published to NuGet as `Swagabond.Cli`)
  - `Swagabond.Core` — OpenAPI spec parsing/validation (`MicrosoftSwaggerParser`, `OpenApiMapper`), spec error/warning
    gating
  - `Swagabond.ObjectModelV1` — the object model exposed to templates (`ApiV1`, `PathV1`, `OperationV1`,
    `SchemaDefinitionV1`, etc.) and the `Transformer/` classes that build it from a parsed `OpenApiDocument`
  - `Swagabond.Configuration` — instruction-set YAML model (`InstructionSet`) and its serializer
  - `Swagabond.Templates` — template engine abstraction (`ITemplateEngine`) plus the Scriban implementation and
    built-in template functions (`f_*` filters)
  - `Swagabond.Cli` — the CLI entrypoint; `Execution/ExecutionPlanBuilder` is where instruction sets, the object
    model, and the template engine come together
  - `Swagabond.Tests` / `Swagabond.IntegrationTests` — unit and integration tests for the above
- `utilities/Swagutils/Swagutils.sln` — internal dev tooling, not shipped: just `ObjectModelDocGenerator`, which
  regenerates `docs/*.md` from XML doc comments on the object model / template functions

## Common commands

Build/test the main solution:
```
dotnet build src/Swagabond.sln
dotnet test src/Swagabond.sln
```

Run a single test (both test projects use xunit-style filtering):
```
dotnet test src/Swagabond.Tests --filter "FullyQualifiedName~SchemaDefinitionV1TransformerTests"
dotnet test src/Swagabond.Tests --filter "FullyQualifiedName~SchemaDefinitionV1TransformerTests.SpecificTestName"
```

Build/test the utilities solution (separate restore/build, has its own tool manifest):
```
dotnet restore utilities/Swagutils/Swagutils.sln
dotnet tool restore --tool-manifest utilities/Swagutils/.config/dotnet-tools.json
dotnet build utilities/Swagutils/Swagutils.sln
```

Run the CLI directly against a spec + instruction set (useful for exercising template changes end-to-end):
```
dotnet run --project src/Swagabond.Cli -- -s <path-or-url-to-swagger.json> -i <path-to-instructions.yaml> -o <output-dir> -c true
```
Key flags (see `src/Swagabond.Cli/Args/Arguments.cs`): `-s/--swagger`, `-i/--instructions`, `-o/--output`,
`-c/--cleanOutput`, `-e/--failOnApiSpecError`, `-w/--failOnApiSpecWarning`, `-p/--maxDop`, `-j/--dumpJson`,
`-v/--verbose`.

After changing the object model (properties added/removed, XML doc comments), template functions, or any of the
built-in template files, regenerate the docs under `docs/`:
```
./update-docs.sh
```
This rebuilds `Swagabond.ObjectModelV1`/`Swagabond.Templates`, runs `ObjectModelDocGenerator`, and re-emits
`docs/*.md`. Review the diff before committing — it's driven entirely by XML doc comments on the model classes.

CI (`.github/workflows/dotnet.yaml`) builds and tests both solutions on every push/PR to `main`; releases are cut
by pushing a `v*.*.*` tag, which triggers `.github/workflows/publish.yaml` to pack and push `Swagabond.Cli` to
nuget.org.

## Architecture: how a generation run works

1. **Parse** — `Swagabond.Core` (`MicrosoftSwaggerParser`/`OpenApiMapper`) reads the swagger/OpenAPI file (json or
   yaml, local path or URL) into a `Microsoft.OpenApi` `OpenApiDocument`, collecting spec errors/warnings that can
   optionally halt generation (`--failOnApiSpecError` / `--failOnApiSpecWarning`).
2. **Transform** — `Swagabond.ObjectModelV1`'s `Transformer/*` classes walk the `OpenApiDocument` and build a tree
   rooted at `ApiV1` (paths → operations → request/response bodies; schemas → properties, recursively). This tree
   is the *entire* surface templates see — it's spec-format-agnostic by design, so every template gets the same
   object model regardless of what the source spec looked like. See `docs/ApiV1.md` for the full shape.
3. **Read instructions** — `Swagabond.Configuration` deserializes the instruction-set YAML (`InstructionSet`) into
   `for_api` / `for_schema_definitions` / (path- and operation-scoped) instruction lists, each naming a template
   file, an output path template, and optional `include_before`/`include_after` template files.
4. **Plan** — `Swagabond.Cli.Execution.ExecutionPlanBuilder` renders each instruction's output-filename template
   against its scope (the whole `ApiV1`, or a given `PathV1`/`OperationV1`/`SchemaDefinitionV1`) to get a concrete
   output path, and queues a render task per unique output path. It throws if two instructions in the same run
   would resolve to the same output path — output path templates must be collision-free for whatever data your
   spec can contain (this is why schema-derived filenames use `SchemaDefinitionV1.Name`, a pre-sanitized/PascalCased
   unique name, rather than the raw dotted `OriginalName`).
5. **Render** — `Swagabond.Templates` resolves the template engine for the instruction (`Scriban` today,
   `TemplateEngineFactory`/`ITemplateEngine` is the extension point for others) and renders the template against
   its scoped model object, with any `include_before`/`include_after` files concatenated in. Custom template
   functions (`f_*`) live in `Swagabond.Templates/Functions/TemplateFunctions.cs`; per-template-set helper
   functions (`x_*` by convention) live in that template set's own `functions.scriban`.
6. **Write** — output is written under the run's output directory, with line endings normalized per
   `InstructionSet.LineEndingStyle`, all up to `--maxDop` renders running in parallel.

## Working with the object model

- `SchemaDefinitionV1.Name` is the pre-computed, PascalCase, code/filename-safe name for a schema (derived from
  the full dotted schema id via `ToClassName()`). `OriginalName` is the raw, unfiltered name — it may contain dots,
  spaces, or other characters and is *not* guaranteed safe for filenames or code identifiers. When a template needs
  to declare or reference a schema as a type/filename, use `Name`, not `OriginalName`.
- Every generated instruction-set/template pair for `csharp-flurl` has two variants:
  `templates/csharp-flurl/instructions.yaml` (includes a `.csproj`) and `instructions-no-csproj.yaml` (for
  generating into an existing project). Keep both in sync when changing how schema definitions are named or
  written out.
- `ApiV1.Operations` and `ApiV1.BaseUrls` are lazily computed and cached from `Paths`/`Servers` on first read;
  mutating `Paths`/`Servers` after that point won't be reflected.
