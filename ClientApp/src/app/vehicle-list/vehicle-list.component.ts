import { KeyValuePair } from './../models/vehicle';
import { VehicleService } from './../services/vehicle.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-vehicle-list',
  templateUrl: './vehicle-list.component.html',
  styleUrls: ['./vehicle-list.component.css']
})
export class VehicleListComponent implements OnInit {

  constructor( private vehicleService : VehicleService) { }
  private readonly PAGE_SIZE = 3;

  makes: KeyValuePair[]
  queryResult: any = {}
  query: any = {
    pageSize :this.PAGE_SIZE
  };
  columns = [ 
         {title:'Id'},
         {title:'ContactName', key:'contactName', isSortable: true},
         {title:'Model', key:'model', isSortable: true},
         {title:'Make', key:'make', isSortable:  true},       
         {}        
  ]


  
  ngOnInit() { 
    this.vehicleService.getMakes()
      .subscribe(makes => {this.makes = makes as KeyValuePair[]});

    this.populateVehicles();
  }

  private populateVehicles() {
    this.vehicleService.getVehicles(this.query)
      .subscribe(result => this.queryResult = result);
  }

  onFilterChange() {
    this.query.page = 1; 
    this.populateVehicles();
  }

  resetFilter() {
    this.query = {
      page: 1,
      pageSize: this.PAGE_SIZE
    };
    this.populateVehicles();
  }

  sortBy(columnName) {
    if (this.query.sortBy === columnName) {
      this.query.isSortAscending = !this.query.isSortAscending; 
    } else {
      this.query.sortBy = columnName;
      this.query.isSortAscending = true;
    }
    this.populateVehicles();
  }

  onPageChanged(page) {
    this.query.page = page; 
    this.populateVehicles();
  }
}
