import { Component, AfterViewInit, Input, SimpleChanges } from '@angular/core';
import { Camera } from '../../models/camera';
import * as L from 'leaflet';

@Component({
  selector: 'app-map',
  standalone: true,
  imports: [],
  templateUrl: './map.component.html',
  styleUrl: './map.component.css'
})
export class MapComponent {
  @Input() cameras: Camera[] = [];

  private map!: L.Map

  ngAfterViewInit() {

    // Fix default icon paths
    const iconDefault = L.icon({
      iconUrl: 'assets/marker-icon.png',
      iconRetinaUrl: 'assets/marker-icon-2x.png',
      shadowUrl: 'assets/marker-shadow.png',
      iconSize: [25, 41],
      iconAnchor: [12, 41],
      popupAnchor: [1, -34],
      tooltipAnchor: [16, -28],
      shadowSize: [41, 41]
    });

    // Apply globally
    L.Marker.prototype.options.icon = iconDefault;

    this.initMap();

  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['cameras'] && this.map) {
      this.renderPins();
    }
  }

  private initMap() {
    const baseMapUrl = 'https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png'
    this.map = L.map('map').setView([52.0914, 5.1115], 14);

    L.tileLayer(baseMapUrl).addTo(this.map);
  }

  private renderPins() {
    this.cameras.forEach((cam) => {
      L.marker([cam.latitude, cam.longitude])
        .addTo(this.map)
        .bindPopup(`<b>${cam.name}</b>`);
    });
  }
}
