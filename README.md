# RoRSkinBuilder
Toolkit for making skins for Risk of Rain 2

## Setup
#### Unity 2018.1 through 2019.2
 Start a new Unity3d Project and add this to your Packages/manifest.json dependencies array;
```json
    "com.kingenderbrine.rorskinbuilder": "https://github.com/KingEnderBrine/RoRSkinBuilder.git",
```

## Update
#### Unity 2018.1 through 2019.2
Open Packages/manifest.json file. Find these 4 lines and remove them, then open Unity, that will download new version.
```
"lock": {
    ...
           
    "com.kingenderbrine.rorskinbuilder": {
      "revision": "HEAD",
      "hash": "...."
    },
    
    ...
}
```
