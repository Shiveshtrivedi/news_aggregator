using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.console.Models
{
    public class NotificationConfigDto
    {
        public int NotificationConfigId { get; set; }
        public int UserId { get; set; }
        public bool KeywordsEnabled { get; set; }
        public List<CategorySettingDto> CategorySettings { get; set; } = new();
    }

    public class CategorySettingDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string CategoryName { get; set; }
        public bool IsEnabled { get; set; }
    }

}
