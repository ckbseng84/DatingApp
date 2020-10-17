import { Component, OnInit } from '@angular/core';
import { AdminService } from 'src/app/_services/admin.service';
import { AlertifyService } from 'src/app/_services/alertify.service';

@Component({
  selector: 'app-photo-management',
  templateUrl: './photo-management.component.html',
  styleUrls: ['./photo-management.component.css']
})
export class PhotoManagementComponent implements OnInit {
  photos: any;
  constructor(private adminService: AdminService,
              private alertify: AlertifyService) { }

  ngOnInit() {
    this.getPhotosForApproval();
  }
  getPhotosForApproval(){
    this.adminService.getPhotoForApproval().subscribe((photos) => {
      this.photos = photos;
    }, (error) => {
      console.log(error);
      this.alertify.error(error);
    });
  }
  approvePhoto(photoId: number){
    this.adminService.approvePhoto(photoId).subscribe(() => {
      this.photos.splice(this.photos.findIndex(p => p.id === photoId), 1);
      this.alertify.success('Photo approved');
    }, (error) => {
      this.alertify.error(error);
    });
  }
  rejectPhoto(photoId: number){
    this.adminService.approvePhoto(photoId).subscribe(() => {
      this.photos.splice(this.photos.findIndex(p => p.id === photoId), 1);
      this.alertify.success('Photo rejected');
    }, (error) => {
      this.alertify.error(error);
    });
  }

}
