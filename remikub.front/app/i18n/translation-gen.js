// Script wich update the foreign culture translation files
// If the key is already present in the file, it won't be overidden
// It add the new key with french value by adding (FR) at the end of the label
// It remve the unused labels.

const flatten = require("flat");
const fs = require("fs");

translateModule(__dirname);

function translateModule(moduleFolder) {
    const frenchMap = new Map();
    const mainFolder = `${moduleFolder}/fr-FR`;

    fs.readdirSync(mainFolder).forEach(file => {
        if (file.endsWith(".json")) {
            populateTranslationFileMap(`${mainFolder}/${file}`, frenchMap);
        }
    });

    const enMap = new Map();
    populateTranslationFileMap(`${moduleFolder}/en-GB/en-GB.json`, enMap);
    generateTranslationFile(`${moduleFolder}/en-GB/en-GB.json`, frenchMap, enMap);
}

function generateTranslationFile(destFile, frMap, translationMap) {
    translationMap.forEach((value, key) => {
        if (!frMap.has(key)) {
            translationMap.delete(key);
        }
    });
    frMap.forEach((value, key) => {
        if (!translationMap.has(key)) {
            translationMap.set(key, value.slice(0, -1) + '(FR)"');
        }
    });

    const translations = [];
    translationMap.forEach(trad => translations.push(trad));
    translations.sort();

    fs.writeFileSync(destFile, "{\n");
    fs.open(destFile, "a", (err1, fd) => {
        if (err1) {
            throw err1;
        }
        translations.forEach((trad, idx) => {
            fs.appendFileSync(fd, (idx > 0 ? ",\n" : "") + trad, "utf8", err2 => { if (err2) { throw err2; } });
        });
        fs.appendFileSync(fd, "\n}\n", "utf8", err3 => { if (err3) { throw err3; } });
        fs.close(fd, err4 => { if (err4) { throw err4; } });
    });
}

function populateTranslationFileMap(filePath, translationMap) {
    const translations = flatten(require(filePath));

    // tslint:disable-next-line:forin
    for (var key in translations) {
        translationMap.set(key, `"${key}": "${translations[key]}"`);
    }
}
