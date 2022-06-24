import React, {Fragment, useState, useEffect} from 'react';
import { useAppSelector, useAppDispatch } from '../../app/hooks';
import { auctionSelected, fetchAuctions } from '../auctions/auctionsSlice';

import { Card, Badge } from 'react-bootstrap';
import { useSelector } from 'react-redux';

export function Auctions(){

    const auctions = useAppSelector(state => state.auctions);    
    const status = useAppSelector(state => state.auctions.status);

    const dispatch = useAppDispatch();

    const isSelected = (auctionId:string) => {
        if(auctionId === auctions.selectedAuctionId) return true;
        return false;
    };

    useEffect(() => {
        if(status === 'idle'){
            dispatch(fetchAuctions());
        }
    }, [dispatch, status]);

    const auctionElements = auctions.auctions.map(auction => 
        <Card className='m-2'
              key={auction.id}
              onClick={() => dispatch(auctionSelected(auction.id))}
              bg={isSelected(auction.id) ? 'secondary' : 'light'}
              text={isSelected(auction.id) ? 'white' : 'dark'}>
            <Card.Header>Auction - {auction.lots.length} Lots]</Card.Header>
            <Card.Body>
                <Card.Title as="h5">{auction.name}</Card.Title>
                <Card.Text>Category: {auction.category} </Card.Text>
            </Card.Body>
            <Card.Footer>
                <Badge className='float-right' bg='primary' pill>12</Badge> <span>people active</span>
            </Card.Footer>
        </Card>    
    );

    return (
        <Fragment>
            {auctionElements}
        </Fragment>
    );
}