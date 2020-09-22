
import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';




@Injectable({providedIn: 'root'})
export class ProgressService {

  private uploadProgress: Subject<any> 
    downloadProgress: Subject<any> = new Subject<any>();

    constructor()  { }

 startTracking(){
     this.uploadProgress = new Subject();
     return this.uploadProgress;
 }

 notify(progress){
    this.uploadProgress.next(progress);
 }

 endTraking(){
     this.uploadProgress.complete();
 }
    
    
}

@Injectable({providedIn: 'root'})

export class BrowserXhrWithProgress extends XMLHttpRequest{

    constructor( private service: ProgressService)  { super()};
    
    build(): XMLHttpRequest {
       
        var xhr: XMLHttpRequest ;
        xhr.onprogress = (event) =>{
             this.service.downloadProgress.next(this.createProgress(event))
        };

        xhr.upload.onprogress = (event) =>{
            this.service.notify(this.createProgress(event))
       };

         xhr.upload.onloadend = ()=>{
             this.service.endTraking();
         }
       return xhr;
    }

    private createProgress (event){
        return { total: event.total,
                 percentage: Math.round(event.loaded/event.total * 100)
         }

     }
}
