import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { MapComponent } from './components/map/map.component';
import { CameraTablesComponent } from './components/camera-tables/camera-tables.component';
import { Camera } from './models/camera';
import { CameraService } from './services/camera.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, MapComponent, CameraTablesComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  title = 'everybody-ng';
  cameras: Camera[] = [];

  constructor(private cameraService: CameraService) {}
  
  ngOnInit(): void {
    this.cameraService.getCameras().subscribe({
      next: (cams) => (this.cameras = cams)
    });
  }
}
