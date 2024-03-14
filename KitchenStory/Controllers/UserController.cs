using AutoMapper;
using BusinessLogicLayer;
using KitchenStory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace KitchenStory.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UserController : ApiController
    {
        private readonly IMapper mapper;
        public UserController()
        {
            var mapConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserDTO, UserModel>();
                cfg.CreateMap<FoodDTO, FoodModel>();
                cfg.CreateMap<FoodModel, FoodDTO>();
            });
            mapper = mapConfig.CreateMapper();
        }

        UserService userBusiness = new UserService();

        [HttpGet]
        [Route("api/user/getfooditemsbyname")]
        public IHttpActionResult GetFoodItemsByName(string searchTerm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    return BadRequest("Invalid search term");
                }
                List<FoodDTO> matchingFoodItems = userBusiness.GetFoodItemsByName(searchTerm);

                return Ok(matchingFoodItems);
            }
            catch (InvalidOperationException ex)
            {
                return Ok("Food not found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("api/user/{userId}/addtocart")]
        public IHttpActionResult AddToCart(int userId, [FromBody] CartItemDTO cartItem)
        {
            try
            {
                //int usersId = GetUserIdFromAuthentication();
                //int userId = 101;

                //if (userId == 0)
                //{
                //    return Unauthorized();
                //}
                userBusiness.AddToCart(userId, cartItem);

                return Ok($"FoodItem {cartItem.FoodId} added to the cart successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("api/user/{userId}/cart")]
        public IHttpActionResult GetCartItems(int userId)
        {
            List<CartDTO> cartItems = userBusiness.GetCartItems(userId);
            //List<CartModel> cartModels = mapper.Map<List<CartModel>>(cartItems);
            return Ok(cartItems);
        }

        [HttpPut]
        [Route("api/user/{userId}/updatequantity/{cartId}")]
        public IHttpActionResult UpdateCartItemQuantity(int userId, int cartId, [FromBody] int newQuantity)
        {
            try
            {
                userBusiness.UpdateCartItemQuantity(userId, cartId, newQuantity);
                return Ok("Cart item quantity updated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("api/user/{userId}/removecartitem/{cartId}")]
        public IHttpActionResult RemoveCartItem(int userId, int cartId)
        {
            try
            {
                string removedItem = userBusiness.RemoveCartItem(userId, cartId);
                return Ok(removedItem);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("api/user/{userId}/getorderdetails")]
        public IHttpActionResult GetOrderDetails(int userId, [FromBody] PaymentMode paymentMode)
        {
            OrderDetailsDTO orderDetails = userBusiness.GetOrderDetails(userId, paymentMode);
            return Ok(orderDetails);
        }

        [HttpPost]
        [Route("api/user/{userId}/placeorder")]
        public IHttpActionResult PlaceOrder(int userId, [FromBody] PlaceOrderDTO order)
        {
            userBusiness.PlaceOrder(userId, order.PaymentMode);
            return Ok($"Your Order has been placed successfully");
        }
    }
}
