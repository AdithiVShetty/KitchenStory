using AutoMapper;
using BusinessLogicLayer;
using KitchenStory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace KitchenStory.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AdminController : ApiController
    {
        private readonly IMapper mapper;
        public AdminController()
        {
            var mapConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserDTO, UserModel>();
                cfg.CreateMap<UserModel, UserDTO>();
                cfg.CreateMap<FoodDTO, FoodModel>();
                cfg.CreateMap<FoodModel, FoodDTO>();
            });
            mapper = mapConfig.CreateMapper();
        }

        AdminService adminBusiness = new AdminService();

        [HttpGet]
        [Route("api/admin")]
        public List<FoodModel> GetFoodMenu()
        {
            List<FoodDTO> foods = adminBusiness.GetListOfFoods();
            List<FoodModel> foodMenu = mapper.Map<List<FoodModel>>(foods);

            return foodMenu;
        }

        [HttpGet]
        [Route("api/admin/{foodId}")]
        public IHttpActionResult GetFoodItemById(int foodId)
        {
            try
            {
                FoodDTO foodItem = adminBusiness.GetFoodItemById(foodId);
                return Ok(foodItem);
            }
            catch (InvalidOperationException ex)
            {
                return Ok("Item not found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("api/admin/addfood")]
        public IHttpActionResult PostNewFoodItem([FromBody] FoodModel foodModel)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(string.Join(" ", errors));
            }
            FoodDTO foodDTO = mapper.Map<FoodDTO>(foodModel);

            try
            {
                adminBusiness.AddFoodToMenu(foodDTO);
                return Ok($"{foodDTO.Name} is added to Food Menu!");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("api/admin/deletefood/{foodId}")]
        public IHttpActionResult DeleteFoodItem(int foodId)         
        {
            try
            {
                string foodName = adminBusiness.DeleteFoodItem(foodId);
                adminBusiness.DeleteFoodItem(foodId);

                return Ok($"Food Item {foodName} is deleted.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("api/admin/updatefood/{foodId}")]
        public IHttpActionResult PutUpdatedFoodItem(int foodId, [FromBody] FoodModel updatedFoodModel)
        {
            if (updatedFoodModel == null)
            {
                return BadRequest("Invalid food data");
            }
            FoodDTO updatedFoodDTO = mapper.Map<FoodDTO>(updatedFoodModel);
            try
            {
                adminBusiness.UpdateFoodMenu(foodId, updatedFoodDTO);
                return Ok($"Food item {updatedFoodDTO.Name} is updated successfully!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}