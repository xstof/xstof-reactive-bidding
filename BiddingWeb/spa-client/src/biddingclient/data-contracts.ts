/* eslint-disable */
/* tslint:disable */
/*
 * ---------------------------------------------------------------
 * ## THIS FILE WAS GENERATED VIA SWAGGER-TYPESCRIPT-API        ##
 * ##                                                           ##
 * ## AUTHOR: acacode                                           ##
 * ## SOURCE: https://github.com/acacode/swagger-typescript-api ##
 * ---------------------------------------------------------------
 */

export interface Auction {
  name?: string | null;
  id?: string | null;
  category?: string | null;
}

export interface Bid {
  auctionId?: string | null;
  lotId?: string | null;
  userId?: string | null;

  /** @format double */
  biddingPrice?: number;
}

export interface Lot {
  name?: string | null;
  id?: string | null;

  /** @format double */
  price?: number;
}
