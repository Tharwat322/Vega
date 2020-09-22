

import { Observable, forkJoin } from 'rxjs';
import { VehicleService } from '../services/vehicle.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SaveVehicle, Vehicle, KeyValuePair } from '../models/vehicle';



@Component({
  selector: 'app-vehicle-form',
  templateUrl: './vehicle-form.component.html',
  styleUrls: ['./vehicle-form.component.css']
})
export class VehicleFormComponent implements OnInit {

  makes: any[];
  vehicle: SaveVehicle = {
    id: 0,
    makeId: 0,
    modelId: 0,
    isRegistered: false,
    features: [],
    contact: {
      name: '',
      phone: '',
      email: ''
    }
  };

  models: any[];
  features: any[];


  constructor(private vehicleService: VehicleService,
    private route: ActivatedRoute,
    private router: Router
  ) {

    this.route.params.subscribe(p => {
      this.vehicle.id = +p['id'] || 0
    })
  }

  ngOnInit() {

    let source = [
      this.vehicleService.getMakes(),
      this.vehicleService.getFeatures(),
    ];

    if (this.vehicle.id) {
      source.push(this.vehicleService.getVehicle(this.vehicle.id))
    }

    let parallelRequest = forkJoin(source);

    parallelRequest.subscribe(data => {
      this.makes = data[0] as [];
      this.features = data[1] as [];

      if (this.vehicle.id) {
        this.setVehicle(data[2] as Vehicle);
        this.populateModels();
      }
    },
      err => {
        if (err.status == 404) {
          this.router.navigate(['/home'])
        }
      });
  }

  private setVehicle(v: Vehicle) {
    this.vehicle.id = v.id;
    this.vehicle.makeId = v.make.id;
    this.vehicle.modelId = v.model.id;
    this.vehicle.isRegistered = v.isRegistered;
    this.vehicle.contact = v.contact;
    this.vehicle.features = this.mapFeatures(v.features, "id");

  }

  onMakeChange() {
    this.populateModels();
    delete this.vehicle.modelId;
  }

  private populateModels() {
    let selectedMake = this.makes.find(m => m.id == this.vehicle.makeId);
    this.models = selectedMake ? selectedMake.models : [];
  }

  onFeatureToggle(featureId, $event) {
    if ($event.target.checked)
      this.vehicle.features.push(featureId);
    else {
      var index = this.vehicle.features.indexOf(featureId);
      this.vehicle.features.splice(index, 1);
    }
  }

  submit() {

    var $result = (this.vehicle.id)? this.vehicleService.updateVehicle(this.vehicle) : this.vehicleService.createVehicle(this.vehicle)
        $result.subscribe(vehicle => {
          alert("Data was saved successfully")
          this.router.navigate(['/vehicles/', this.vehicle.id])
        });      
  }

  delete() {
    if (confirm("Are you sure to delete this?"))
      this.vehicleService.deleteVehicle(this.vehicle.id).subscribe(res => {
        alert("Vehicle is deleted Successfully");
        this.router.navigate(['/home'])
      });
  }

  private mapFeatures(features: any, id: string) {
    let result: []
    result = features.map((i: { id: any; }) => i.id)
    return result;
  }

}
