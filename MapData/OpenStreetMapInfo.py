from OSMPythonTools.overpass import Overpass, overpassQueryBuilder
from OSMPythonTools.nominatim import Nominatim
nominatim = Nominatim()
areaId = nominatim.query('Volusia County, Florida, United States').areaId()
overpass = Overpass()
nodeIDs = {}
doublesIDs = {}
result = overpass.query(overpassQueryBuilder(area=areaId, elementType='way', selector='"highway"~"path|foot|crossing|footway|sidewalk"'))
Downloads = open("..\\SafeStride\\Assets+"
                 "+\\Data\\Intersections.txt" , "w+")
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
for item in doublesIDs.items():
    Downloads.write(item + "\n")
print("ALL DONE!")