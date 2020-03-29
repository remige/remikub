import { FetcherResponse } from "./fetcher-response";
import { isObject } from "lodash-es";
import { userContext } from "../context/user-context";
import i18next from "i18next";
import { IFetcherError } from "./ifetcher-error";

interface IRequestConfig {
    method: HttpMethodType;
    url: string;
    data?: {};
    failsOnUnauthorized?: boolean;
}

export type HttpMethodType = "GET" | "POST" | "PUT" | "PATCH" | "DELETE";

const ContentTypeHeader = "Content-Type";
const AcceptLanguageHeader = "Accept-Language";
const apiVersion = "v1";

class Fetcher {
    public async httpGet<T>(url: string): Promise<T> {
        return await this.callFetch({ method: "GET", url });
    }

    public async httpPost<T>(url: string, data?: {}): Promise<T> {
        return await this.callFetch({ method: "POST", url, data });
    }

    public async httpPut<T>(url: string, data?: {}): Promise<T> {
        return await this.callFetch({ method: "PUT", url, data });
    }

    private async callFetch(requestConfig: IRequestConfig) {
        const response = await this.fetch(requestConfig);
        if (response.succeeded) {
            return response.value;
        } else {
            alert(response.error.code);
        }
    }

    private async fetch(requestConfig: IRequestConfig): Promise<FetcherResponse<any>> {
        const { method, url, data, failsOnUnauthorized } = requestConfig;

        const body = data ? JSON.stringify(data) : undefined;

        const headers: { [key: string]: string } = {};
        if (data) {
            headers[ContentTypeHeader] = isObject(data) ? "application/json" : "text/plain";
        }
        if (userContext.locale) {
            headers[AcceptLanguageHeader] = userContext.locale;
        }

        try {
            const request = await fetch(`/api/${apiVersion}` + url, { method, body, headers, credentials: "include" });

            if (request.status === 204) {
                // no data to parse
                return new FetcherResponse(true);
            } else if ((request.status === 401 || request.status === 403) && !failsOnUnauthorized) {
                throw new Error(i18next.t("error.general.unauthorized"));
            } else if (request.status === 404) {
                throw new Error(i18next.t("error.general.notFound"));
            } else if (request.status >= 500) {
                return FetcherResponse.Failed(request.status, { code: "error.general.unexpectedError" });
            } else if (request.status >= 400) {
                const error = await this.parseTextResponseAsync(request) as IFetcherError;
                return FetcherResponse.Failed(request.status, error);
            } else {
                return new FetcherResponse(await this.parseTextResponseAsync(request));
            }
        } catch (error) {
            return FetcherResponse.Failed(500, error.message);
        }
    }

    private async parseTextResponseAsync(response: Response) {
        const result = await response.text();
        if (!result) {
            return undefined;
        }
        if (this.isJsonResponse(response)) {
            return JSON.parse(result);
        }
        return result;
    }

    private isJsonResponse(response: Response) {
        const contentType = response.headers.get(ContentTypeHeader);
        return contentType && contentType.indexOf("application/json") !== -1;
    }
}

export const fetcher = new Fetcher();
