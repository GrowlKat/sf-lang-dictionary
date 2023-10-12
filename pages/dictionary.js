import React, { useState, useEffect, useRef } from "react"
import { CustomButton, CustomInput } from "./components/CustomComponents"
import "./styles/Dictionary.css"
import "./index.css"
import "./App.css"
import api from "./api/API"
import WordComponent from "./components/WordComponent"
import CharacterDropdown from "./components/CharacterDropdown"
import { BiSolidDownArrow } from "react-icons/bi"
import { RiKeyboardLine } from "react-icons/ri"
import { BsLinkedin, BsGithub, BsTwitter } from "react-icons/bs"
import { checkIfVocal } from "./Utils"

/**
 * Main page that let the user search for words
 * @returns 
 */
function Dictionary() {
    const [search, setSearch] = useState("") // Search string captured at input
    const [validSearch, setValidSearch] = useState(true) // Sets if should appear an error message
    const [word, setWord] = useState(null) // Gets the object returned by the API
    const [wordConjugations, setWordConjugations] = useState(null) // Gets the object returned by the API
    const [shownWord, setShownWord] = useState(<></>) // Show the data of the current word in application
    const [errorMessage, setErrorMessage] = useState("") // Sets the error message shown if there's an error while searching a word
    const [dropdownOpen, setDropdownOpen] = useState(false) // Sets if the dropdown menu should be shown or not

    const inputRef = useRef(null); // Reference to the input element, used to focus on it when the user clicks on a character button

    /**
     * Update the word that sould be shown in application
     * @param {*} wordObject The word object obtained by API
     */
    function showWord(wordObject, conjugationsObject) {
        let isEmpty = wordObject === null || wordObject === undefined // The object is null or undefined?
        setValidSearch(search !== "" && !isEmpty) // If search string and word object are empty, an error message is shown
        if (!isEmpty) setShownWord(<WordComponent wordObject={wordObject} wordConjugations={conjugationsObject}/>) // If word object is not empty, updates the shown word in application
    }

    async function handleSubit(event) {
        event.preventDefault() // Prevents reloading the page

        let result = {}

        // Checks if search string is not empty, if it's not, try updating the word that the user searched, if not, an error message is shown
        if (search.trim() !== "") {
            try {
                result = await api.get(`/Rootwords/GetByWord/${search.toLowerCase()}`) // Gets a word with the API by the search given by user
                
                // If result is not empty, cancel the error message and updates the word shown in application, if not, show a message error
                if (result !== "") {
                    setValidSearch(true)
                    let resultConjugations = await api.get(`/Suffixes/GetByDeclension/${checkIfVocal(result.rootword1) ? 1 : 2}`) // Gets the conjugations of the word
                    setWordConjugations(resultConjugations)
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

    /**
     * Show or hide the dropdown menu with special characters, and focus on the input element
     * @param {*} e The event that triggered the function
     */
    function handleDropdownClick(e) {
        e.preventDefault()
        setDropdownOpen(!dropdownOpen)
        inputRef.current.focus()
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

    // Updates the word shown in application when word object is updated too
    useEffect(() => {
        if (word) showWord(word, wordConjugations)
      }, [word, wordConjugations]);

    return(
        <>
        <div className="App">
            <main className='App-main'>
                <div className="word-form">
                    <form onSubmit={handleSubit}>
                        Search for a word:<br/>
                        {/* Search bar to search for a word */}
                        <div className="word-searchbar">
                            {/* Button to show or hide the dropdown menu of special characters */}
                            <CustomButton action="#"
                                borderColor={"transparent"} 
                                backgroundColor={"transparent"} 
                                transitionColor={"transparent"} 
                                style={{display: "inline-flex", alignItems: "center", justifyContent: "center", alignContent: "center", position: "relative", width: "64px", height: "48px"}}
                                type="button"
                                onClick={handleDropdownClick} >
                                <RiKeyboardLine style={{width: "32px", height: "32px", zIndex: "1", display: "inline-flex", alignItems: "center", justifyContent: "center", alignContent: "center", position: "relative"}}/>
                                <BiSolidDownArrow style={{position: "relative", width: "16px", height: "16px"}}/>
                            </CustomButton>
                            <label>
                                {/* Input to search the word */}
                                <CustomInput 
                                    backgroundColor={"white"} 
                                    transitionColor={"#9E9E9E"} 
                                    className={validSearch ? "" : "invalid-input"}
                                    value={search} placeholder="Enter a word to search about"
                                    onChange={(e) => setSearch(e.target.value)}
                                    ref={inputRef}
                                    style={{color: "black"}} />
                            </label>
                            {/* Button to search the word, submits the form */}
                            <CustomButton 
                                borderColor={"black"} 
                                backgroundColor={"#162F94"} 
                                transitionColor={"#004ABF"} 
                                type="submit" >
                                Search
                            </CustomButton>
                        </div>
                    </form>

                    {/* Here are the error messsages, the dropdown menu, and the word shown in application */}
                    {!validSearch && <p className="error-message">{errorMessage}</p>}
                    <CharacterDropdown dropdownOpen={dropdownOpen} handleClick={setSearch} searchValue={search} inputRef={inputRef}/>
                    {shownWord}
                </div>
            </main>
            
            {/* Footer where credits are shown */}
            <footer className='App-footer'>
                <ul>
                    <li>Developed by&nbsp;<b>Growl Kat</b></li>
                    <li>{<BsLinkedin/>}<a href='https://linkedin.com/in/growlkat'>/in/growlkat</a></li>
                    <li>{<BsGithub/>}<a href='https://github.com/GrowlKat'>/GrowlKat</a></li>
                    <li>{<BsTwitter/>}<a href='https://twitter.com/Growl_Kat'>@Growl_Kat</a></li>
                </ul>
            </footer>
        </div>
        </>
    )
}

export default Dictionary;