import type { TaskListItemDto, TaskListItemModifyDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class TaskListService {
  apiName = 'Default';
  

  create = (data: TaskListItemModifyDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, TaskListItemDto>({
      method: 'POST',
      url: '/api/app/task-list',
      body: data,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/task-list/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, TaskListItemDto[]>({
      method: 'GET',
      url: '/api/app/task-list',
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, data: TaskListItemModifyDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'PUT',
      url: `/api/app/task-list/${id}`,
      body: data,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}