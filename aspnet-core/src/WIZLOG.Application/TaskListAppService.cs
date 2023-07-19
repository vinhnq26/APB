using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using static Volo.Abp.Identity.Settings.IdentitySettingNames;
using Volo.Abp.Users;
using Volo.Abp.ObjectMapping;
using Scriban.Syntax;

namespace WIZLOG
{
    public class TaskListAppService : ApplicationService, ITaskListService
    {
        private readonly IRepository<TaskListItem, Guid> _todoItemRepository;

        public TaskListAppService(IRepository<TaskListItem, Guid> todoItemRepository)
        {
            _todoItemRepository = todoItemRepository;
        }

        // TODO: Implement the methods here...

        public async Task<List<TaskListItemDto>> GetListAsync()
        {
            var items = await _todoItemRepository.GetListAsync();
            return items
                .Select(item => new TaskListItemDto
                {
                    Id = item.Id,
                    TaskId = item.TaskId,
                    Name = item.Name,
                    StartDate = item.StartDate,
                    Deadline = item.Deadline,
                    EndDate = item.EndDate,
                    Assignee = item.Assignee,
                    ReporterId = item.ReporterId,
                    CreateDate = item.CreateDate,
                    TaskStatus = item.TaskStatus,
                    Progress = item.Progress
                }).ToList();
        }
        
        public async Task<TaskListItemDto> CreateAsync(TaskListItemModifyDto data)
        {
            var taskItem = await _todoItemRepository.InsertAsync(
                new TaskListItem {
                    TaskId = data.TaskId,
                    Name = data.Name,
                    StartDate = data.StartDate,
                    Deadline = data.Deadline,
                    EndDate = data.EndDate,
                    Assignee = data.Assignee,
                    CreateDate = data.CreateDate,
                    TaskStatus = data.TaskStatus,
                    Progress = data.Progress

                }
            );  

            return new TaskListItemDto
            {
                Id = taskItem.Id,
                TaskId = taskItem.TaskId,
                Name = taskItem.Name,
                StartDate = taskItem.StartDate,
                Deadline = taskItem.Deadline,
                EndDate = taskItem.EndDate,
                Assignee = taskItem.Assignee,
                CreateDate = taskItem.CreateDate,
                TaskStatus = taskItem.TaskStatus,
                Progress = taskItem.Progress

            };
        }

        public async Task UpdateAsync(Guid id , TaskListItemModifyDto data)
        {
            var findItem = await _todoItemRepository.GetAsync( id );

            //Automatically set properties of the user object using the UpdateUserInput

            findItem.TaskId = data.TaskId;
            findItem.Name = data.Name;
            findItem.StartDate = data.StartDate;
            findItem.Deadline = data.Deadline;
            findItem.EndDate = data.EndDate;
            findItem.Assignee = data.Assignee;
            findItem.CreateDate = data.CreateDate;
            findItem.TaskStatus = data.TaskStatus;
            findItem.Progress = data.Progress;

            await _todoItemRepository.UpdateAsync(findItem);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _todoItemRepository.DeleteAsync(id);
        }

        public async Task<List<TaskListItemDto>> SearchListAsync(string filter = null, int skipCount = 0, int maxResultCount = 10)
        {
            // Retrieve the items from the repository based on the filter
            var items = await _todoItemRepository.GetListAsync();

            // Apply the filter (in this case, we're filtering by the Assignee field)
            if (!string.IsNullOrEmpty(filter))
            {
                items = items.Where(item => item.TaskId.ToLower().Contains(filter.ToLower())).ToList();
            }

            // Apply the skipCount and maxResultCount to the items list
            var pagedItems = items.Skip(skipCount).Take(maxResultCount).ToList();

            // Map the filtered and paged items to TaskListItemDto
            return pagedItems.Select(item => new TaskListItemDto
            {
                Id = item.Id,
                TaskId = item.TaskId,
                Name = item.Name,
                StartDate = item.StartDate,
                Deadline = item.Deadline,
                EndDate = item.EndDate,
                Assignee = item.Assignee,
                ReporterId = item.ReporterId,
                CreateDate = item.CreateDate,
                TaskStatus = item.TaskStatus,
                Progress = item.Progress
            }).ToList();
        }
    }
}
