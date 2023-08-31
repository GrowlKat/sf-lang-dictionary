import React, { useState, useEffect } from "react"
import { CustomButton, CustomInput } from "./Components/StyledComponents"
import "./Styles/Dictionary.css"
import api from "./API"
import { capitalize } from "./ExtensionMethods"

/**
 * Main page that let the user search for words
 * @returns 
 */
function Dictionary() {
    const [search, setSearch] = useState("") // Search string captured at input
    const [validSearch, setValidSearch] = useState(true) // Sets if should appear an error message
    const [word, setWord] = useState(null) // Gets the object returned by the API
    const [shownWord, setShownWord] = useState(<></>) // Show the data of the current word in application
    const [errorMessage, setErrorMessage] = useState("") // Sets the error message shown if there's an error while searching a word

    /**
     * Update the word that sould be shown in application
     * @param {*} wordObject The word object obtained by API
     */
    function showWord(wordObject) {
        let isEmpty = wordObject === null || wordObject === undefined // The object is null or undefined?
        let rootword = ""; // Initialize a string for trying to capitalizing the rootword in the object

        // If rootword in object is not empty, capitalizes it, if not, just sets the empty rootword
        rootword = wordObject.rootword1 !== "" ? capitalize(wordObject.rootword1) : wordObject.rootword1

        setValidSearch(search !== "" && !isEmpty) // If search string and word object are empty, an error message is shown

        // If word object is not empty, updates the shown word in application
        if (!isEmpty) {
            setShownWord(
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
    }

    async function handleSubit(event) {
        event.preventDefault() // Prevents reloading the page

        let result = {}

        // Checks if search string is not empty, if it's not, try updating the word that the user searched, if not, an error message is shown
        if (search.trim() !== "") {
            try {
                result = await api.get(`/Rootwords/GetByWord/${search}`) // Gets a word with the API by the search given by user
                
                // If result is not empty, cancel the error message and updates the word shown in application, if not, show a message error
                if (result !== "") {
                    setValidSearch(true)
                    setWord(result)
                }
                else {
                    setValidSearch(false)
                    setErrorMessage("Word not found, please try with another search")
                }
            }
            catch {
                // When an exception is catched, automatically sets an error message 
                setValidSearch(false)
                setErrorMessage("Word not found, please try with another search")
            }
        }
        else {
            setValidSearch(false)
            setErrorMessage("Please enter a search to find a word")
        }
    }

    // The component to show words in application is initialized
    useEffect(() => {
        setShownWord(
            <>
            <dl>
            <dt key={"word0"}>Search for a word in Selenish Language!</dt>
                <dd key={"data0"}></dd>
            </dl>
            </>
        )
    }, [])

    // Updates the word shown in application is updated when word object is updated too
    useEffect(() => {
        if (word) showWord(word)
      }, [word]);

    return(
        <>
        <form onSubmit={handleSubit}>
            <label>
                Search for a word:<br/>
                <CustomInput 
                    backgroundColor={"white"} 
                    transitionColor={"#9E9E9E"} 
                    className={validSearch ? "" : "invalid-input"}
                    value={search} placeholder="Enter the word to search about"
                    onChange={(e) => setSearch(e.target.value)}/>
            </label>
            {!validSearch && <p className="error-message">{errorMessage}</p>}
            <CustomButton 
                borderColor={"black"} 
                backgroundColor={"#162F94"} 
                transitionColor={"#004ABF"} 
                type="submit"
            >
                Search
            </CustomButton>
        </form>
        <div>
            { shownWord }
        </div>
        </>
    )
}

export default Dictionary;