import { Component } from '@angular/core';
import {FileList} from './pages/filelist/filelist';

@Component({
  selector: 'app-root',
  imports: [FileList],
  templateUrl: './app.html',
  styleUrl: './app.scss',
  standalone: true
})
export class App {
}
