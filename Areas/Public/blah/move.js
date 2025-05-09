/*
* Simple script to expidite development by moving these files to the main NOP project.
*/

var fs = require('fs')


console.log('meloy');

const
    sourceRoot = '../',
    destinationRoot = 'C:/Repos/nopCommerce-release-4.60.4/nopCommerce-release-4.60.4/src/Presentation/Nop.Web/Plugins/i7MEDIA.Plugin.Widgets.Registry/Areas/Public/',
    files = [
        '/Scripts/index.js',
        '/Scripts/product.js',
        '/Styles/index.css',
        '/Styles/product.css',
        '/Views/List.cshtml',
        '/Views/Index.cshtml',
        '/Views/RegistryLink.cshtml',
        '/Views/ProductLink.cshtml',
        '/Scripts/modules/utils.js'
    ]

files.forEach(file => {
    const source = `${sourceRoot}/${file}`,
        destination = `${destinationRoot}/${file}`;



    fs.copyFile(source, destination, (err) => {
        if (err) {
            console.error(`Unable to move ${file}`, err);
        } else {
            console.log(`Moved ${file}`);
        }
    });
});