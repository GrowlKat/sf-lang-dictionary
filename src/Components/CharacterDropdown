import React, { useEffect, useState } from 'react'

function CharacterDropdown({dropdownOpen}) {
  const [isDropdownOpen, setIsDropdownOpen] = useState(false)
  const specialCharacters = ['@', '#', '$', '%', '&', '*']

  const handleCharacterClick = (character) => {
    console.log(searchValue + character)
  };

  useEffect(() => {
    setIsDropdownOpen(dropdownOpen)
  }, [dropdownOpen])

  return (
    <div className="dropdown-search-bar">
      {isDropdownOpen && (
        <div className="character-menu">
          {specialCharacters.map((character) => (
            <button
              key={character}
              onClick={() => handleCharacterClick(character)}
            >
              {character}
            </button>
          ))}
        </div>
      )}
    </div>
  );
};

export default CharacterDropdown;
