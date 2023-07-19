import { ToasterService } from '@abp/ng.theme.shared';
import { Component, Injector, OnInit } from '@angular/core';
import { TaskListItemDto, TaskListService } from '@proxy';
import * as moment from 'moment';
import { AbstractListComponent } from '../shared/component/abstract.component';
import { AuthService } from '@abp/ng.core';
import { FormBuilder, Validators } from '@angular/forms';
import { OAuthService } from 'angular-oauth2-oidc';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent
  extends AbstractListComponent
  implements OnInit {
  taskListItems: TaskListItemDto[];
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
  maxResultCount: number = 10;

  constructor(injector: Injector,
    private taskListService: TaskListService,
    private toasterService: ToasterService, private fb: FormBuilder, private oAuthService: OAuthService, private authService: AuthService) {
    super(injector);
  }

  ngOnInit(): void {
    this.taskListService.getList().subscribe(response => {
      this.taskListItems = response;
    });
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

  // create(): void {
  //   this.taskListService.create(this.data).subscribe((result) => {
  //     this.taskListItems = this.taskListItems.concat(result);
  //   });
  // }

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
        this.taskListService.getList().subscribe(response => {
          this.taskListItems = response;
        });
        this.isModalOpenCreate = false;
        this.isEdit = false;
        this.editData = null;
      });
    } else {
      this.taskListService.create(formData).subscribe((result) => {
        // console.log("result", result)
        // this.taskListItems = this.taskListItems.concat(result);
        this.taskListService.getList().subscribe(response => {
          this.taskListItems = response;
        });
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
      console.log("response", response)
      this.taskListItems = response;

    });

  }

}


