@echo on
for %%f in (json/*.json) do (
    mongoimport --uri mongodb+srv://bramhalc:qAGBTrJ6U4McuEDl@safestride.pum3uy6.mongodb.net/OffenderData --collection initialExcel --type json --file json/%%~nf.json
)