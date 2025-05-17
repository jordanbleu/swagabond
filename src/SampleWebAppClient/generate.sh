############################################
# Run this to generate the client code     #
############################################

echo "Current directory: $(pwd)"

# Build Swagabond CLI 
dotnet build ../Swagabond.Cli/Swagabond.Cli.csproj

# Generate the documentation files first 
dotnet run --project ../Swagabond.Cli/Swagabond.Cli.csproj -s ../SampleWebApp/swagger.json -i ../../templates/markdown/instructions.yaml -o "$(pwd)/docs"

# Generate the Client code
dotnet run --project ../Swagabond.Cli/Swagabond.Cli.csproj -s ../SampleWebApp/swagger.json -i ../../templates/csharp-flurl-noproj/instructions.yaml -o "$(pwd)"
