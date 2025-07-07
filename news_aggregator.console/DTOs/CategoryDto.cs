using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace news_aggregator.console.Models
{
    public class CategoryDto
    {
        [JsonPropertyName("categoryId")]
        public int CategoryId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("isHidden")]
        public bool? IsHidden { get; set; } = false;
    }

    public class AddCategoryRequestDto
    {
        public string CategoryName { get; set; }
    }
}
