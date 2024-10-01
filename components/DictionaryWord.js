import { useEffect, useState } from "react"
import { capitalize, Cases, Numbers } from "../Utils"
import { BiSolidDownArrow } from "react-icons/bi"
import "./DictionaryWord.css"

/**
 * Render a component that show a word info in the dictionary (Meant to be used in an `<dl>` element)
 */
const DictionaryWord = ({wordObject, lang}) => {
    return (
        <div>
            { lang === "sf" ? (
            <>
            <dt>{capitalize(wordObject.rootword1)}</dt>
            <dd>
                <div><b>Meaning:</b> {wordObject.meaning}</div>
                <div><b>Pronunciation:</b> {wordObject.pronunciation}</div>
                <div><b>Tags:</b> {wordObject.tags}</div>
            </dd>
            </>
            )
            : (
            <>
            <dt>{wordObject.meaning}</dt>
            <dd>
                <div><b>Translation:</b> {capitalize(wordObject.rootword1)}<br/></div>
                <div><b>Pronunciation:</b> {wordObject.pronunciation}<br/></div>
                <div><b>Tags:</b> {wordObject.tags}</div>
            </dd>
            </>
            )
            }
        </div>
    )
}

export default DictionaryWord