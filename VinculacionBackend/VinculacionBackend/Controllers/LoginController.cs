using System.Web.Http;
using VinculacionBackend.Models;
using System.Web.Http.Cors;
using VinculacionBackend.ActionFilters;
using VinculacionBackend.Data.Interfaces;
using VinculacionBackend.Interfaces;
using System.Web.Http.Description;
using System;

namespace VinculacionBackend.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class LoginController : ApiController
    {
        private readonly IUsersServices _usersServices;
        private readonly IEncryption _encryption;
        private readonly ILogger _logger;

        public LoginController(IUsersServices usersServices, IEncryption encryption, ILogger logger)
        {
            _usersServices = usersServices;
            _encryption = encryption;
            _logger = logger;
        }

        [ResponseType(typeof(TokenModel))]
        [Route("api/Login")]
        [ValidateModel]
        public IHttpActionResult PostUserLogin(LoginUserModel loginUser)
        {
            try
            {
                var user = _usersServices.FindValidUser(loginUser.User, _encryption.Encrypt(loginUser.Password));
                string userInfo = user.Email + ":" + user.Password;
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(userInfo);
                string token = System.Convert.ToBase64String(plainTextBytes);
                var tokenModel = new TokenModel { Token = "Basic " + token, Id = user.Id, AccountId = user.AccountId };
                return Ok(tokenModel);
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }

        [Route("api/Login/GetUserRole")]
        [ValidateModel]
        public string PostUserRole(EmailModel model)
        {
            try
            {
                return _usersServices.GetUserRole(model.Email);
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }
    }
}