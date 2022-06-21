import React from "react";
import { useAppSelector } from "../../app/hooks";

import { Table } from 'react-bootstrap';

export function Lots(){

    const selectedAuction = useAppSelector(state => state.auctions.auctions.find(a => a.id == state.auctions.selectedAuctionId))

    return (
        <Table responsive>
            <thead>
                <tr>
                    <th>ID</th>
                    <th>Name</th>
                    <th>Price</th>
                    <th>Bid Now</th>
                </tr>
            </thead>
            <tbody>
                {selectedAuction?.lots.map(lot => {
                    return (<tr>
                        <th>{lot.id}</th>
                        <th>{lot.name}</th>
                        <th>{lot.price}</th>
                        <th>TODO</th>
                    </tr>)
                })}
            </tbody>
        </Table>
    );
}