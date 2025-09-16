import { Component, Input } from '@angular/core';
import { Camera } from '../../models/camera';
import { NgFor } from '@angular/common';

@Component({
  selector: 'app-camera-tables',
  standalone: true,
  imports: [NgFor],
  templateUrl: './camera-tables.component.html',
  styleUrl: './camera-tables.component.css'
})
export class CameraTablesComponent {
@Input() cameras: Camera[] = [];

  get column3() {
    return this.cameras.filter((c) => c.number % 3 === 0 && c.number % 15 !== 0);
  }

  get column5() {
    return this.cameras.filter((c) => c.number % 5 === 0 && c.number % 15 !== 0);
  }

  get column15() {
    return this.cameras.filter((c) => c.number % 15 === 0);
  }

  get columnOther() {
    return this.cameras.filter((c) => c.number % 3 !== 0 && c.number % 5 !== 0);
  }
}
