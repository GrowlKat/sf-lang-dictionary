import React, { useState, useEffect, useRef } from "react"
import { CustomButton, CustomInput, NavInput, NavButton } from "../components/CustomComponents"
import "./styles/global.css"
import "./dictionary.css"
import "./index.css"
import "./App.css"
import api from "../api/API"
import WordComponent from "../components/WordComponent"
import CharacterDropdown from "../components/CharacterDropdown"
import { BiSolidDownArrow } from "react-icons/bi"
import { RiKeyboardLine } from "react-icons/ri"
import { BsLinkedin, BsGithub, BsTwitter } from "react-icons/bs"
import { checkIfVocal } from "../Utils"
import SelenianFlag from '../images/selenian-flag.svg'
import EnglishFlag from '../images/english-flag.svg'
import DictionaryWord from "../components/DictionaryWord"

/**
 * Page where the user can see a virtual dictionary, sorted by alphabetical order.
 */
const Dictionary = () => {
    const [words, setWords] = useState([])
    const [wordList, setWordList] = useState([<></>])
    const [currentPage, setCurrentPage] = useState(1)
    const [wordsPerPage] = useState(10)
    const [search, setSearch] = useState("")
    const [tagSearch, setTagSearch] = useState("")
    const [searchButtonMessage, setSearchButtonMessage] = useState("Search")
    const [searchLang, setSearchLang] = useState("sf") // Sets the language that the user is searching for
    const [searchLangFlag, setSearchLangFlag] = useState(<></>) // Sets the flag of the language that the user is searching for
    const [tags, setTags] = useState([])

    const searchInput = useRef(null)
    const tagsInput = useRef(null)

    useEffect(async () => {
        let data = { sortAlphabetically: true, lang: searchLang }
        const res = await api.get("/Rootwords/GetAll", {params: data})
        setWordList(res.map(w => <DictionaryWord key={w.rootId} wordObject={w} lang={searchLang}/>))
        setSearchButtonMessage("Search")
    }, [])

    // Updates the list of words when the user searches for a word
    useEffect(() => {
        setWordList(words.map(w => <DictionaryWord key={w.rootId} wordObject={w} lang={searchLang}/>))
    }, [words])

    // Updates the flag of the language that the user is searching for
    useEffect(() => {
        setSearchLangFlag(searchLang === "sf" ? <SelenianFlag style={{width: "32px"}}/> : <EnglishFlag  style={{width: "32px"}}/>) // Sets the flag of the language that the user is searching for
    }, [searchLang])

    useEffect(() => {
        if (search === "") {
            setSearchButtonMessage("Show All Words")
        }
        else {
            setSearchButtonMessage("Search")
        }
    }, [search])

    const handlePageChange = (event) => {
        setCurrentPage(Number(event.target.id))
    }

    const handleSearch = async () => {
        if (search !== "") {
            const res = await api.get("/Rootwords/SearchWords", {
                params: { search: search, sortAlphabetically: true, lang: searchLang },
            })
            console.log(res)
            setWords(res)
        }
        else {
            const res = await api.get("/Rootwords/GetAll", {params: { sortAlphabetically: true, lang: searchLang }})
            setWords(res)
        }
    }

    const handleTagSearch = async () => {
        if (tagSearch !== "") {
            const res = await api.post("/Rootwords/SearchByTags", {
                sortAlphabetically: true, 
                exclusiveSearch: true, 
                lang: searchLang,
                tags: stringToTags(tagSearch)
            })
            setWords(res)
        }
        else {
            const res = await api.get("/Rootwords/GetAll", {params: { sortAlphabetically: true, lang: searchLang }})
            setWords(res)
        }
    }

    const handleClearSearch = async () => {
        searchInput.current.value = ""
        setSearch("")
    }

    const handleClearTags = async () => {
        tagsInput.current.value = ""
        setTagSearch("")
    }

    /**
     * Converts a string of tags into a list of tags
     * @param {*} tags The string of tags to convert
     * @returns A list of tags
     */
    const stringToTags = (tags) => {
        let tagList = []

        // If there are tags separated by commas, split them into a list, otherwise, add the tag to the list
        if (tags.includes(", ") || (tags.includes(","))) {
            tags = tags.split(", ")
            tags.forEach(tag => {
                let tempList = tag.split(",")
                if (tempList.length > 1) {
                    tempList.forEach(t => tagList.push(t))
                }
                else {
                    tagList.push(tag)
                }
            })
        }
        else {
            tagList.push(tags)
        }
        return tagList
    }

    return (
        <>
        <div className="App">
            <nav className="App-nav">
                <h1>Virtual Dictionary</h1>
                <div className={"nav-searchbar"}>
                    {/* Select to choose the language that the user is searching for */}
                    <div style={{display: "flex", position: "relative", width: "240px", left: "-5vw", flexDirection: "column", marginBottom: "16px"}}>
                        <div style={{position: "relative", left: "12px"}}>
                            <label style={{color: "white", fontSize: "1em", width: "240px"}}>Language to Search:</label>
                            <br/>
                        </div>
                        <div style={{position: "relative", left: "24px", display: "flex", alignItems: "center"}}>
                            <label style={{color: "white", fontSize: "20px", width: "120px", position: "relative", display: "flex", justifyContent: "center"}}>{searchLangFlag}</label>
                            <select 
                            value={searchLang} 
                            style={{color: "black", borderColor: "white", borderRadius: "4px", width: "80px", fontSize: "1em", right: "24px", position: "relative"}} 
                            onChange={(e) => setSearchLang(e.target.value)}>
                                <option value="sf">Selenian</option>
                                <option value="en">English</option>
                            </select>
                        </div>
                    </div>
                    <div>
                        <div className="nav-searchbar">
                            <NavInput
                                ref={searchInput}
                                placeholder="Search for a word..."
                                type="text"
                                style={{position: "relative", left: "0%"}}
                                onChange={(e) => setSearch(e.target.value)}
                            />
                            <NavButton backgroundColor={"#162F94"} transitionColor={"#004ABF"} onClick={handleSearch}>{searchButtonMessage}</NavButton>
                            <NavButton backgroundColor={"#162F94"} transitionColor={"#004ABF"} onClick={handleClearSearch}>Clear Search</NavButton>
                        </div>
                        <div className="SearchBar">
                            <NavInput
                                ref={tagsInput}
                                placeholder="Search by tags... (Separate with commas like this: tag1, tag2, tag3)"
                                type="text"
                                onChange={(e) => setTagSearch(e.target.value)}
                            />
                            <NavButton backgroundColor={"#162F94"} transitionColor={"#004ABF"} onClick={handleTagSearch}>Search</NavButton>
                            <NavButton backgroundColor={"#162F94"} transitionColor={"#004ABF"} onClick={handleClearTags}>Clear Tags</NavButton>
                        </div>
                    </div>
                </div>
                {/** 
                <div className="Pagination">
                    <ul className="PaginationList">
                        {words.length > 0 &&
                            Array.from({ length: Math.ceil(words.length / wordsPerPage) }, (_, i) => (
                                <li key={i}>
                                    <button
                                        className="PaginationButton"
                                        id={i + 1}
                                        onClick={handlePageChange}
                                    >
                                        {i + 1}
                                    </button>
                                </li>
                            ))}
                    </ul>
                </div>*/}
            </nav>
            <main className="App-main">
                { wordList.length > 0 ? 
                <div className="dictionary-content">
                    <dl style={{fontSize: "22px"}}>{wordList}</dl>
                </div>
                : <div style={{fontSize: "22px"}}>Please Wait, it can take up to a pair o minutes...</div>
                }
                {/*
                    <dl>
                    () => {
                        //const indexOfLastWord = currentPage * wordsPerPage
                        //const indexOfFirstWord = indexOfLastWord - wordsPerPage
                        //const currentWords = words.slice(indexOfFirstWord, indexOfLastWord)
                    }
                    </dl>
                */}
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

export default Dictionary