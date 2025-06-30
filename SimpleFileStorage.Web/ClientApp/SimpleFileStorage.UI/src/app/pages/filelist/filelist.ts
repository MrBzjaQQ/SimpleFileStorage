import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import {ApiService, StoredFile} from '../../services/apiservice.js';
import { saveAs } from 'file-saver';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'filelist',
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './filelist.html',
  styleUrl: './filelist.scss',
  standalone: true
})
export class FileList implements OnInit {
  files: any[] = [];
  searchTerm = '';
  take = 10;
  skip = 0;
  totalLoaded = 0;
  @ViewChild('fileInput') fileInput!: ElementRef<HTMLInputElement>;

  constructor(private apiService: ApiService) {}

  ngOnInit(): void {
    this.loadFiles();
  }

  loadFiles(): void {
    this.apiService.getFiles(this.take, this.skip, this.searchTerm).subscribe(data => {
        this.files = data;
        this.totalLoaded = data.length;
    });
  }

  downloadFile(file: StoredFile): void {
    this.apiService.downloadFile(file.id).subscribe(blob => {
      saveAs(blob, file.fileName);
    });
  }

  deleteFile(id: string): void {
    this.apiService.deleteFile(id).subscribe(() => {
      this.loadFiles();
    });
  }

  nextPage(): void {
    this.skip += this.take;
    this.loadFiles();
  }

  prevPage(): void {
    if (this.skip >= this.take) {
      this.skip -= this.take;
      this.loadFiles();
    }
  }

  onSearch(): void {
    this.skip = 0;
    this.loadFiles();
  }

  onUpload(): void {
    const file = this.fileInput.nativeElement.files?.[0];
    if (!file) return;
    this.apiService.uploadFile(file).subscribe(() => {
      this.loadFiles();
      this.fileInput.nativeElement.value = '';
    });
  }
}
