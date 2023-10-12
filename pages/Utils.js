export var vocalsArray = ['a', 'ā', 'e', 'ē', 'i', 'ī', 'o', 'ō', 'u', 'ū', 'y', 'ȳ', 'ä', 'ö', 'ü']

export const Cases = {
    NOMINATIVE: 1,
    ACCUSATIVE: 2,
    GENITIVE: 3,
    DATIVE: 4
}

export const Numbers = {
    SINGULAR: 1,
    PLURAL: 2
}

/**
 * Capitalizes a string
 * @param {*} s The string to be capitalized
 * @returns The capitalized string
 */
export function capitalize(s) {
    return s.charAt(0).toUpperCase() + s.slice(1)
}

export function checkIfVocal(s) {
    let lastChar = s.charAt(s.length - 1)
    for (let i = 0; i < vocalsArray.length; i++) {
        if (lastChar === vocalsArray[i] || lastChar === vocalsArray[i].toUpperCase()) {
            return true
        }
    }
    return false
}