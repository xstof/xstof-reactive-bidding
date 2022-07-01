import React, {useEffect} from 'react';
import { useAppSelector, useAppDispatch } from './app/hooks';

import logo from './logo.svg';
import { Container, Row, Col } from 'react-bootstrap';
import { Header } from './components/Header';
import { Counter } from './features/counter/Counter';
import { Auctions } from './features/auctions/Auctions';
import { Lots } from './features/auctions/Lots';

import { configLoaded, fetchConfig } from './features/config/configSlice';

import './App.css';

function App() {

  const config = useAppSelector(state => state.config);
  const configStatus = useAppSelector(state => state.config.status);

  const dispatch = useAppDispatch();

  useEffect(() => {
    if(configStatus === 'idle'){
        dispatch(fetchConfig());
    }
}, [dispatch, configStatus]);

  if(configStatus === 'idle' || configStatus === 'loading'){
    return (
      <div className="App">
        <Header></Header>
        <Container>
          <Row>
            <Col>
              ... loading configuration ...
            </Col>
          </Row>
        </Container>
      </div>
    );
  }

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
