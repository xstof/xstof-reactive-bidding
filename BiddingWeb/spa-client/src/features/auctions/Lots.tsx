import React from "react";
import { useAppSelector } from "../../app/hooks";

import { Table, Button, ButtonGroup } from 'react-bootstrap';

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
                    return (<tr key={lot.id}>
                        <th>{lot.id}</th>
                        <th>{lot.name}</th>
                        <th>{lot.price}</th>
                        <th>
                        <ButtonGroup size="sm" className="mb-2">
                            <Button>Bid {lot.price + 1}$</Button>
                            <Button>Bid {lot.price + 2}$</Button>
                            <Button>Bid {lot.price + 5}$</Button>
                        </ButtonGroup>
                        </th>
                    </tr>)
                })}
            </tbody>
        </Table>
    );
}