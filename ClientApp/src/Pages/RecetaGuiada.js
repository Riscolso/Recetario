import React from 'react'

import StepProgressBar from 'react-step-progress';
import {Container, Col, Row} from 'react-bootstrap'
import {Link} from 'react-router-dom'

import Paso from '../Components/Paso'
import Footer from '../Components/Footer'

import 'react-step-progress/dist/index.css';
import logo from '../Images/logo.PNG'
 
const ings = <Paso/>;

function onFormSubmit() {
  // handle the submit logic here
  // This function will be executed at the last step
  // when the submit button (next button in the previous steps) is pressed
}

class RecetaGuiada extends React.Component{
    render(){
        return(
            <Container>
                <Link to='/'>
                    <img src={logo} className='Logo rounded'/>
                </Link>
                <StepProgressBar
                    startingStep={0}
                    onSubmit={onFormSubmit}
                    previousBtnName='Paso anterior'
                    nextBtnName='Paso siguiente'
                    primaryBtnClass = 'btn-Primary'
                    steps={[
                        {
                            label: 'Ingredientes',
                            name: 'Ingredientes',
                            content: ings
                        },
                        {
                            label: 'Paso 1',
                            name: 'Paso 1',
                            content: ings
                        },
                        {
                            label: 'Paso 2',
                            name: 'Paso 2',
                            content: ings
                        },
                        {
                            label: 'Paso 3',
                            name: 'Paso 3',
                            content: ings
                        },
                        {
                            label: '¡A disfrutar!',
                            name: 'Final',
                            content: ings
                        }
                    ]}
                />
                <Footer/>
            </Container>
        )
    }
}

export default RecetaGuiada