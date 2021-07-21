## Azure Maps Creator Tools
Azure Maps Creator service provides functionality for creating and using map data for indoor environments.
To create a custom Indoor Map, you have to prepare [**Drawing Package**](https://docs.microsoft.com/en-us/azure/azure-maps/drawing-requirements) and upload it using [Azure Maps Conversion Service](https://docs.microsoft.com/en-us/rest/api/maps/v2/conversion)

A **Drawing Package** is a *.zip archive that contains the following files:
- DWG files in AutoCAD DWG file format.
- A manifest.json file that describes the DWG files in the Drawing package.

This solution provides the following features: 

- [x] Generates manifest describing DWG files and facility metadata from CAD files
- [ ] Tool for aligning CAD drawing over the AzureMap


## Getting Started
Download the solution and compile the ManifestGenerator project.
Make sure that **manifest-defaults.json** and **CategoryMapping.json** files are located in the same directory with the executable.

You can make changes to the *directoryInfo* section of the **manifest-defaults.json** file. These changes will be applied to the generated manifest.

```javascript 
"directoryInfo": {
    "name": "Contoso Building 1",
    "streetAddress": "Contoso Way",
    "unit": "1",
    "locality": "Eastside",
    "postalCode": "00000",
    "adminDivisions": [
      "Contoso City",
      "Contoso State",
      "United States"
    ],
    "hoursOfOperation": "Mo-Fr 08:00-17:00 open",
    "phone": "1 (425) 555-1234",
    "website": "www.contoso.com",
    "nonPublic": false,
    "anchorLatitude": 47.636152,
    "anchorLongitude": -122.132600,
    "anchorHeightAboveSeaLevel": 1000,
    "defaultLevelVerticalExtent": 3
  }
```

You can make changes to the *dwgLayers* section to make sure that DWG layer names in the manifest correspond to the layers in DWG files.
```javascript
  "dwgLayers": {
    "exterior": [
      "GROS$"
    ],
    "unit": [
      "RM$"
    ],
    "wall": [
      "A-WALL-EXST",
      "A-WALL-CORE-EXST",
      "A-GLAZ-EXST",
      "A-GLAZ-SILL-EXST",
      "A-WALL-MOVE-EXST"
    ],
    "door": [
      "A-DOOR-EXST"
    ],
    "unitLabel": [
      "A-IDEN-NUMR-EXST"
    ],
    "unitCategory": [
      "A-IDEN-NAME-EXST"
    ],
    "zone": [
      "ZONES"
    ],
    "zoneLabel": [
      "ZONELABELS"
    ]
  }
```
###### !  Make Shure that the specified layer names are common for all DWG files in the package.

If DWG files include the Unit categories layer, use **CategoryMapping.json** to map category names from your DWG files to category names accepted by Azure Maps Creator.

Sample:
```javascript
{
  "LobbyReception": "room.lobby",

  "Workpoint": "room.workspace",

  "FocusRoom": "room.focus",

  "Small": "room.conference.small",

  "Large": "room.conference.large",

  "Conference": "room.conference",

  "OpenMeeting": "room.openMeeting",

  "Office": "room.office"
}
```

To create a manifest file simply run the command below:
```
Chronos.AzureMaps.ManifestGenerator.exe -bm {PathToTheLocalFoderWithDWGFiles}
```

In the Command Prompt, you should see a list of processed CAD files with found units.
Once done, the process will save a `manifest.json` file in the folder with DWG files.
Verify manifest and apply manual changes to the file if needed and compress dwg files with manifest in a zip archive.

To upload zip package to Azure Maps you can use this [tool](https://github.com/Azure-Samples/AzureMapsCreator) created by the Microsoft team or upload it [manually](https://docs.microsoft.com/en-us/azure/azure-maps/drawing-package-guide).

## License

MIT