using Component.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component.Data.Repository
{
    public class WorkFlowRepository : BaseRepository<WorkFlow>
    {
        public object GetActiveWorkFlow(Guid referenceEntityId)
        {
            throw new NotImplementedException();
        }

   
    }
}
