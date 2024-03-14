using AutoMapper;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace BusinessLogicLayer
{
    public class AdminService
    {
        private readonly IMapper mapper;
        public AdminService()
        {
            var mapConfig = new MapperConfiguration(cfg => {
                cfg.CreateMap<Food, FoodDTO>();
                cfg.CreateMap<FoodDTO, Food>();
                cfg.CreateMap<User, UserDTO>();
                cfg.CreateMap<UserDTO, User>();
            });
            mapper = mapConfig.CreateMapper();
        }

        DataAccessLayer.KitchenStoryDBEntities db = new KitchenStoryDBEntities();
        public List<FoodDTO> GetListOfFoods()
        {
            DbSet<Food> foodDb = db.Foods;

            List<FoodDTO> foods = mapper.Map<List<FoodDTO>>(foodDb);
            return foods;
        }
        public FoodDTO GetFoodItemById(int foodId)
        {
            DbSet<Food> foodDb = db.Foods;
            Food foodItem = foodDb.FirstOrDefault(f => f.Id == foodId);

            if (foodItem != null)
            {
                FoodDTO foodDTO = mapper.Map<FoodDTO>(foodItem);
                return foodDTO;
            }
            else
            {
                throw new InvalidOperationException($"Food item with ID {foodId} not found.");
            }
        }
        public void AddFoodToMenu(FoodDTO newFoodItem)
        {
            DbSet<Food> foodDb = db.Foods;

            if (foodDb.Any(u => u.Name == newFoodItem.Name))
            {
                throw new InvalidOperationException("This Food already exists.");
            }
            Food food = mapper.Map<FoodDTO, Food>(newFoodItem);

            foodDb.Add(food);
            db.SaveChanges();
        }
        public string DeleteFoodItem(int foodId)
        {
            DbSet<Food> foodDb = db.Foods;
            DbSet<Cart> cartDb = db.Carts;
            var foodItem = foodDb.FirstOrDefault(f => f.Id == foodId);

            if (foodItem != null)
            {
                string foodName = foodItem.Name;
                foodDb.Remove(foodItem);

                var cartItems = cartDb.Where(c => c.FoodId == foodId);
                foreach (var cartItem in cartItems)
                {
                    cartDb.Remove(cartItem);
                }
                db.SaveChanges();
                return foodName;
            }
            return null;
        }
        public void UpdateFoodMenu(int foodId, FoodDTO updatedFoodItem)
        {
            DbSet<Food> foodDb = db.Foods;
            DbSet<Cart> cartDb = db.Carts;

            var existingFood = foodDb.FirstOrDefault(f => f.Id == foodId);

            if (existingFood != null)
            {
                existingFood.Name = updatedFoodItem.Name;
                existingFood.Price = updatedFoodItem.Price;

                var cartItems = cartDb.Where(c => c.FoodId == foodId);
                foreach (var cartItem in cartItems)
                {
                    cartItem.FoodName = updatedFoodItem.Name;
                    cartItem.CartPrice = updatedFoodItem.Price * cartItem.Quantity;
                }
                db.SaveChanges();
            }
        }
    }
}