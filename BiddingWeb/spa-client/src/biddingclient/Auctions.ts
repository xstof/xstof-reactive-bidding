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

import { Auction, Lot } from "./data-contracts";
import { HttpClient, RequestParams } from "./http-client";

export class Auctions<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags Auctions
   * @name GetAuctions
   * @request GET:/Auctions
   */
  getAuctions = (params: RequestParams = {}) =>
    this.request<Auction[], any>({
      path: `/Auctions`,
      method: "GET",
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags Auctions
   * @name GetAuction
   * @request GET:/Auctions/{auctionId}
   */
  getAuction = (auctionId: string, params: RequestParams = {}) =>
    this.request<Auction, any>({
      path: `/Auctions/${auctionId}`,
      method: "GET",
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags Auctions
   * @name GetAuctionLots
   * @request GET:/Auctions/{auctionId}/lots
   */
  getAuctionLots = (auctionId: string, params: RequestParams = {}) =>
    this.request<Lot[], any>({
      path: `/Auctions/${auctionId}/lots`,
      method: "GET",
      format: "json",
      ...params,
    });
}
