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

import { Bid } from "./data-contracts";
import { ContentType, HttpClient, RequestParams } from "./http-client";

export class Bids<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags Bids
   * @name PlaceBid
   * @request POST:/Bids
   */
  placeBid = (data: Bid, params: RequestParams = {}) =>
    this.request<void, any>({
      path: `/Bids`,
      method: "POST",
      body: data,
      type: ContentType.Json,
      ...params,
    });
}
