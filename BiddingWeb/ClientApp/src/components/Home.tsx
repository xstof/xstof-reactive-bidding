import * as React from 'react';
import { connect } from 'react-redux';

const Home = () => (
  <div>
    <p>Welcome to your new single-page application, built with React</p>
  </div>
);

export default connect()(Home);
