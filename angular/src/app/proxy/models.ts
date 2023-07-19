
export interface TaskListItemDto extends TaskListItemModifyDto {
  id?: string;
}

export interface TaskListItemModifyDto {
  taskId?: string;
  name?: string;
  startDate?: string;
  deadline?: string;
  endDate?: string;
  assignee?: string;
  reporterId?: string;
  createDate?: string;
  taskStatus: number;
  progress: number;
}
