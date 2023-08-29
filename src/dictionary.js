import React, { useState, useEffect } from "react"
import { CustomButton, CustomInput } from "./Components/StyledComponents"
import "./Styles/Dictionary.css"
import api from "./API"
import { capitalize } from "./ExtensionMethods"

function Dictionary() {
    const [search, setSearch] = useState("")
    const [validSearch, setValidSearch] = useState(true)
    const [word, setWord] = useState(null)
    const [shownWord, setShownWord] = useState(<></>)
    const [errorMessage, setErrorMessage] = useState("")

    async function handleSubit(event) {
        event.preventDefault()

        let result = {}

        if (search.trim() !== "") {
            try {
                result = await api.get(`/rootwords/getByWord/${search}`)
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
                setValidSearch(false)
                setErrorMessage("Word not found, please try with another search")
            }
        }
        else {
            setValidSearch(false)
            setErrorMessage("Please enter a search to find a word")
        }
    }

    function showWord(wordObject) {
        let isEmpty = wordObject === null || wordObject === undefined
        let rootword = "";

        rootword = typeof wordObject.rootword1 !== "" ? capitalize(wordObject.rootword1) : wordObject.rootword1

        setValidSearch(search !== "" && !isEmpty)

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