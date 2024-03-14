using AutoMapper;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class UserService
    {
        private readonly IMapper mapper;
        public UserService()
        {
            var mapConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Food, FoodDTO>();
                cfg.CreateMap<FoodDTO, Food>();
                cfg.CreateMap<User, UserDTO>();
                cfg.CreateMap<Cart, CartDTO>();
            });
            mapper = mapConfig.CreateMapper();
        }

        DataAccessLayer.KitchenStoryDBEntities db = new KitchenStoryDBEntities();
        public List<FoodDTO> GetFoodItemsByName(string searchTerm)
        {
            DbSet<Food> foodDb = db.Foods;
            List<Food> matchingFoods;

            if (searchTerm.Length == 1)
            {
                matchingFoods = foodDb.Where(f => f.Name.StartsWith(searchTerm)).ToList();
            }
            else
            {
                matchingFoods = foodDb.Where(f => f.Name.Contains(searchTerm)).ToList();
            }
            if (matchingFoods.Count == 0)
            {
                throw new InvalidOperationException("Item not found.");
            }
            List<FoodDTO> matchingFoodDTOs = mapper.Map<List<FoodDTO>>(matchingFoods);

            return matchingFoodDTOs;
        }
        public void AddToCart(int userId, CartItemDTO cartItem)
        {
            DbSet<Food> foodDb = db.Foods;
            DbSet<Cart> cartDb = db.Carts;

            Food foodItem = foodDb.FirstOrDefault(f => f.Id == cartItem.FoodId);
            decimal cartPrice = foodItem.Price * cartItem.Quantity;

            Cart existingCartItem = cartDb.FirstOrDefault(c => c.UserId == cartItem.UserId && c.FoodId == cartItem.FoodId);

            if (existingCartItem != null)
            {
                existingCartItem.Quantity += cartItem.Quantity;
                existingCartItem.CartPrice += cartPrice;
            }
            else
            {
                Cart newCartItem = new Cart
                {
                    UserId = cartItem.UserId,
                    FoodId = cartItem.FoodId,
                    Quantity = cartItem.Quantity,
                    CartPrice = cartPrice,
                    FoodName = foodItem.Name
                };
                cartDb.Add(newCartItem);
            }
            db.SaveChanges();
        }
        public List<CartDTO> GetCartItems(int userId)
        {
            DbSet<Cart> cartDb = db.Carts;

            List<CartDTO> cartItems = cartDb.Where(c => c.UserId == userId)
                     .Select(cart => new CartDTO
                     {
                         Id = cart.Id,
                         UserId = cart.UserId,
                         FoodId = cart.FoodId,
                         Quantity = cart.Quantity,
                         FoodName = cart.FoodName,
                         CartPrice = cart.CartPrice
                     }).ToList();
            return cartItems;
        }
        public void UpdateCartItemQuantity(int userId, int cartId, int quantity)
        {
            DbSet<Cart> cartDb = db.Carts;
            DbSet<Food> foodDb = db.Foods;

            Cart existingCartItem = cartDb.FirstOrDefault(c => c.Id == cartId && c.UserId == userId);
            Food food = foodDb.FirstOrDefault(f => f.Id == existingCartItem.FoodId);

            if (existingCartItem != null)
            {
                existingCartItem.Quantity = quantity;
                existingCartItem.CartPrice = food.Price * quantity;
                db.SaveChanges();
            }
        }
        public string RemoveCartItem(int userId, int cartId)
        {
            DbSet<Cart> cartDb = db.Carts;
            Cart cartItem = cartDb.FirstOrDefault(c => c.Id == cartId && c.UserId == userId);

            cartDb.Remove(cartItem);
            db.SaveChanges();

            return $"{cartItem.FoodName} is removed from the cart.";
        }
        public OrderDetailsDTO GetOrderDetails(int userId, PaymentMode paymentMode)
        {
            DbSet<Cart> cartDb = db.Carts;
            DbSet<Food> foodDb = db.Foods;
            DbSet<User> userDb = db.Users;

            var cartItems = cartDb
                .Where(c => c.UserId == userId)
                .Select(c => new CartDTO
                {
                    Id = c.Id,
                    FoodId = c.FoodId,
                    FoodName = c.FoodName,
                    Quantity = c.Quantity,
                    CartPrice = c.CartPrice
                }).ToList();

            decimal totalAmount = cartItems.Sum(c => c.CartPrice);

            var userDetails = userDb
                .Where(u => u.Id == userId)
                .Select(u => new UserDTO
                {
                    FullName = u.FullName,
                    EmailId = u.EmailId,
                    ContactNo = u.ContactNo,
                    Address = u.Address
                })
                .FirstOrDefault();

            OrderDetailsDTO orderDetails = new OrderDetailsDTO
            {
                CartItems = cartItems,
                TotalAmount = totalAmount,
                UserDetails = userDetails,
                PaymentMode = paymentMode
            };
            return orderDetails;
        }
        public void PlaceOrder(int userId, string paymentMode)
        {
            List<Cart> cartItems = db.Carts.Where(c => c.UserId == userId).ToList();
            decimal totalAmount = cartItems.Sum(c => c.CartPrice);

            Order newOrder = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                TotalAmount = totalAmount,
                PaymentMode = paymentMode
            };
            db.Orders.Add(newOrder);
            db.SaveChanges();

            // Clear the user's cart after placing the order
            //db.Carts.RemoveRange(cartItems);
            //db.SaveChanges();
        }
    }
}