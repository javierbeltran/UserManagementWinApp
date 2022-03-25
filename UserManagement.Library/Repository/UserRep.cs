using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Library.Database;
using UserManagement.Library.Models;
using UserManagement.Library.Response;

namespace UserManagement.Library.Repository
{
    public class UserRep : IUserRep
    {
        private readonly DBConfiguration dbConfig;

        public UserRep(DBConfiguration databaseConfig)
        {
            this.dbConfig = databaseConfig;
        }

        public async Task<GenericResponse> Create(User user)
        {
            try
            {
                using (var connection = new SqliteConnection(dbConfig.Name))
                {
                    var id = await connection.InsertAsync(user);

                    return new GenericResponse(id);
                }
            }
            catch (Exception ex)
            {
                return new GenericResponse(ex.Message);
            }
        }

        public async Task<GenericResponse> Get(int userId)
        {
            try
            {
                using (var connection = new SqliteConnection(dbConfig.Name))
                {
                    var user = await connection.GetAsync<User>(userId);

                    return new GenericResponse(user);
                }
            }
            catch (Exception ex)
            {
                return new GenericResponse(ex.Message);
            }
        }

        public async Task<GenericResponse> GetAll()
        {
            try
            {
                using (var connection = new SqliteConnection(dbConfig.Name))
                {
                    var userList = await connection.GetAllAsync<User>();

                    return new GenericResponse(userList);
                }
            }
            catch (Exception ex)
            {
                return new GenericResponse(ex.Message);
            }
        }

        public async Task<GenericResponse> Delete(int userId)
        {
            try
            {
                using (var connection = new SqliteConnection(dbConfig.Name))
                {
                    var user = await connection.GetAsync<User>(userId);
                    var result = await connection.DeleteAsync(user);

                    return new GenericResponse(result);
                }
            }
            catch (Exception ex)
            {
                return new GenericResponse(ex.Message);
            }
        }

        public async Task<GenericResponse> Update(User user)
        {
            try
            {
                using (var connection = new SqliteConnection(dbConfig.Name))
                {
                    {
                        var result = await connection.UpdateAsync(user);

                        return new GenericResponse(result);
                    }
                }
            }
            catch (Exception ex)
            {
                return new GenericResponse(ex.Message);
            }
        }

        public async Task<GenericResponse> EmailIsValid(string email)
        {
            try
            {
                using (var connection = new SqliteConnection(dbConfig.Name))
                {

                    int coincidences = await connection.ExecuteScalarAsync<int>($"SELECT COUNT(*) FROM User WHERE TRIM(Email) = TRIM('{email}')");

                    return new GenericResponse(coincidences <= 0);
                }
            }
            catch (Exception ex)
            {
                return new GenericResponse(ex.Message);
            }
        }
    }
}
