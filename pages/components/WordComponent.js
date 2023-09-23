import { capitalize } from "../ExtensionMethods"

/**
 * Render a component that show a word info
 * @param {*} wordObject Object requested in API with word info 
 * @returns 
 */
function WordComponent({wordObject}) {
    let rootword = "" // Initialize a string for trying to capitalizing the rootword in the object

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
        </dl>
        </>
    )
}

export default WordComponent;