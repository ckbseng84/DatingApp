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
  getPhotoForApproval(){
    return this.http.get(this.baseUrl + 'admin/photosForModeration');
  }
  approvePhoto(photoId: number){
    return this.http.post(this.baseUrl + 'admin/approvePhoto/' + photoId, {});
  }
  rejectPhoto(photoId: number){
    return this.http.post(this.baseUrl + 'admin/rejectPhoto/' + photoId, {});
  }
}
