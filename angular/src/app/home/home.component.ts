import { ToasterService } from '@abp/ng.theme.shared';
import { Component, Injector, OnInit } from '@angular/core';
import { PageDataDto, TaskListItemDto, TaskListService } from '@proxy';
import * as moment from 'moment';
import { AbstractListComponent } from '../shared/component/abstract.component';
import { AuthService } from '@abp/ng.core';
import { FormBuilder, Validators } from '@angular/forms';
import { OAuthService } from 'angular-oauth2-oidc';
import * as XLSX from 'xlsx';
import { HttpHeaders } from '@angular/common/http';
import { IFormFile } from '@proxy/microsoft/asp-net-core/http';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent
  extends AbstractListComponent
  implements OnInit {
  taskListItems: any;
  data: TaskListItemDto;
  isModalOpenCreate: boolean = false;
  isModalOpenDelete: boolean = false;
  getIdDelete: string = '';
  inProgress: boolean;
  isModalOpen: boolean;
  getIdEdit: string = '';
  isEdit: boolean = false;
  editData: TaskListItemDto | null;
  skipCount: number = 0;
  maxResultCount: number = 5;
  currentPage: number = 1;
  totalPages: number = 1;
  pages: number[] = [];
  selectedFile: File | null = null;
  pagesGenerated: boolean = false;

  constructor(injector: Injector,
    private taskListService: TaskListService,
    private toasterService: ToasterService, private fb: FormBuilder, private oAuthService: OAuthService, private authService: AuthService) {
    super(injector);
  }

  ngOnInit(): void {
    this.handleGetData();
  }

  async handleGetData(page?: number) {
    this.taskListService.getList(this.skipCount, this.maxResultCount, page).subscribe(response => {
      this.taskListItems = response.data;
      this.currentPage = response.currentPage;
      this.totalPages = response.totalPages;
      // Call generatePages() only if it has not been called yet
      if (!this.pagesGenerated) {
        this.generatePages();
        this.pagesGenerated = true;
      }
    });
  }
  async generatePages() {
    for (let page = 0; page <= this.totalPages - 1; page++) {
      this.pages.push(page);
    }
  }

  get hasLoggedIn(): boolean {
    return this.oAuthService.hasValidAccessToken();
  }

  login() {
    this.authService.navigateToLogin();
  }

  handleDelete(id: string): void {
    this.taskListService.delete(id).subscribe(() => {
      this.taskListItems = this.taskListItems.filter(item => item.id !== id);
      this.toasterService.info('Deleted the task item.');
    });
  }

  handleFormatDate(date: string): string {
    const newDate = moment(date).format("DD/MM/YYYY");
    return newDate;
  }
  form = this.fb.group({
    Project_Name: [null, [Validators.required]],
    Task_ID: [null, [Validators.required]],
    Task_Name: [null, [Validators.required]],
    Assignee: [null, [Validators.required]],
    Start_Date: [null, [Validators.required]],
    Deadline: [null, [Validators.required]],
    Task_Status: [null, [Validators.required]],
  });

  save() {
    if (this.form.invalid) return;
    this.inProgress = true;
    const formData = {
      taskId: this.form.value.Task_ID,
      name: this.form.value.Task_ID,
      startDate: this.form.value.Start_Date,
      deadline: this.form.value.Start_Date,
      endDate: this.form.value.Start_Date,
      assignee: this.form.value.Task_ID,
      createDate: this.form.value.Start_Date,
      taskStatus: this.form.value.Task_Status,
      progress: this.form.value.Task_Status,
    }
    if (this.isEdit) {
      this.taskListService.update(this.editData.id, formData).subscribe((result) => {
        this.handleGetData();
        this.isModalOpenCreate = false;
        this.isEdit = false;
        this.editData = null;
      });
    } else {
      this.taskListService.create(formData).subscribe((result) => {
        this.handleGetData();
        this.isModalOpenCreate = false;
      });
    }
  }
  handleOnRowEdit(data: TaskListItemDto) {
    const taskData = {
      Project_Name: data.name,
      Task_ID: data.taskId,
      Task_Name: data.name,
      Assignee: data.assignee,
      Start_Date: data.startDate,
      Deadline: data.deadline,
      Task_Status: data.taskStatus,
    };
    this.form.patchValue(taskData as any);
    this.isModalOpenCreate = true;
    this.isEdit = true;
    this.editData = data;
  }
  handleShowCreate() {
    const taskData = {
      Project_Name: '',
      Task_ID: '',
      Task_Name: '',
      Assignee: '',
      Start_Date: '',
      Deadline: '',
      Task_Status: '',
    };
    this.form.patchValue(taskData as any);
    this.isModalOpenCreate = true
  }

  handleFilter(value: string) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.taskListService.searchList(filterValue, this.skipCount, this.maxResultCount).subscribe((response) => {
      this.taskListItems = response;

    });

  }

  exportToExcel(): void {
    // Define the data
    const header = ['ID', 'Task ID', 'Name', 'Start Date', 'Deadline', 'End Date', 'Assignee', 'Reporter ID', 'Create Date', 'Task Status', 'Progress'];
    const data = [header, ...this.taskListItems.map(item => [item.id, item.taskId, item.name, item.startDate, item.deadline, item.endDate, item.assignee, item.reporterId, item.createDate, item.taskStatus, item.progress])];

    // Define styles for the header and cells (columns)
    const wscols = [
      { wpx: 250 }, // Set the width of the first column to 100px
      { wpx: 100 },
      { wpx: 100 },
      { wpx: 100 },
      { wpx: 100 },
      { wpx: 100 },
      { wpx: 100 },
      { wpx: 100 },
      { wpx: 100 },
      { wpx: 100 },
      { wpx: 100 }, // Set the width of the second column to 200px
      // Add more width values for other columns if needed
    ];

    const ws = XLSX.utils.aoa_to_sheet(data);

    // Apply the styles to the worksheet
    ws['!cols'] = wscols;

    // Set the height of the header row (index 0)
    ws['!rows'] = [{ hpx: 30 }, ...new Array(data.length - 1).fill({})];
    // Create a workbook and add the worksheet
    const wb: XLSX.WorkBook = { Sheets: { 'data': ws }, SheetNames: ['data'] };

    // Convert the workbook to a buffer and save the Excel file
    const excelBuffer: any = XLSX.write(wb, { bookType: 'xlsx', type: 'array' });
    this.saveAsExcelFile(excelBuffer, 'data_export');
  }

  private saveAsExcelFile(buffer: any, fileName: string): void {
    const data: Blob = new Blob([buffer], { type: 'application/octet-stream' });
    const url: string = window.URL.createObjectURL(data);

    const a: HTMLAnchorElement = document.createElement('a');
    a.href = url;
    a.download = `${fileName}.xlsx`;
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);
    window.URL.revokeObjectURL(url);
  }

  onFileSelected(event: any): void {
    this.selectedFile = event.target.files[0] as File;
    this.uploadFile()
  }
  uploadFile(): void {
    if (!this.selectedFile) {
      console.error('No file selected.');
      return;
    }
    const formData: any = new FormData();
    formData.append('file', this.selectedFile, this.selectedFile.name);

    this.taskListService.importDataByFile(formData).subscribe((response) => {
      console.log("response", response)
      window.location.reload();
    },
      (error) => {
        console.error('Error uploading file:', error);
      });
  }


}
