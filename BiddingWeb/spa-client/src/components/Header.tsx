import React from 'react';
import {Navbar} from 'react-bootstrap';

export function Header() {
    return (
        <Navbar bg="dark" variant="dark" expand="lg">
            <Navbar.Brand><span className='p-3'>Bidding Buddy</span></Navbar.Brand>
        </Navbar>      
    );
}
