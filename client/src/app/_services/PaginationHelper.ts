import { HttpClient, HttpParams } from "@angular/common/http";
import { map } from "rxjs";
import { PaginatedResult } from "../_models/pagination";

export function getPaginatedResult<T>(url:string, params: HttpParams, http: HttpClient) {
    const paginatesResult: PaginatedResult<T> = new PaginatedResult<T>();
    return http.get<T>(url, { observe: 'response', params }).pipe(
      map(response => {
        if (response.body) {
          paginatesResult.result = response.body;
        }
        const pagination = response.headers.get('Pagination');
        if (pagination) {
          paginatesResult.pagination = JSON.parse(pagination);
        }
        return paginatesResult;
      })

    );
  }

  export function getPaginationHeaders(pageNumber:number,pageSize:number) {
    let params = new HttpParams();
   
    params = params.append('pageNumber', pageNumber);
    params = params.append('pageSize', pageSize);
    
    return params;
  }