import React from 'react'

import {Button, Container, Col, Row} from 'react-bootstrap'
import {Link} from 'react-router-dom'

import logo from '../Images/logo.PNG'
import mainReceta from '../Images/mainReceta.jpg'

import './CSS/Paso.css'

class Paso extends React.Component{
    render(){
        return(
            <Container className='Cont__Paso'>
            <Row><h3>Prepara los ingredientes</h3></Row>
            <Row>
                <Col>
                    <ul>
                    <li>2 litros de agua</li>
                    <li>1 pizca de sal</li>
                    <li>1 paquete de spaghetti</li>
                    <li>1 kilo de chile poblano</li>
                    <li>500 mililitros de crema</li>
                    <li>1 lata de elotes</li>
                    <li>500 mililitros de leche de vaca</li>
                    </ul>
                    <Button className='btn-Primary'>Enviar a mi correo</Button>
                </Col>
                <Col>
                    <img src={mainReceta} className='ImgPaso'/>
                </Col>
            </Row>
            </Container>
        )
    }

}

export default Paso