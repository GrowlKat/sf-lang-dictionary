import styled from "styled-components"

export const CustomButton = styled.button`
    width: 256px;
    height: 64px;
    font-size: 1em;
    color: white;
    margin: 4px;
    border: 2px solid ${({ borderColor }) => borderColor};
    background: ${({ backgroundColor }) => backgroundColor};
    border-radius: 10px;
    transition: background-color 0.3s ease 0s;
    &:hover {
        background: ${({ transitionColor }) => transitionColor};
    }
    &:active {
        background: #00D4C7
    }
`

export const CustomInput = styled.input`
    width: 40vw;
    height: 60px;
    font-size: 1em;
    margin: 4px;
    background: ${({ backgroundColor }) => backgroundColor};
    border-radius: 10px;
    transition: background-color 0.3s ease 0s;
    &:hover {
        background: ${({ transitionColor }) => transitionColor};
    }
`

export const NavButton = styled.button`
    width: 128px;
    height: 24px;
    font-size: 1em;
    margin: 4px;
    border: 2px solid ${({ borderColor }) => borderColor};
    background: ${({ backgroundColor }) => backgroundColor};
    border-radius: 10px;
    transition: background-color 0.3s ease 0s;
    &:hover {
        background: ${({ transitionColor }) => transitionColor};
    }

    @media (max-width: 700px) {
        width: 240px;
    }
`

export const NavInput = styled.input`
    width: 30vw;
    height: 24px;
    display: inline-block;
    color: black;
    font-size: 1em;
    margin: 4px;
    background: ${({ backgroundColor }) => backgroundColor};
    border-radius: 10px;
    transition: background-color 0.3s ease 0s;
    &:hover {
        background: ${({ transitionColor }) => transitionColor};
    }

    @media (max-width: 700px) {
        width: 80vw;
    }
`