using AutoMapper;
using BusinessLogicLayer;
using KitchenStory.Models;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace KitchenStory.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AccountController : ApiController
    {
        private readonly IMapper mapper;
        public AccountController()
        {
            var mapConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserDTO, UserModel>();
                cfg.CreateMap<UserModel, UserDTO>();
            });
            mapper = mapConfig.CreateMapper();
        }

        AccountService accountBusiness = new AccountService();

        [HttpPost]
        [Route("api/account/register")]
        public IHttpActionResult PostNewUser([FromBody] UserModel userModel)
        {
            if (userModel == null)
            {
                return BadRequest("Invalid user data");
            }
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(string.Join(" ", errors));
            }
            UserDTO userDTO = mapper.Map<UserDTO>(userModel);
            try
            {
                accountBusiness.RegisterUser(userDTO);
                return Ok($"{userDTO.FullName}, you have created an account with {userDTO.EmailId}!");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("api/account/login")]
        public IHttpActionResult PostLogin([FromBody] UserModel login)
        {
            try
            {
                UserDTO loggedInUser = accountBusiness.Login(login.EmailId, login.Password);

                if (loggedInUser.IsAdmin)
                {
                    return Ok(new { userType = "admin", userId = loggedInUser.Id });
                    //return Ok($"Welcome, Admin! You are now logged in.");
                }
                else
                {
                    return Ok(new { userType = "user", userId = loggedInUser.Id, userName = loggedInUser.FullName });
                    //return Ok($"Welcome {loggedInUser.FullName}! You are now logged in with UserID: {loggedInUser.Id}.");
                }
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("api/account/adminresetpassword")]
        public IHttpActionResult AdminResetPassword([FromBody] ResetPasswordModel resetPasswordModel)
        {
            try
            {
                accountBusiness.AdminResetPassword(
                    resetPasswordModel.EmailId,
                    resetPasswordModel.NewPassword,
                    resetPasswordModel.ConfirmPassword
                );
                return Ok("Password Reset Successful!");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("api/account/userresetpassword")]
        public IHttpActionResult UserResetPassword([FromBody] ResetPasswordModel resetPasswordModel)
        {
            try
            {
                accountBusiness.UserResetPassword(
                    resetPasswordModel.EmailId,
                    resetPasswordModel.NewPassword,
                    resetPasswordModel.ConfirmPassword,
                    resetPasswordModel.ContactNo
                );
                return Ok("Password has been reset successfully!");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}