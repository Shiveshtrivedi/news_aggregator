using news_aggregator.console.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.console.Menu.NewFolder
{
    public class CategoryHandler
    {
        private readonly ICategoryService _categoryService;

        public CategoryHandler(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task AddNewCategory()
        {
            Console.Write("Enter new category name: ");
            var name = Console.ReadLine();
            var success = await _categoryService.AddCategoryAsync(name!);
            Console.WriteLine(success ? "Category added successfully." : "Failed to add category.");
            Console.ReadKey();
        }
    }
}
