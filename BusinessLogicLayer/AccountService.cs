using AutoMapper;
using DataAccessLayer;
using System;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BusinessLogicLayer
{
    public class AccountService
    {
        private readonly IMapper mapper;
        public AccountService()
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
        public void RegisterUser(UserDTO newUser)
        {
            DbSet<User> userDb = db.Users;

            if (userDb.Any(u => u.EmailId == newUser.EmailId))
            {
                throw new InvalidOperationException("User with this email id already exists.");
            }
            User user = mapper.Map<UserDTO, User>(newUser);
            user.Password = HashPassword(newUser.Password);

            userDb.Add(user);
            db.SaveChanges();
        }
        public UserDTO Login(string email, string password)
        {
            DbSet<User> userDb = db.Users;
            string hashedPassword = HashPassword(password);
            User loginUser = userDb.FirstOrDefault(u => u.EmailId == email && u.Password == hashedPassword);

            if (loginUser != null)
            {
                UserDTO userDTO = new UserDTO
                {
                    Id = loginUser.Id,
                    EmailId = loginUser.EmailId,
                    FullName = loginUser.FullName,
                    IsAdmin = loginUser.IsAdmin
                };

                return userDTO;
            }
            else
            {
                throw new InvalidOperationException("Invalid email or password.");
            }
        }
        public void AdminResetPassword(string email, string newPassword, string confirmPassword)
        {
            DbSet<User> userDb = db.Users;
            User admin = userDb.FirstOrDefault(u => u.EmailId == email && u.IsAdmin);

            if (admin != null)
            {
                if (newPassword != confirmPassword)
                {
                    throw new InvalidOperationException("New password and confirm password do not match.");
                }
                admin.Password = HashPassword(newPassword);
                db.SaveChanges();
            }
            else
            {
                throw new InvalidOperationException("Admin user not found for the given email.");
            }
        }
        public void UserResetPassword(string email, string newPassword, string confirmPassword, string contactNumber)
        {
            DbSet<User> userDb = db.Users;
            User user = userDb.FirstOrDefault(u => u.EmailId == email && !u.IsAdmin && u.ContactNo == contactNumber);

            if (user != null)
            {
                if (newPassword != confirmPassword)
                {
                    throw new InvalidOperationException("New password and confirm password do not match.");
                }
                user.Password = HashPassword(newPassword);
                db.SaveChanges();
            }
            else
            {
                throw new InvalidOperationException("User not found or invalid contact number.");
            }
        }
        public string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                byte[] truncatedBytes = new byte[8];
                Array.Copy(bytes, truncatedBytes, 8);

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < truncatedBytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}