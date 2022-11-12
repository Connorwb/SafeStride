from OSMPythonTools.overpass import Overpass, overpassQueryBuilder
from OSMPythonTools.nominatim import Nominatim
import time
import sys
import os
import json
nominatim = Nominatim()
areaId = nominatim.query('Volusia County, Florida, United States').areaId()
overpass = Overpass()
nodeIDs = {}
doublesIDs = {}
result = overpass.query(overpassQueryBuilder(area=areaId, elementType='way', selector='"highway"~"path|foot|crossing|footway|sidewalk"'))
print("Opening Data Folder")
time.sleep(2)
if str(sys.argv).__contains__("-d"):
    print("Making JSON files")
elif str(sys.argv).__contains__("-t"):
    Downloads = open("..\\..\\..\\SafeStride\\Assets\\Data\\Intersections.txt", "w+")
elif str(sys.argv).__contains__("-u"):
    print("Unity Mode!!!!")
    Downloads = open("{0}\\Assets\\Data\\Intersections.txt".format(os.getcwd()), "w+")
else :
    raise Exception("There was no argument given for the data file's destination.")
print("Opened Data Folder")
time.sleep(2)
for way in result.elements():
    for node in way.nodes():
        if node.id() in nodeIDs.keys():
            doublesIDs.update({node.id(): str(node.lat()) + "," + str(node.lon()) + "," + nodeIDs[node.id()]})
            del nodeIDs[node.id()]
            print("Found intersection " + str(node.id()) + " at " + str(node.lat()) + "," + str(node.lon()))
        elif node.id() in doublesIDs.keys():
            doublesIDs[node.id()] = doublesIDs[node.id()] + "," + str(way.id())
        else:
            nodeIDs.update({node.id(): str(way.id())})
if str(sys.argv).__contains__("-d"):
    for key, value in doublesIDs.items():
        Downloads = open(".\\json\\Intersections" + str(key) + ".json", "w+")
        stringArray = value.split(",")
        for i in range(2, len(stringArray)):
            stringArray[i] = int(stringArray[i])
        towrite = {
            "nodeID": key,
            "latitude": float(stringArray[0]),
            "longitude": float(stringArray[1]),
            "connections": stringArray[2:]
        }
        json_object = json.dumps(towrite, indent=4)
        Downloads.write(json_object + "\n")
else:
    for key, value in doublesIDs.items():
        print(value)
        Downloads.write(value + "\n")
print("ALL DONE!")