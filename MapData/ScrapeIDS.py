from OSMPythonTools.overpass import Overpass, overpassQueryBuilder
from OSMPythonTools.nominatim import Nominatim
nominatim = Nominatim()
areaId = nominatim.query('Volusia County, Florida, United States').areaId()
overpass = Overpass()
nodeIDs = {}
doublesIDs = {}
result = overpass.query(overpassQueryBuilder(area=areaId, elementType='way', selector='"highway"~"path|foot|crossing|footway|sidewalk"'))
print("Opening Data Folder")
for way in result.elements():
    for node in way.nodes():
        print(str(node.id()) + ", ", end = '')
