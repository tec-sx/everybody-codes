import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { Camera } from '../models/camera';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class CameraService {

  private apiUrl = 'https://localhost:7006/camera';

  constructor(private http: HttpClient) {}
  
  getCameras(): Observable<Camera[]> {
    return this.http.get<Camera[]>(this.apiUrl);
  }
}
