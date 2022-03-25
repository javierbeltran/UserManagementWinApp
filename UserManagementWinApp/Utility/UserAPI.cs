using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Library.Models;

namespace UserManagementWinApp.Utility
{
    public class UserAPI
    {
        private const string apiBaseAddress = "http://localhost:54469";

        private async Task<T> ParseJsonResponse<T>(HttpResponseMessage response)
        {
            T returnObj = default;
            if (response.IsSuccessStatusCode)
            {
                var res = await response.Content.ReadAsStringAsync();
                if (typeof(T) == typeof(string)) return (T)(object)res;
                if (typeof(T) == typeof(int)) return (T)(object)Convert.ToInt32(res);
                returnObj = JsonConvert.DeserializeObject<T>(res);
            }
            else
            {
                var result = $"{(int)response.StatusCode} ({response.ReasonPhrase})";
            }
            return returnObj;
        }

        public async Task<User> GetUser(int Id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(new Uri(string.Format(UsersAPIMethods.Get,apiBaseAddress,Id)));
                    return await ParseJsonResponse<User>(response);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(new Uri(string.Format(UsersAPIMethods.GetAll, apiBaseAddress)));
                    return await ParseJsonResponse<IEnumerable<User>>(response);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> UpdateUser(User user)
        {
            try
            {
                var json = JsonConvert.SerializeObject(user);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {
                    var response = await client.PutAsync(new Uri(string.Format(UsersAPIMethods.Put, apiBaseAddress)), data);
                    return await ParseJsonResponse<bool>(response);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<int> CreateUser(User user)
        {
            try
            {
                var json = JsonConvert.SerializeObject(user);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {
                    var response = await client.PostAsync(new Uri(string.Format(UsersAPIMethods.Post, apiBaseAddress)), data);
                    return await ParseJsonResponse<int>(response);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> DeleteUser(int Id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.DeleteAsync(new Uri(string.Format(UsersAPIMethods.Delete, apiBaseAddress, Id)));
                    return await ParseJsonResponse<bool>(response);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> EmailIsValid(string email)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(new Uri(string.Format(UsersAPIMethods.EmailIsValid, apiBaseAddress, email)));
                    return await ParseJsonResponse<bool>(response);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }

    public static class UsersAPIMethods
    {
        public const string Get = @"{0}/api/User/Get?Id={1}";
        public const string GetAll = @"{0}/api/User/GetAll";
        public const string Post = @"{0}/api/User/Post";
        public const string Put = @"{0}/api/User/Put";
        public const string Delete = @"{0}/api/User/Delete?Id={1}";
        public const string EmailIsValid = @"{0}/api/User/EmailIsValid?Email={1}";
    }
}