using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using VinculacionBackend.Models;
using System.Web.Http.Cors;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Interfaces;
using VinculacionBackend.Security.BasicAuthentication;
using System;

namespace VinculacionBackend.Controllers
{

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class HoursController : ApiController
    {
        private readonly IHoursServices _hoursServices;
        private readonly ILogger _logger;

        public HoursController(IHoursServices hoursServices, ILogger logger)
        {
            _hoursServices = hoursServices;
            _logger = logger;
        }

        // POST: api/Hours
        [ResponseType(typeof(Hour))]
        [Route("api/Hours")]
        [CustomAuthorize(Roles = "Admin,Professor")]
        public IHttpActionResult PostHour(HourEntryModel hourModel)
        {
            try
            {
                var hour = _hoursServices.Add(hourModel, HttpContext.Current.User.Identity.Name);
                return Ok(hour);
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }


        [ResponseType(typeof(Hour))]
        [Route("api/Hours/AddMany")]
        [CustomAuthorize(Roles = "Admin,Professor")]
        public IHttpActionResult PostManyHours(HourCollectionEntryModel hourModel)
        {
            try
            {
                _hoursServices.AddMany(hourModel, HttpContext.Current.User.Identity.Name);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }

        // POST: api/Hours
        [ResponseType(typeof(Hour))]
        [Route("api/Hours/{HourId}")]
        [CustomAuthorize(Roles = "Admin,Professor")]
        public IHttpActionResult PutHour(long hourId,HourEntryModel hourModel)
        {
            try
            {
                var hour = _hoursServices.Update(hourId, hourModel);
                return Ok(hour);
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }
    }
}