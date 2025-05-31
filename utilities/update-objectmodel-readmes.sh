################################################################################################
# Run this to generate the markdown for the Swagabond object model and the template functions
################################################################################################

echo "Current directory: $(pwd)"

# Make Sure that the object model v1 and templates projects are built since 
# we need the documentation file
# Building these in debug mode because the doc generator is hard coded to the debug dir
# It's hard coded to the debug dir because it makes local dev easier and im lazy 
dotnet build ../src/Swagabond.ObjectModelV1/Swagabond.ObjectModelV1.csproj -c debug
dotnet build ../src/Swagabond.Templates/Swagabond.Templates.csproj -c debug

# Run the doc generator 
dotnet build ./Swagutils/ObjectModelDocGenerator/ObjectModelDocGenerator.csproj -c debug

cd ./Swagutils/ObjectModelDocGenerator/bin/debug/net8.0 || exit
dotnet run --project ../../../ObjectModelDocGenerator.csproj
