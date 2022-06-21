import React from 'react';
import logo from './logo.svg';
import { Container, Row, Col } from 'react-bootstrap';
import { Header } from './components/Header';
import { Counter } from './features/counter/Counter';
import { Auctions } from './features/auctions/Auctions';
import { Lots } from './features/auctions/Lots';

import './App.css';

function App() {

  return (
    <div className="App">
      <Header></Header>
      <Container>
        <Row>
          <Col xs={4}>
            <Auctions />
          </Col>
          <Col xs={8}>
            <Lots/>
          </Col>
        </Row>
      </Container>

      <header className="App-header">
        <Counter />
        <p>
          Edit <code>src/App.tsx</code> and save to reload.
        </p>
      </header>
    </div>
  );
}

export default App;
