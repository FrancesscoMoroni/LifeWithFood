import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public tags: Tag[] = [];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<Tag[]>(baseUrl + 'weatherforecast').subscribe(result => {
      this.tags = result;
    }, error => console.error(error));
  }
}

interface Tag {
  IdTag: number;
  name: string;
}
