# Prior to running this for the first time you'll need to run:
# chmod +x /Users/jbleu/Documents/git/Swagabond/update-docs.sh
# chmod +x ./utilities/Swagutils/SampleWebAppClient/generate.sh
# This script should be run if any changes are made to:
# * The object model (adding new properties or removing them)
# * The xml comments on the object model 
# * Template functions (adding or removing them)
# * The xml comments on the template functions
# * Any of the included template code
#
# ...when in doubt just run it for any change :)


echo "Updating the docs and stuff..."

# 1. Update the stuff in the ./docs folder 
cd ./utilities || exit 
./update-objectmodel-readmes.sh
cd ../

# 2. Regenerate the sample client
cd  ./utilities/Swagutils/SampleWebAppClient || exit
./generate.sh
cd ../../../

echo "Update Complete! Please verify the changed files before committing."
