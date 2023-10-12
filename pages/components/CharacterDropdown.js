import React, { useEffect, useState } from 'react'
import { CustomButton } from './CustomComponents'

/**
 * Dropdown menu that shows special characters to be used in search bar
 * @param {*} dropdownOpen Sets if the dropdown menu should be shown or not
 * @param {*} handleClick Return the search value to it's parent component
 * @param {*} searchValue The search value that should be returned to it's parent component
 * @param {*} inputRef Reference to the input element, used to focus on it when the user clicks on a character button
 */
function CharacterDropdown({dropdownOpen, handleClick, searchValue, inputRef}) {
  const [isDropdownOpen, setIsDropdownOpen] = useState(false) // Sets if the dropdown menu should be shown or not
  const [specialCharacters, setSpecialCharacters] = useState(['']) // The special characters that should be shown in dropdown menu

  /**
   * Return the search value to it's parent component
   * @param {*} character The new character to be added search value
   * @param {*} e Event that triggered the function
   */
  const handleCharacterClick = (character, e) => {
    e.preventDefault() // Prevents reloading the page
    handleClick(searchValue + character) // Return the search value with the character selected to it's parent component
    inputRef.current.focus() // Focus on the input element
  };

  // Update the isDropdownOpen state and set the special characters that should be shown in dropdown menu
  useEffect(() => {
    setIsDropdownOpen(dropdownOpen)
    setSpecialCharacters(['ā', 'Ā', 'ē', 'Ē', 'ī', 'Ī', 'ō', 'Ō', 'ū', 'Ū', 'ä', 'Ä', 'ö', 'Ö', 'ü', 'Ü', 'þ', 'Þ', 'ð', 'Ð'])
  }, [dropdownOpen])

  return (
    <div className="dropdown-search-bar">
      {isDropdownOpen && (
        <div className="character-menu">
          {
          // Map the special characters to be shown in dropdown menu
          specialCharacters && specialCharacters.map((character) => (
            <CustomButton 
              borderColor={"black"} 
              backgroundColor={"#162F94"} 
              transitionColor={"#004ABF"}
              key={character}
              type="button"
              onClick={(e) => handleCharacterClick(character, e)}
              style={{width: "42px", height: "36px", display: "inline-flex", alignItems: "center", justifyContent: "center", alignContent: "center", position: "relative", top: "32px", bottom: "8px", fontSize: "20px"}}
              tabIndex="-1" >
              { character }
            </CustomButton>
          ))}
        </div>
      )}
    </div>
  );
};

export default CharacterDropdown;