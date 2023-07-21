using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using System.IO;
using Microsoft.AspNetCore.Http;
using ClosedXML.Excel;
using Microsoft.Extensions.Primitives;
using Volo.Abp;

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
        private int currentPage = 1;
        public async Task<PageDataDto> GetListAsync(int skipCount = 0, int maxResultCount = 5, int pageNumber = 1)
        {
            var items = await _todoItemRepository.GetListAsync();
            currentPage = pageNumber;
            int totalPages = (int)Math.Ceiling((double)items.Count / maxResultCount);
            currentPage = Math.Max(currentPage, 1);
            currentPage = Math.Min(currentPage, totalPages);
            // Apply the skipCount and maxResultCount to the items list
            var skip = (currentPage - 1) * maxResultCount;
            var pagedItems = items.Skip(skip).Take(maxResultCount).ToList();
            var pageData = pagedItems
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
            return new PageDataDto
            {
                Data = pageData,
                CurrentPage = currentPage,
                TotalPages = totalPages,
            };
        }

        public async Task<TaskListItemDto> CreateAsync(TaskListItemModifyDto data)
        {
            var taskItem = await _todoItemRepository.InsertAsync(
                new TaskListItem
                {
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

        public async Task UpdateAsync(Guid id, TaskListItemModifyDto data)
        {
            var findItem = await _todoItemRepository.GetAsync(id);

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

        public async Task<List<TaskListItemDto>> SearchListAsync(string filter = null, int skipCount = 0, int maxResultCount = 5, int pageNumber = 1)
        {
            // Retrieve the items from the repository based on the filter
            var items = await _todoItemRepository.GetListAsync();

            // Apply the filter (in this case, we're filtering by the Assignee field)
            if (!string.IsNullOrEmpty(filter))
            {
                items = items.Where(item => item.TaskId.ToLower().Contains(filter.ToLower())).ToList();
            }
            currentPage = pageNumber;
            var skip = (currentPage - 1) * maxResultCount;
            // Apply the skipCount and maxResultCount to the items list
            var pagedItems = items.Skip(skip).Take(maxResultCount).ToList();

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
        public async Task<bool> ImportData(IFormFile file)
        {
            var result = true;
            try
            {
                if (file == null || file.Length <= 0)
                {
                    throw new UserFriendlyException("No file uploaded.");
                }

                // Read the Excel file into a memory stream.
                using (var memoryStream = new MemoryStream())
                {
                    if (file == null || file.Length <= 0)
                    {
                        throw new UserFriendlyException("No file uploaded.");
                    }

                    // Read the Excel file into a memory stream.
                    using (var fileMemoryStream = new MemoryStream())
                    {
                        file.CopyTo(fileMemoryStream);
                        fileMemoryStream.Position = 0;

                        // Create a workbook using ClosedXML.
                        using (var workbook = new XLWorkbook(fileMemoryStream))
                        {
                            var worksheet = workbook.Worksheets.FirstOrDefault();
                            if (worksheet == null)
                            {
                                throw new UserFriendlyException("No worksheet found in the Excel file.");
                            }

                            // Assuming your data starts from the second row (index 2).
                            int startRow = 2;
                            int endRow = worksheet.LastRowUsed().RowNumber();

                            //var dataObjects = new List<YourDataObject>();

                            for (int row = startRow; row <= endRow; row++)
                            {
                                var TaskId = worksheet.Cell(row, 2).Value.ToString();
                                var Name = worksheet.Cell(row, 3).Value.ToString();
                                var StartDate = DateTime.Parse(worksheet.Cell(row, 4).Value.ToString());
                                var Deadline = DateTime.Parse(worksheet.Cell(row, 5).Value.ToString());
                                var EndDate = DateTime.Parse(worksheet.Cell(row, 6).Value.ToString());
                                var Assignee = worksheet.Cell(row, 7).Value.ToString();
                                var CreateDate = worksheet.Cell(row, 9).Value.ToString();
                                var TaskStatus = worksheet.Cell(row, 10).Value;
                                var Progress = worksheet.Cell(row, 11).Value;

                                var dataObject = new YourDataObject
                                {
                                    TaskId = TaskId,
                                    Name = Name,
                                    StartDate = StartDate,
                                    Deadline = Deadline,
                                    EndDate = EndDate,
                                    Assignee = Assignee,
                                    CreateDate = CreateDate,
                                    TaskStatus = (int)TaskStatus,
                                    Progress = (int)Progress
                                };
                                await _todoItemRepository.InsertAsync(
                 new TaskListItem
                 {
                     TaskId = dataObject.TaskId,
                     Name = dataObject.Name,
                     StartDate = dataObject.StartDate,
                     Deadline = dataObject.Deadline,
                     EndDate = dataObject.EndDate,
                     Assignee = dataObject.Assignee,
                     CreateDate = dataObject.CreateDate,
                     TaskStatus = dataObject.TaskStatus,
                     Progress = dataObject.Progress

                 }
             );
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                Console.WriteLine(ex.Message);

            }
            return result;
        }
    }
}
