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