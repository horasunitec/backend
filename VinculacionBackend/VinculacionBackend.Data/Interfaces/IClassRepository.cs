using System.Collections.Generic;
using VinculacionBackend.Data.Entities;

namespace VinculacionBackend.Data.Interfaces
{
    public interface IClassRepository : IRepository<Class>
    {
    	IEnumerable<Class> GetAllAlpha();
    }
}
