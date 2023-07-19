using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace WIZLOG
{
    public interface ITaskListService : IApplicationService
    {
        Task<List<TaskListItemDto>> GetListAsync();
        Task<TaskListItemDto> CreateAsync(TaskListItemModifyDto data);
        Task DeleteAsync(Guid id);
    }
}
