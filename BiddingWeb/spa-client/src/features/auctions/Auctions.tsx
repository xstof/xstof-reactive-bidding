import React, {Fragment, useState} from 'react';
import { useAppSelector, useAppDispatch } from '../../app/hooks';
import { selectAuction } from '../auctions/auctionsSlice';

import { Card } from 'react-bootstrap';

export function Auctions(){

    const auctions = useAppSelector(state => state.auctions);
    const dispatch = useAppDispatch();

    const isSelected = (auctionId:string) => {
        if(auctionId === auctions.selectedAuctionId) return true;
        return false;
    };

    const auctionElements = auctions.auctions.map(auction => 
        <Card className='m-2' 
              onClick={() => dispatch(selectAuction(auction.id))}
              bg={isSelected(auction.id) ? 'secondary' : 'light'}
              text={isSelected(auction.id) ? 'white' : 'dark'}>
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