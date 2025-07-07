using news_aggregator.console.Services.Interfaces;
using System;
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

            try
            {
                var success = await _categoryService.AddCategoryAsync(name!);
                Console.WriteLine(success ? "Category added successfully." : "Failed to add category.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.ReadKey();
        }

        public async Task ToggleCategoryvisibility()
        {
            try
            {
                var categories = await _categoryService.GetAllCategoriesAsync();

                Console.WriteLine("Available Categories:");
                foreach (var category in categories)
                {
                    Console.WriteLine($"{category.CategoryId}. {category.Name} (Hidden: {category.IsHidden})");
                }

                Console.Write("Enter Category ID to toggle visibility: ");

                if (int.TryParse(Console.ReadLine(), out int categoryId))
                {
                    var result = await _categoryService.ToggleCategoryVisibilityAsync(categoryId);
                    Console.WriteLine(result
                        ? "Category visibility toggled successfully."
                        : "Failed to toggle category visibility.");
                }
                else
                {
                    Console.WriteLine("Invalid Category ID.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.ReadKey();
        }
    }
}
    