/**
 * Capitalizes a string
 * @param {*} s The string to be capitalized
 * @returns The capitalized string
 */
export function capitalize(s) {
    return s.charAt(0).toUpperCase() + s.slice(1)
}