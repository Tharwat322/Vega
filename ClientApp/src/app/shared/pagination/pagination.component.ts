import { Component, OnChanges, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'pagination',
  templateUrl: './pagination.component.html'
        
})
export class PaginationComponent implements OnChanges {

@Input('total-items') totalItems; 
@Input('page-size') pageSize ;
@Output('page-changed') pageChanged = new EventEmitter();
pages:any[]
currentPage = 1;

  constructor() { }

  ngOnChanges() {
    
      this.currentPage = 1;
      var pagesCount = Math.ceil(this.totalItems/this.pageSize)
      this.pages = []

      for(var i = 1; i <= pagesCount; i++){
           this.pages.push(i);
      } 
      console.log(this)
  }

  changePage(page){
    
    this.currentPage = page;
    this.pageChanged.emit(page);

    console.log(page)
  }

  previous(){
  
    if(this.currentPage == 1)
       return ;
    this.currentPage--;
    this.pageChanged.emit(this.currentPage);
    console.log(this)
  }

  next(){

     if(this.currentPage == this.pages.length)
         return ;
      this.currentPage++;
      this.pageChanged.emit(this.currentPage);

      console.log("Next",this)
  }

}
