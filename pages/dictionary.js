import React, { useState, useEffect } from "react"
import { CustomButton, CustomInput } from "./Components/StyledComponents"
import "./styles/Dictionary.css"
import "./index.css"
import "./App.css"
import api from "./api/API"
import WordComponent from "./components/WordComponent"
import CharacterDropdown from "./components/CharacterDropdown"
import { MdOutlineEmojiSymbols } from "react-icons/md"
import { BiSolidCaretDownCircle, BiSolidDownArrow } from "react-icons/bi"
import { BsLinkedin, BsGithub, BsTwitter } from "react-icons/bs"

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
        setValidSearch(search !== "" && !isEmpty) // If search string and word object are empty, an error message is shown
        if (!isEmpty) setShownWord(<WordComponent wordObject={wordObject}/>) // If word object is not empty, updates the shown word in application
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
        <div className="App">
            <main className='App-main'>
                <div className="word-form">
                    <form onSubmit={handleSubit}>
                        <label>
                            Search for a word:<br/>
                            <CustomButton action="#"
                                borderColor={"transparent"} 
                                backgroundColor={"transparent"} 
                                transitionColor={"transparent"} 
                                style={{width: "48px", height: "48px", display: "inline-flex", alignItems: "center", justifyContent: "center", alignContent: "center", position: "relative", top: "4px"}}
                                type="button"
                            >
                                <MdOutlineEmojiSymbols style={{width: "32px", height: "32px", zIndex: "1"}}/>
                            </CustomButton>
                            <BiSolidDownArrow style={{position: "absolute", top: "84px", left: "20px", width: "16px", height: "16px"}}/>
                            <CustomInput 
                                backgroundColor={"white"} 
                                transitionColor={"#9E9E9E"} 
                                className={validSearch ? "" : "invalid-input"}
                                value={search} placeholder="Enter a word to search about"
                                onChange={(e) => setSearch(e.target.value)}/>
                            {/*<CharacterDropdown dropdownOpne={true}/>*/}
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
                    {shownWord}
                </div>
            </main>
            
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