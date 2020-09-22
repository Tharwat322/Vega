import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({providedIn: 'root'})

export class PhotoService {

    constructor( private http: HttpClient) { }




  getPhotos(vehicleId){

   return this.http.get(`/api/vehicles/${vehicleId}/photos`)
  }    

  uploadPhoto( vehicleId, file){
      
    let formData: FormData
    formData.append('file', file);
     return this.http.post(`/api/vehicles/${vehicleId}/photos`, formData)

  }
    
}