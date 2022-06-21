import React, {Fragment, useState} from 'react';
import { useAppSelector } from '../../app/hooks';

import { Card, CardGroup } from 'react-bootstrap';

export function Auctions(){

    const auctions = useAppSelector(state => state.auctions);
    const auctionElements = auctions.auctions.map(auction => 
        <Card className='m-2'>
            <Card.Header>Auction</Card.Header>
            <Card.Body>
                <Card.Title as="h5">{auction.name}</Card.Title>
                <Card.Text>Category: {auction.category} </Card.Text>
            </Card.Body>
        </Card>    
    );

    return (
        <Fragment>
            {auctionElements}
        </Fragment>
    );
}