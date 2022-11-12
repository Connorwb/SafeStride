@echo on
for %%f in (json/*.json) do (
    mongoimport --uri mongodb+srv://bramhalc:qAGBTrJ6U4McuEDl@safestride.pum3uy6.mongodb.net/WaypointData --collection Intersections --type json --file json/%%~nf.json
)