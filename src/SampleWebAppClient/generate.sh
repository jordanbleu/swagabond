############################################
# Run this to generate the client code     #
############################################
echo ""
echo "Generating Client + Docs ==> Current directory: $(pwd)"
echo "..."

# Build Swagabond CLI 
dotnet build ../Swagabond.Cli/Swagabond.Cli.csproj

# Generate the documentation files first 
dotnet run --project ../Swagabond.Cli/Swagabond.Cli.csproj -s ../SampleWebApi/swagger.json -i ../../templates/markdown/instructions.yaml -o "$(pwd)/docs"

# Generate the Client code
dotnet run --project ../Swagabond.Cli/Swagabond.Cli.csproj -s ../SampleWebApi/swagger.json -i ../../templates/csharp-flurl/instructions-no-csproj.yaml -o "$(pwd)"
