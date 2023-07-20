using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Microsoft.AspNetCore.Http;

namespace WIZLOG
{
    public interface ITaskListService : IApplicationService
    {
        Task<PageDataDto> GetListAsync(int skipCount = 0, int maxResultCount = 1, int pageNumber = 1);
        Task<TaskListItemDto> CreateAsync(TaskListItemModifyDto data);
        Task DeleteAsync(Guid id);
        Task<List<TaskListItemDto>> SearchListAsync(string filter = null, int skipCount = 0, int maxResultCount = 10 , int pageNumber = 1);
        Task ImportData(IFormFile file);
    }
}
