/*
* Simple script to expidite development by moving these files to the main NOP project.
*/

var fs = require('fs')

const
    sourceRoot = '../',
    destinationRoot = 'C:/Repos/nopCommerce-release-4.60.4/nopCommerce-release-4.60.4/src/Presentation/Nop.Web/Plugins/i7MEDIA.Plugin.Widgets.Registry/Areas/Admin/',
    files = [
        '/Views/_RegistryList.cshtml',
        '/Views/_RegistryConsultant.cshtml',
        '/Views/_RegistryType.cshtml',
        '/Views/index.cshtml',
        '/Views/_RegistryShippingOption.cshtml',
        '/Views/_RegistryRow.cshtml',
        '/Views/_RegistryNotification.cshtml',
        '/Views/_AddDialog.cshtml',

        '/Scripts/Tabs/registry.js',
        '/Scripts/Tabs/registryConsultant.js',
        '/Scripts/Tabs/registryType.js',
        '/Scripts/Tabs/registryShippingOption.js',
        '/Scripts/index.js',

        '/Styles/index.css',
        '/Styles/Tabs/registry.css',
        '/Styles/Tabs/registryConsultant.css',
        '/Styles/Tabs/registryType.css',
        '/Styles/Tabs/registryShippingOption.css',
        '../modules/utils.js'
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