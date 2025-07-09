import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FileList } from './filelist';
import { ApiService, StoredFile } from '../../services/apiservice';
import { FormsModule } from '@angular/forms';
import { of } from 'rxjs';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { ElementRef } from '@angular/core';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { setSaveAs } from '../../utils/file-saver-wrapper';

describe('FileList', () => {
  let component: FileList;
  let fixture: ComponentFixture<FileList>;
  let apiService: jasmine.SpyObj<ApiService>;
  let saveAsSpy: jasmine.Spy;
  let fileInput: ElementRef;

  beforeEach(async () => {
    const apiServiceSpy = jasmine.createSpyObj('ApiService', ['getFiles', 'uploadFile', 'downloadFile', 'deleteFile']);
    apiServiceSpy.getFiles.and.returnValue(of([]));
    apiServiceSpy.uploadFile.and.returnValue(of(null));
    apiServiceSpy.downloadFile.and.returnValue(of(new Blob()));
    apiServiceSpy.deleteFile.and.returnValue(of(null));

    saveAsSpy = jasmine.createSpy('saveAs');
    (window as any).saveAs = saveAsSpy;

    await TestBed.configureTestingModule({
      imports: [FormsModule, HttpClientTestingModule],
      providers: [{ provide: ApiService, useValue: apiServiceSpy }],
      schemas: [CUSTOM_ELEMENTS_SCHEMA]
    }).compileComponents();

    fixture = TestBed.createComponent(FileList);
    component = fixture.componentInstance;
    apiService = apiServiceSpy;
    fileInput = new ElementRef({ nativeElement: { files: [] }});
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  describe('loadFiles', () => {
    it('should call getFiles with correct parameters', () => {
      component.take = 10;
      component.skip = 0;
      component.searchTerm = 'test';
      component.loadFiles();
      expect(apiService.getFiles).toHaveBeenCalledWith(10, 0, 'test');
    });

    it('should update files and totalLoaded', () => {
      const mockData: StoredFile[] = [{ id: '1', fileName: 'file.txt', uploadedAt: new Date() }];
      apiService.getFiles.and.returnValue(of(mockData));
      component.loadFiles();
      expect(component.files).toEqual(mockData);
      expect(component.totalLoaded).toBe(1);
    });
  });

  describe('downloadFile', () => {
    it('should call downloadFile with file ID', () => {
      const file: StoredFile = { id: '1', fileName: 'file.txt', uploadedAt: new Date() };
      component.downloadFile(file);
      expect(apiService.downloadFile).toHaveBeenCalledWith('1');
    });

    it('should call saveAs with downloaded blob', () => {
      const fileName = 'file.txt';
      const file: StoredFile = { id: '1', fileName, uploadedAt: new Date() };
      const blob = new Blob(['Hello, world!'], { type: 'text/plain' });
      const saveAsSpy = jasmine.createSpy('saveAs');
      setSaveAs(saveAsSpy);
      apiService.downloadFile.and.returnValue(of(blob));
      component.downloadFile(file);
      expect(saveAsSpy).toHaveBeenCalledWith(blob, fileName);
    });
  });

  describe('deleteFile', () => {
    it('should call deleteFile with file ID', () => {
      component.deleteFile('1');
      expect(apiService.deleteFile).toHaveBeenCalledWith('1');
    });

    it('should reload files after deletion', () => {
      spyOn(component, 'loadFiles').and.callThrough();
      component.deleteFile('1');
      expect(component.loadFiles).toHaveBeenCalled();
    });
  });

  describe('nextPage', () => {
    it('should increment skip and load files', () => {
      component.take = 10;
      component.skip = 0;
      component.nextPage();
      expect(component.skip).toBe(10);
      expect(apiService.getFiles).toHaveBeenCalledWith(10, 10, '');
    });

    it('should disable next button when at end', () => {
      component.take = 10;
      component.skip = 0;
      component.totalLoaded = 5; // less than take, so at end
      fixture.detectChanges();
      const nextBtn: HTMLButtonElement = fixture.nativeElement.querySelector('.pagination button:last-child');
      expect(nextBtn.disabled).toBeTrue();
    });
  });

  describe('prevPage', () => {
    it('should decrement skip and load files', () => {
      component.skip = 10;
      component.prevPage();
      expect(component.skip).toBe(0);
      expect(apiService.getFiles).toHaveBeenCalledWith(10, 0, '');
    });

    it('should disable prev button when at start', () => {
      component.skip = 0;
      fixture.detectChanges();
      const prevBtn: HTMLButtonElement = fixture.nativeElement.querySelector('.pagination button:first-child');
      expect(prevBtn.disabled).toBeTrue();
    });
  });

  describe('onSearch', () => {
    it('should reset skip and load files', () => {
      const term = 'test';
      component.skip = 5;
      component.searchTerm = term;
      component.onSearch();
      expect(component.skip).toBe(0);
      expect(apiService.getFiles).toHaveBeenCalledWith(component.take, 0, term);
    });
  });

  describe('onUpload', () => {
    it('should upload file and reset input', () => {
      const mockFile = new File(['content'], 'test.txt');
      (fileInput.nativeElement as any).files = [mockFile];
      component.fileInput = fileInput;
      component.onUpload();
      expect(apiService.uploadFile).toHaveBeenCalledWith(mockFile);
      expect(component.fileInput.nativeElement.value).toBe('');
    });
  });
});
