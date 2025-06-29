################################################################################################
# Run this to generate the markdown for the Swagabond object model and the template functions
################################################################################################

echo "Current directory: $(pwd)"

# Make Sure that the object model v1 and templates projects are built since 
# we need the documentation file
dotnet build ../src/Swagabond.ObjectModelV1/Swagabond.ObjectModelV1.csproj -c debug
dotnet build ../src/Swagabond.Templates/Swagabond.Templates.csproj -c debug

# Run the doc generator 
dotnet build ./Swagutils/ObjectModelDocGenerator/ObjectModelDocGenerator.csproj -c debug

cd ./Swagutils/ObjectModelDocGenerator/bin/debug/net9.0 || exit
dotnet run --project ../../../ObjectModelDocGenerator.csproj
