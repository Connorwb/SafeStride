from OSMPythonTools.overpass import Overpass, overpassQueryBuilder
from OSMPythonTools.nominatim import Nominatim
import time
import sys
import os
nominatim = Nominatim()
areaId = nominatim.query('Volusia County, Florida, United States').areaId()
overpass = Overpass()
nodeIDs = {}
doublesIDs = {}
result = overpass.query(overpassQueryBuilder(area=areaId, elementType='way', selector='"highway"~"path|foot|crossing|footway|sidewalk"'))
print("Opening Data Folder")
time.sleep(2)
if str(sys.argv).__contains__("-d"):
    Downloads = open("..\\SafeStride\\Assets\\Data\\Intersections.txt", "w+")
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
for key, value in doublesIDs.items():
    print(value)
    Downloads.write(value + "\n")
print("ALL DONE!")