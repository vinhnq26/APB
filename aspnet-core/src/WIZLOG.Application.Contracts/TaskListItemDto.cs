using System;
using System.Collections.Generic;
using System.Text;

namespace WIZLOG
{
    public class TaskListItemDto : TaskListItemModifyDto
    {
        public Guid Id { get; set; }
    }

    public class TaskListItemModifyDto
    {
        public string TaskId { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime Deadline { get; set; }
        public DateTime EndDate { get; set; }
        public string Assignee { get; set; }
        public Guid ReporterId { get; set; }
        public string CreateDate { get; set; }
        public int TaskStatus { get; set; }
        public int Progress { get; set; }
    }
}
