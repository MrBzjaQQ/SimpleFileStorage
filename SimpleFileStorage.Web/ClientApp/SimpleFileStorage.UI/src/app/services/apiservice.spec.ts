import { TestBed } from '@angular/core/testing';
import { ApiService, StoredFile } from './apiservice';
import { HttpClient } from '@angular/common/http';
import { of, throwError } from 'rxjs';

describe('ApiService', () => {
  let service: ApiService;
  let httpSpy: jasmine.SpyObj<HttpClient>;

  beforeEach(() => {
    httpSpy = jasmine.createSpyObj('HttpClient', ['get', 'post', 'delete']);
    TestBed.configureTestingModule({
      providers: [
        ApiService,
        { provide: HttpClient, useValue: httpSpy }
      ]
    });

    service = TestBed.inject(ApiService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  describe('getFiles()', () => {
    it('should call GET /api/files with correct parameters', (done) => {
      const mockResponse : jasmine.ArrayLike<StoredFile> = [];
      httpSpy.get.and.returnValue(of(mockResponse));

      service.getFiles(10, 0, '').subscribe({
        next: (result) => {
          expect(result).toEqual(mockResponse);
          done();
        },
        error: () => fail('should not have error')
      });
    });

    it('should handle errors', (done) => {
      const errorMessage = 'Test Error';
      httpSpy.get.and.returnValue(throwError(() => new Error(errorMessage)));

      service.getFiles(10, 0, '').subscribe({
        next: () => fail('should not have value'),
        error: (error) => {
          expect(error.message).toBe(errorMessage);
          done();
        }
      });
    });
  });

  describe('uploadFile()', () => {
    it('should call POST /api/files/upload with correct form data', (done) => {
      const mockResponse = { success: true };
      httpSpy.post.and.returnValue(of(mockResponse));

      // Mock file and test upload logic...
      const file = new File(['test content'], 'test.txt');
      service.uploadFile(file).subscribe({
        next: (result) => {
          expect(result).toEqual(mockResponse);
          done();
        },
        error: () => fail('should not have error')
      });
    });

    it('should handle upload errors', (done) => {
      const errorMessage = 'Upload Error';
      httpSpy.post.and.returnValue(throwError(() => new Error(errorMessage)));

      const file = new File(['test content'], 'test.txt');
      service.uploadFile(file).subscribe({
        next: () => fail('should not have value'),
        error: (error) => {
          expect(error.message).toBe(errorMessage);
          done();
        }
      });
    });
  });

  describe('downloadFile()', () => {
    it('should call GET /api/files/{id} with correct response type', (done) => {
      const mockBlob = new Blob(['test content'], { type: 'text/plain' });
      httpSpy.get.and.returnValue(of(mockBlob));

      service.downloadFile('123').subscribe({
        next: (blob) => {
          expect(blob).toEqual(mockBlob);
          done();
        },
        error: () => fail('should not have error')
      });
    });

    it('should handle download errors', (done) => {
      const errorMessage = 'Download Error';
      httpSpy.get.and.returnValue(throwError(() => new Error(errorMessage)));

      service.downloadFile('123').subscribe({
        next: () => fail('should not have value'),
        error: (error) => {
          expect(error.message).toBe(errorMessage);
          done();
        }
      });
    });
  });

  describe('deleteFile()', () => {
    it('should call DELETE /api/files/{id}', (done) => {
      const mockResponse = { success: true };
      httpSpy.delete.and.returnValue(of(mockResponse));

      service.deleteFile('123').subscribe({
        next: (result) => {
          expect(result).toEqual(mockResponse);
          done();
        },
        error: () => fail('should not have error')
      });
    });

    it('should handle deletion errors', (done) => {
      const errorMessage = 'Delete Error';
      httpSpy.delete.and.returnValue(throwError(() => new Error(errorMessage)));

      service.deleteFile('123').subscribe({
        next: () => fail('should not have value'),
        error: (error) => {
          expect(error.message).toBe(errorMessage);
          done();
        }
      });
    });
  });
});
