import { Component, Input, OnInit } from '@angular/core';
import { Photo } from 'src/app/_models/photo';
import { User } from 'src/app/_models/user';
import { AdminService } from 'src/app/_services/admin.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AuthService } from 'src/app/_services/auth.service';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-photo-management',
  templateUrl: './photo-management.component.html',
  styleUrls: ['./photo-management.component.css']
})
export class PhotoManagementComponent implements OnInit {
  photos: Photo[];
  constructor(private adminService: AdminService,
              private alertify: AlertifyService) {}

  ngOnInit() {
    this.getPhotosForModeration();
  }
  getPhotosForModeration(){
    this.adminService.getPhotoForModerate().subscribe((photos: Photo[]) => {
      this.photos = photos;
      console.log(photos);
    }, (error) => {
      this.alertify.error(error);
    });
  }
  approvePhoto(photo: Photo){
    this.adminService.setPhotoToApprove(photo.id).subscribe( (next) => {
          this.photos.splice(this.photos.findIndex(p => p.id === photo.id), 1);
          this.alertify.success('Photo approve');
        }, (error) => {
          this.alertify.error(error);
        }
      );
  }
  rejectPhoto(id: number) {
    this.alertify.confirm('confirm to reject?', () => {
      this.adminService.SetPhotoToReject(id)
      .subscribe(
        (next) => {
          this.photos.splice(this.photos.findIndex(p => p.id === id), 1);
          this.alertify.success('Photo rejected!');
        },
        (error) => {
          this.alertify.error(error);
          console.log(error);
        }
      );
    });
  }

}
