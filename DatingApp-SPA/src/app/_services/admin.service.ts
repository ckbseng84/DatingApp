import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getUsersWithRoles(){
    return this.http.get(this.baseUrl + 'admin/userswithroles');
  }
  updateUserRoles(user: User, roles: {}){
    return this.http.post(this.baseUrl + 'admin/editRoles/' + user.userName, roles);
  }
  getPhotoForModerate(){
    return this.http.get(this.baseUrl + 'admin/photosForModeration');
  }
  setPhotoToApprove(id: number){
    return this.http.post(this.baseUrl + 'admin/photosForModeration/' + id + '/setApprove', {});
  }
  SetPhotoToReject(id: number){
    return this.http.post(this.baseUrl + 'admin/photosForModeration/' + id + '/setReject', {});
  }
}
