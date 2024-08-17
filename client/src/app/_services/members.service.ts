import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Member } from '../_models/member';
import { map, of, take } from 'rxjs';
import { PaginatedResult } from '../_models/pagination';
import { UserParams } from '../_models/userParams';
import { AccountService } from './account.service';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl = environment.apiUrl;
  members: Member[] = [];
  memberCache = new Map();

  user: User | undefined;
  userParams: UserParams | undefined;
  //paginatesResult: PaginatedResult<Member[]> = new PaginatedResult<Member[]>();

  constructor(private http: HttpClient, private accountService: AccountService) {
    console.log('MembersService constructor');
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user => {
        console.log(user);
        if (user){
          console.log(user);
          this.userParams = new UserParams(user);
          this.user = user;
        }
      }
    });
   }
   getUserParams(){return this.userParams;}
   setUserParams(params: UserParams){this.userParams = params;}
   resetUserParams(){
    if(this.user){
      this.userParams = new UserParams(this.user);
      return this.userParams;
    }
    return;
   }

  getMembers(userParams: UserParams){
    const response = this.memberCache.get(Object.values(userParams).join('-'));
    if(response) return of(response);
    
    //console.log(Object.values(userParams).join('-'));
    let params = this.getPaginationHeaders(userParams.pageNumber, userParams.pageSize);
    params = params.append('minAge', userParams.minAge);
    params = params.append('maxAge', userParams.maxAge);
    params = params.append('gender', userParams.gender);
    params = params.append('orderBy', userParams.orderBy);


    return this.getPaginatedResult<Member[]>(this.baseUrl + 'user', params).pipe(
      map(response=>{
        this.memberCache.set(Object.values(userParams).join('-'), response);
        return response;
      })
    );
  }
  
  

  getMember(username: string){
    const member = [...this.memberCache.values()]
      .reduce((arr, elem) => arr.concat(elem.result), [])
      .find((member: Member) => member.userName === username);
    if(member) return of(member);
    //console.log(member);
    //console.log(this.memberCache);
    return this.http.get<Member>(this.baseUrl + 'user/' + username);
  }
  
  updateMember(member: Member){
    return this.http.put(this.baseUrl + 'user', member).pipe(
      map(() => {
        const index = this.members.indexOf(member);
        this.members[index] ={...this.members[index], ...member}; // merge the new member with the old member
      })
    );
  }

  setMainPhoto(photoId: number){
    return this.http.put(this.baseUrl + 'user/set-main-photo/' + photoId, {});
  }
  
  deletePhoto(photoId: number){ 
    return this.http.delete(this.baseUrl + 'user/delete-photo/' + photoId);
  }

  addLike(username: string){
    return this.http.post(this.baseUrl + 'likes/' + username, {});
  }

  

  getLikes(predicate: string, pageNumber: number, pageSize: number){
    let params = this.getPaginationHeaders(pageNumber, pageSize);
    params = params.append('predicate', predicate);
    return this.getPaginatedResult<Member[]>(this.baseUrl + 'likes', params);
  }
  



  private getPaginatedResult<T>(url:string, params: HttpParams) {
    const paginatesResult: PaginatedResult<T> = new PaginatedResult<T>();
    return this.http.get<T>(url, { observe: 'response', params }).pipe(
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

  private getPaginationHeaders(pageNumber:number,pageSize:number) {
    let params = new HttpParams();
   
    params = params.append('pageNumber', pageNumber);
    params = params.append('pageSize', pageSize);
    
    return params;
  }
  // getHttpOptions(){
  //   const userString = localStorage.getItem('user');
  //   if(!userString) return;
  //   const user = JSON.parse(userString);
  //   return {
  //     headers: new HttpHeaders({
  //       Authorization: 'Bearer ' + user.token
  //       })
      
  //   }
  // }

}
