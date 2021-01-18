import React, { Component } from 'react';

import {Button, Container, Col, Row} from 'react-bootstrap'
import NavBar from '../Components/NavBar'
import Footer from '../Components/Footer'
import TitleReceta from '../Components/TitleReceta'
import Timer from '../Components/Timer'
import CardCalifica from '../Components/CardCalifica'

import mainFoto from '../Images/mainReceta.jpg'
import paso from '../Images/paso1.jpg'
import './CSS/RecetaCompleta.css'


class RecetaCompleta extends Component{
    constructor(props) {
        console.log("aqui Estoy");
        alert("323232");
        super(props);
        this.state = {
            receta: []};
    }

    componentDidMount() {
        this.ObtenerServidor();
    }

    static GenerarDatos(receta) {
        console.alert("Entro");
        return (
            <div>
                <NavBar />
                <TitleReceta
                    title={receta.nombre}
                    likes='30'
                    tiempo={receta.tiempoPrep}
                    user={receta.usuario.usuario}
                    descripcion='Este spaghetti preparado en una cremosa y picosita salsa de chile poblano acompañado con elote será la sensación en casa o en cualquier evento donde la prepares. Es una pasta deliciosa que tiene ingredientes muy mexicanos que no puedes dejar de preparar. ¡Inténtalo!'
                />
                <Button className='Btn-primary'>Receta guiada</Button>
                <Container className='Contenido'>
                    <Row>
                        <Col>
                            <h3>Ingredientes</h3>
                            <ul>
                                <li>2 litros de agua</li>
                                <li>1 pizca de sal</li>
                                <li>1 paquete de spaghetti</li>
                                <li>1 kilo de chile poblano</li>
                                <li>500 mililitros de crema</li>
                                <li>1 lata de elotes</li>
                                <li>500 mililitros de leche de vaca</li>
                            </ul>
                            <Button className="Btn-help"> Enviar a mi correo</Button>
                        </Col>
                        <Col>
                            <img src={mainFoto} className='mainFoto rounded' />
                        </Col>
                    </Row>
                    <hr />
                    <Row>
                        <Col>
                            <h3>Preparacion</h3>
                        </Col>
                        <Col></Col>
                    </Row>
                    <Row className='Paso'>
                        <Row className='Paso'>
                            <Col>
                                <p className='DescPaso'>
                                    1. Calentar el agua con la sal con el aceite hasta que hierva, agregar la sopa y cocinar por 15 minutos, ya que este lista escurre.
                                </p>
                            </Col>
                            <Col>
                                <img src={paso} className='Paso-Foto' />
                            </Col>
                        </Row>
                        <Row className='Paso'>
                            <p className='DescPaso'>
                                2. Poner a tostar los chiles poblanos para después quitarles la piel y las semillas.
                            </p>
                        </Row>
                        <Row className='Paso'>
                            <Col>
                                <img src={mainFoto} className='Paso-Foto' />
                            </Col>
                            <Col>
                                <Timer />
                            </Col>
                        </Row>
                        <Row className='Paso'>
                            <Col>
                                <p className='DescPaso'>
                                    3. Lorem Ipsum is simply dummy text of the printing and
                                    typesetting industry. Lorem Ipsum has been the
                                    industry's standard dummy text ever since the 1500s,
                                    when an unknown printer took a galley of type and
                                    scrambled it to make a type specimen book. It has
                                </p>
                            </Col>
                            <Col>
                                <img src={mainFoto} className='Paso-Foto' />
                            </Col>
                        </Row>
                    </Row>
                    <hr />
                    <CardCalifica />
                </Container>
                <Footer />
            </div>
        );
    }

    render() {
        let contenido = RecetaCompleta.GenerarDatos(this.state.receta);

        return (
            <div>
                <h1 id="tabelLabel" >Weather forecast</h1>
                <p>This component demonstrates fetching data from the server.</p>
                {contenido}
            </div>
        );
    }

    async ObtenerServidor() {
        console.log("Mirame");
        alert("2");
        const response = await fetch('api/RecetaSPA/ObtenerReceta/1');
        const data = await response.json();
        this.setState({ receta: data });
    }
}

export default RecetaCompleta