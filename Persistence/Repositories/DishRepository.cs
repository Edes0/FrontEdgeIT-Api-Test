using System.Collections.Generic;
using dotnet_api_test.Exceptions.ExceptionResponses;
using dotnet_api_test.Persistence.Repositories.Interfaces;

namespace dotnet_api_test.Persistence.Repositories
{
    public class DishRepository : IDishRepository
    {
        private readonly AppDbContext _context;

        public DishRepository(AppDbContext context)
        {
            _context = context;
        }

        void IDishRepository.SaveChanges()
        {

        }

        public IEnumerable<Dish> GetAllDishes()
        {
            return _context.Dishes;
        }

        public dynamic? GetAverageDishPrice()
        {
            return _context.Dishes.Average(d => d.Cost);
        }

        public Dish GetDishById(int Id)
        {
            return GetDishAndCheckIfItExists(Id);
        }

        public void DeleteDishById(int Id)
        {
            var dish = GetDishAndCheckIfItExists(Id);
            _context.Dishes.Remove(dish);
            _context.SaveChanges();
        }

        public Dish CreateDish(Dish dish)
        {
            _context.Dishes.Add(dish);
            _context.SaveChanges();
            return dish;
        }

        public Dish UpdateDish(Dish dish)
        {
            _context.Dishes.Update(dish);
            _context.SaveChanges();
            return dish;
        }

        private Dish GetDishAndCheckIfItExists(int Id)
        {
            var dish = _context.Dishes.Find(Id);

            if (dish is null)
                throw new NotFoundRequestExceptionResponse($"Dish with id: ({Id}) doesn't exist");

            return dish;
        }
    }
}