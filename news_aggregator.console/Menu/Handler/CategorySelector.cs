using news_aggregator.console.Menu.Handler.Interface;
using news_aggregator.console.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace news_aggregator.console.Menu.Handler
{
    public class CategorySelector : ICategorySelector
    {
        private readonly ICategoryService _categoryService;

        public CategorySelector(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<string?> SelectCategoryAsync()
        {
            try
            {
                var categories = await _categoryService.GetAllCategoriesAsync();

                if (categories == null || categories.Count == 0)
                {
                    Console.WriteLine("No categories found.");
                    Console.ReadKey();
                    return null;
                }

                for (int i = 0; i < categories.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {categories[i].Name}");
                }

                Console.Write("Enter category number: ");
                if (!int.TryParse(Console.ReadLine(), out int selectedIndex) ||
                    selectedIndex < 1 || selectedIndex > categories.Count)
                {
                    Console.WriteLine("Invalid category selection.");
                    Console.ReadKey();
                    return null;
                }

                return categories[selectedIndex - 1].Name;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.ReadKey();
                return null;
            }
        }
    }
}
