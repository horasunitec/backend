using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using System.Web.OData;
using VinculacionBackend.ActionFilters;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Interfaces;
using VinculacionBackend.Models;
using VinculacionBackend.Security.BasicAuthentication;

namespace VinculacionBackend.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PeriodsController : ApiController
    {
        private readonly IPeriodsServices _periodsServices;
        private readonly ILogger _logger;

        public PeriodsController(IPeriodsServices periodsServices, ILogger logger)
        {
            _periodsServices = periodsServices;
            _logger = logger;
        }

        // GET: api/Periods
        [Route("api/Periods")]
        [EnableQuery]
        public IQueryable<Period> GetPeriods()
        {
            try
            {
                return _periodsServices.All();
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }

        // GET: api/Periods/5
        [ResponseType(typeof(Period))]
        [Route("api/Periods/{id}")]

        public IHttpActionResult GetPeriod(long id)
        {
            try
            {
                Period period = _periodsServices.Find(id);
                return Ok(period);
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }

        // GET: api/Periods/5
        [ResponseType(typeof(Period))]
        [Route("api/Periods/GetCurrentPeriod")]

        public IHttpActionResult GetCurrentPeriod()
        {
            try
            {
                return Ok(_periodsServices.GetCurrentPeriod());
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
            
        }

        // PUT: api/Periods/SetCurrentPeriod/5
        [ResponseType(typeof(Period))]
        [Route("api/Periods/SetCurrentPeriod/{periodId}")]
        [CustomAuthorize(Roles = "Admin")]
        public IHttpActionResult PutSetCurrentPeriod(long periodId)
        {
            try
            {
                var period = _periodsServices.SetCurrentPeriod(periodId);
                return Ok(period);
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }

        // POST: api/Periods
        [ResponseType(typeof(Period))]
        [Route("api/Periods")]
        [CustomAuthorize(Roles = "Admin")]
        [ValidateModel]
        public IHttpActionResult PostPeriod(PeriodEntryModel periodModel)
        {
            try
            {
                var newPeriod = new Period();
                _periodsServices.Map(newPeriod, periodModel);
                _periodsServices.Add(newPeriod);
                return Ok(newPeriod);
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }   
        }

        [ResponseType(typeof(Period))]
        [Route("api/Periods/{periodId}")]
        [CustomAuthorize(Roles = "Admin")]
        [ValidateModel]
        public IHttpActionResult PustPeriod(long periodId,PeriodEntryModel periodModel)
        {
            try
            {
                var newPeriod = _periodsServices.UpdatePeriod(periodId, periodModel);
                return Ok(newPeriod);
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }

        // DELETE: api/Periods/5
        [Route("api/Periods/{id}")]
        [ResponseType(typeof(Period))]
        public IHttpActionResult DeletePeriod(long id)
        {
            try
            {
                var period = _periodsServices.Delete(id);
                return Ok(period);
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }
    }
}