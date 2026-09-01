# Prior to running this for the first time you'll need to run these commands to give your machine permissions:
# chmod +x ./update-docs.sh
# chmod +x ./utilities/update-objectmodel-readmes.sh
#
#
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

echo "Update Complete! Please verify the changed files before committing."
