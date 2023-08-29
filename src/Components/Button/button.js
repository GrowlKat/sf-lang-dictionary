import styled from "styled-components"

export const CustomButton = styled.button`
    width: 200px;
    height: 50px;
    font-size: 1em;
    border: 4px solid ${({ borderColor }) => borderColor};
    /*background: linear-gradient(180deg, ${({ backgroundColor1 }) => backgroundColor1} 0%, ${({ backgroundColor2 }) => backgroundColor2} 100%);*/
    background: ${({ backgroundColor }) => backgroundColor};
    opacity: 1;
    transition: background-color 0.5s ease 0s;
    &:hover {
        background: ${({ transitionColor }) => transitionColor};
    }
`