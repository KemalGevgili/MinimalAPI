using BusinessLayer.Models;
using DataAccessLayer.Entities;
using DataAccessLayer.Repository;

namespace BusinessLayer.Services
{
    public class UserService
    {
        private readonly UserDb _userDb;

        public UserService()
        {
            _userDb = new UserDb();
        }

        public UserModel GetUserById(int id)
        {
            User user = _userDb.GetUserById(id);

            if (user != null)
            {
                return MapUserToUserModel(user);
            }

            return null;
        }

        public List<UserModel> GetAllUsers()
        {
            List<User> users = _userDb.GetAllUsers();
            List<UserModel> userModels = new List<UserModel>();

            foreach (var user in users)
            {
                userModels.Add(MapUserToUserModel(user));
            }

            return userModels;
        }

        public void AddUser(UserModel userModel)
        {
            User user = MapUserModelToUser(userModel);
            _userDb.AddUser(user);
        }

        public void UpdateUser(UserModel userModel)
        {
            User user = MapUserModelToUser(userModel);
            _userDb.UpdateUser(user);
        }

        public void DeleteUser(int id)
        {
            _userDb.DeleteUser(id);
        }

        private UserModel MapUserToUserModel(User user)
        {
            return new UserModel
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname
            };
        }

        private User MapUserModelToUser(UserModel userModel)
        {
            return new User
            {
                Id = userModel.Id,
                Name = userModel.Name,
                Surname = userModel.Surname
            };
        }
    }
}
