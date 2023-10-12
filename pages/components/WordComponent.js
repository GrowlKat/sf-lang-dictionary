import { useState } from "react"
import { capitalize, Cases, Numbers } from "../Utils"
import { BiSolidDownArrow } from "react-icons/bi"
import './CharacterDropdown.js.css'

/**
 * Render a component that show a word info
 * @param {*} wordObject Object requested in API with word info 
 * @returns 
 */
function WordComponent({wordObject, wordConjugations}) {
    const [isTableOpen, setIsTableOpen] = useState(false) // Sets if the table should be shown or not
    let rootword = "" // Initialize a string for trying to capitalizing the rootword in the object

    /**
     * Find the conjugation of a word based on its case and number
     * @param {*} rootword The rootword to analyze
     * @param {*} caseType The case type of the word
     * @param {*} numberType  The number type of the word
     * @returns The suffix of the word case declension
     */
    function setConjugation(rootword, caseType, numberType) {
        // Find the conjugation of the word based on its case and number, each switch case have a nested switch case for the number type
        switch (caseType) {
            case Cases.NOMINATIVE:
                switch (numberType) {
                    case Numbers.SINGULAR:
                        return rootword + wordConjugations.find((c) => c.stp.subtype1 === "Singular Nominative").suffix1
                    case Numbers.PLURAL:
                        return rootword + wordConjugations.find((c) => c.stp.subtype1 === "Plural Nominative").suffix1
                }
            case Cases.ACCUSATIVE:
                switch (numberType) {
                    case Numbers.SINGULAR:
                        return rootword + wordConjugations.find((c) => c.stp.subtype1 === "Singular Accusative").suffix1
                    case Numbers.PLURAL:
                        return rootword + wordConjugations.find((c) => c.stp.subtype1 === "Plural Accusative").suffix1
                }
            case Cases.GENITIVE:
                switch (numberType) {
                    case Numbers.SINGULAR:
                        return rootword + wordConjugations.find((c) => c.stp.subtype1 === "Singular Genitive").suffix1
                    case Numbers.PLURAL:
                        return rootword + wordConjugations.find((c) => c.stp.subtype1 === "Plural Genitive").suffix1
                }
            case Cases.DATIVE:
                switch (numberType) {
                    case Numbers.SINGULAR:
                        return rootword + wordConjugations.find((c) => c.stp.subtype1 === "Singular Dative").suffix1
                    case Numbers.PLURAL:
                        return rootword + wordConjugations.find((c) => c.stp.subtype1 === "Plural Dative").suffix1
                }
        }
    }

    function toggleTable() {
        setIsTableOpen(!isTableOpen)
    }

    // If rootword in object is not empty, capitalizes it, if not, just sets the empty rootword
    rootword = wordObject.rootword1 !== "" ? capitalize(wordObject.rootword1) : wordObject.rootword1
    return(
        <>
        <dl>
        <dt key={"word" + wordObject.rootId}>{rootword}</dt>
            <dd key={"data" + wordObject.rootId}>
                Meaning: {wordObject.meaning}<br/>
                Pronunciation: {wordObject.pronunciation}
            </dd>
            <BiSolidDownArrow className={`inline-flex relative transform ${isTableOpen ? 'rotate-180' : ''}`} onClick={toggleTable} style={{cursor: 'pointer'}}/>
        </dl>
        {isTableOpen && <div className="declension-table">
            <table>
                <caption>Declension: {wordConjugations[0].mtpId === 1 ? "Strong" : "Soft"}</caption>
                <thead>
                    <tr>
                        <th>Case</th>
                        <th>Nominative</th>
                        <th>Accusative</th>
                        <th>Genitive</th>
                        <th>Dative</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <th>Singular</th>
                        <td>{capitalize(setConjugation(wordObject.rootword1, Cases.NOMINATIVE, Numbers.SINGULAR))}</td>
                        <td>{capitalize(setConjugation(wordObject.rootword1, Cases.ACCUSATIVE, Numbers.SINGULAR))}</td>
                        <td>{capitalize(setConjugation(wordObject.rootword1, Cases.GENITIVE, Numbers.SINGULAR))}</td>
                        <td>{capitalize(setConjugation(wordObject.rootword1, Cases.DATIVE, Numbers.SINGULAR))}</td>
                    </tr>
                    <tr>
                        <th>Plural</th>
                        <td>{capitalize(setConjugation(wordObject.rootword1, Cases.NOMINATIVE, Numbers.PLURAL))}</td>
                        <td>{capitalize(setConjugation(wordObject.rootword1, Cases.ACCUSATIVE, Numbers.PLURAL))}</td>
                        <td>{capitalize(setConjugation(wordObject.rootword1, Cases.GENITIVE, Numbers.PLURAL))}</td>
                        <td>{capitalize(setConjugation(wordObject.rootword1, Cases.DATIVE, Numbers.PLURAL))}</td>
                    </tr>
                </tbody>
            </table>
        </div>
        }
        </>
    )
}

export default WordComponent;