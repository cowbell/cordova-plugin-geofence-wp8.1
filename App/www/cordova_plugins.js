cordova.define('cordova/plugin_list', function(require, exports, module) {
module.exports = [
    {
        "file": "plugins/com.borismus.webintent/www/webintent.js",
        "id": "com.borismus.webintent.WebIntent",
        "clobbers": [
            "WebIntent"
        ]
    },
    {
        "file": "plugins/com.cowbell.cordova.geofence/www/geofence.js",
        "id": "com.cowbell.cordova.geofence.geofence",
        "clobbers": [
            "geofence"
        ]
    },
    {
        "file": "plugins/com.vladstirbu.cordova.promise/www/promise.js",
        "id": "com.vladstirbu.cordova.promise.Promise",
        "clobbers": [
            "Promise"
        ]
    }
];
module.exports.metadata = 
// TOP OF METADATA
{
    "com.borismus.webintent": "1.0.0",
    "com.cowbell.cordova.geofence": "0.2.2",
    "com.vladstirbu.cordova.promise": "1.0.0"
}
// BOTTOM OF METADATA
});