import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface StoredFile { id: string, fileName: string, uploadedAt: Date }

@Injectable({ providedIn: 'root' })
export class ApiService {
  constructor(private http: HttpClient) {}

  getFiles(take: number, skip: number, searchTerm: string): Observable<StoredFile[]> {
    let params = new HttpParams()
      .set('take', take.toString())
      .set('skip', skip.toString())
      .set('searchTerm', searchTerm);
    return this.http.get<any[]>('/api/files', { params });
  }

  uploadFile(file: File): Observable<any> {
    const formData = new FormData();
    formData.append('file', file);
    formData.append('fileName', file.name);
    return this.http.post('/api/files/upload', formData);
  }

  downloadFile(id: string): Observable<Blob> {
    return this.http.get(`/api/files/${id}`, { responseType: 'blob' });
  }

  deleteFile(id: string): Observable<any> {
    return this.http.delete(`/api/files/${id}`);
  }
}
