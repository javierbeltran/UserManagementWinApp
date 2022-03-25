using System.Threading.Tasks;
using UserManagement.Library.Models;
using UserManagement.Library.Response;

namespace UserManagement.Library.Repository
{
    public interface IUserRep
    {
        Task<GenericResponse> Create(User user);
        Task<GenericResponse> Delete(int userId);
        Task<GenericResponse> Get(int userId);
        Task<GenericResponse> GetAll();
        Task<GenericResponse> Update(User user);
        Task<GenericResponse> EmailIsValid(string email);
    }
}