import { VehicleService } from './../services/vehicle.service';
import { Component, OnInit, ElementRef, ViewChild, NgZone } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PhotoService } from '../services/photo.service';
import { ProgressService } from '../services/progress.service';

@Component({
  selector: 'app-view-vehicle',
  templateUrl: './view-vehicle.component.html',
  styleUrls: ['./view-vehicle.component.css']
})
export class ViewVehicleComponent implements OnInit {

  @ViewChild('fileInput', null) fileInput: ElementRef;
  photos: any[]
  vehicleId: number;
  vehicle: any;
  progress: any;

  constructor(private vehicleService: VehicleService,
    private zone: NgZone,
    private photoService: PhotoService,
    private progressService: ProgressService,
    private route: ActivatedRoute,
    private router: Router) {

    this.route.params.subscribe(p => {
      this.vehicleId = +p['id'];
      if (isNaN(this.vehicleId) || this.vehicleId <= 0) {
        this.router.navigate(['/vehicles'])
        return;
      }

    });
  }

  ngOnInit() {
    this.vehicleService.getVehicle(this.vehicleId).subscribe(v => {

      this.vehicle = v;

      console.log("Vehicle", v)
    },
      err => {
        console.log(err);
      }
    )
  }

  delete() {
    if (confirm("Are you sure?")) {
      this.vehicleService.deleteVehicle(this.vehicle.id).subscribe(v => {
        this.router.navigate(['/vehicles'])
      })
    }
  }

  uploadPhoto() {

    this.progressService.startTracking().subscribe(progress => {
      this.zone.run( this.progress = progress) 
      console.log(progress)
    },
      null,
     () => { this.progress = null} 
    )
    var nativeElement: HTMLInputElement = this.fileInput.nativeElement;
    var file =  nativeElement.files[0];
     nativeElement.value = '';
    this.photoService.uploadPhoto(this.vehicleId, file)
      .subscribe(x => { console.log(x) });

  }

}
