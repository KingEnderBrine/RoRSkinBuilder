# RoRSkinBuilder
Toolkit for making skins for Risk of Rain 2

## Setup
There are two ways of installing, either manually editing the projects manifest.json or by using the Unity Package Manager
#### Manual:
- Open the manifest.json file in ProjectRoot/Packages
- Add the following to the top of dependencies

  `"com.kingenderbrine.rorskinbuilder": "https://github.com/KingEnderBrine/RoRSkinBuilder.git",` 
- Save and close the file
- Focus Unity to start the install
    - If you encounter an error when you go back into Unity, make sure the syntax for the json is correct
    - Examples of errors are extra or missing characters such as `,`, `{` and `}`
#### Unity Package Manager
- Add Package from Git URL by using the Unity Package Manager plus icon at the top left corner
- Enter a url `https://github.com/KingEnderBrine/RoRSkinBuilder.git`;
