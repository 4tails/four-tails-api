using FourTails.Core.Repositories;
using FourTails.Core.DomainModels;
using FourTails.DTOs.Payload.User;

namespace FourTails.Services.Container;

public interface IUserService
{
    Task<IEnumerable<User>> ReadAllUsers();
    Task<User> ReadById(string id);
    User Update(User user, UserUpdateDetailsDTO userUpdateDetailsDTO);

    Task DeleteUser(User user);
}

public class UserService : IUserService
{
    private readonly ICrudRepository<User> _crudRepository;
    public UserService(ICrudRepository<User> crudRepository)
    {
        _crudRepository = crudRepository;
    }

    public async Task<IEnumerable<User>> ReadAllUsers()
    {
        return await _crudRepository.ReadAll();
    }

    public async Task<User> ReadById(string id)
    {
        return await _crudRepository.ReadById(id);
    }

    public User Update(User user, UserUpdateDetailsDTO userUpdateDetailsDTO)
    {
        
            user.Age = userUpdateDetailsDTO.Age;
            user.Address = userUpdateDetailsDTO.Address;
            user.FirstName = userUpdateDetailsDTO.FirstName;
            user.LastName = userUpdateDetailsDTO.LastName;
            user.UserName = userUpdateDetailsDTO.UserName;

            _crudRepository.Update(user);

            return user;
  
    }

    public Task DeleteUser(User user)
    {
        return _crudRepository.Delete(user);
    }
}