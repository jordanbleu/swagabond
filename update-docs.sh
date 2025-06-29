#
# This script should be run if any changes are made to:
# * The object model (adding new properties or removing them)
# * The xml comments on the object model 
# * Template functions (adding or removing them)
# * The xml comments on the template functions
# * Any of the included template code
#
# ...when in doubt just run it for every PR 

echo "Updating the docs and stuff..."

# 1. Update the stuff in the ./docs folder 
./utilities/update-objectmodel-readmes.sh

# 2. Regenerate the sample client 
./utilities/Swagutils/SampleWebAppClient/generate.sh

echo "Update Complete! Please verify the changed files before committing."
