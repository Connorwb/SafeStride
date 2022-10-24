import time
import sys
import os
import json
print("Opening JSON File")
time.sleep(2)
BigJson = open(".\\csvjson.json", "r")
data = json.load(BigJson)
for offender in data:
    LittleJson = open(".\\json\\Offender" + str(offender[""]) +".json", "w+")
    json_object = json.dumps(offender, indent=4)
    LittleJson.write(json_object)
print("ALL DONE!")