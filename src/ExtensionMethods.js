/**
 * Loads all the extention methods needed on the project
 */

/*export function loadExtensionMethods() {
    String.prototype.capitalize = function () {
        return this.charAt(0).toUpperCase() + this.slice(1)
    }
}*/

export function capitalize(s) {
    return s.charAt(0).toUpperCase() + s.slice(1)
}