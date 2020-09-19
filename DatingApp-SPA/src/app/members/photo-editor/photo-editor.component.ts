import { Component, Input, OnInit } from '@angular/core';
import { Photo } from 'src/app/_models/photo';
import { FileUploader } from 'ng2-file-upload';
import { environment } from 'src/environments/environment';
import { AuthService } from 'src/app/_services/auth.service';

// const URL = '/api/';
const URL = 'https://evening-anchorage-3159.herokuapp.com/api/';
@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit {
  @Input() photos: Photo[];
  uploader: FileUploader;
  hasBaseDropZoneOver = false;
  response: string;
  baseUrl = environment.apiUrl; // todo what is environment.apiUrl at here

 constructor(private authService: AuthService){
    this.uploader = new FileUploader({
      url: this.baseUrl + 'users/' + this.authService.decodedToken.nameid + '/photos',
      authToken: 'bearer ' + localStorage.getItem('token'),
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024,
      // disableMultipart: true, // 'DisableMultipart' must be 'true' for formatDataFunction to be called.
      // formatDataFunctionIsAsync: true,
      // formatDataFunction: async (item) => {
      //   return new Promise( (resolve, reject) => {
      //     resolve({
      //       name: item._file.name,
      //       length: item._file.size,
      //       contentType: item._file.type,
      //       date: new Date()
      //     });
      //   });
     // }
    });

    this.hasBaseDropZoneOver = false;

    this.response = '';
    this.uploader.onAfterAddingFile = (file) => {file.withCredentials = false; };
    this.uploader.onSuccessItem = (item, response, status, header) => {
      if (response){
        const res: Photo = JSON.parse(response);
        const photo = {
          id: res.id,
          url: res.url,
          dateAdded: res.dateAdded,
          description: res.description,
          isMain: res.isMain
        };
        this.photos.push(photo);
      }
    };
    this.uploader.response.subscribe( res => this.response = res );
  }

  ngOnInit() {
  }
  public fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }
}
