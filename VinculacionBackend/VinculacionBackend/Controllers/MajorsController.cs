using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Cors;
using System.Web.OData;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Interfaces;
using System;

namespace VinculacionBackend.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class MajorsController : ApiController
    {
        private readonly IMajorsServices _majorsServices;
        private readonly IMemoryCacher _memCacher;
        private readonly ILogger _logger;

        public MajorsController(IMajorsServices majorsServices,IMemoryCacher memcCacher, ILogger logger)
        {
            _majorsServices = majorsServices;
            _memCacher = memcCacher;
            _logger = logger;
        }

        // GET: api/Majors
        [Route("api/Majors")]
        [EnableQuery]
        public IEnumerable<Major> GetMajors()
        {
            try
            {
                return _memCacher.GetMajors(_majorsServices);
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }
        
        // GET: api/Majors/5
        [ResponseType(typeof(Major))]
        [Route("api/Majors/{majorId}")]
        public IHttpActionResult GetMajor(string majorId)
        {
            try
            {
                Major major = _majorsServices.Find(majorId);
                return Ok(major);
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }


        [ResponseType(typeof(Major))]
        [Route("api/Majors/MajorsByProject/{projectId}")]
        public IQueryable<Major> GetMajorsByProject(long projectId)
        {
            try
            {
                return _majorsServices.GetByProject(projectId);
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                throw;
            }
        }
    }
}