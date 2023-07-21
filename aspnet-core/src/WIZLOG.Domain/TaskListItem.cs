using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace WIZLOG
{
    public class TaskListItem : BasicAggregateRoot<Guid>
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
    public class YourDataObject
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
