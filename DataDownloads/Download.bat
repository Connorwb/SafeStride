@echo on
mongoexport --uri mongodb+srv://bramhalc:qAGBTrJ6U4McuEDl@safestride.pum3uy6.mongodb.net/WaypointData --collection Intersections --out Import/data.json --verbose

