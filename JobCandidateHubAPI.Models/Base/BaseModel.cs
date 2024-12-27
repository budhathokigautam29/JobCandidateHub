using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobCandidateHubAPI.Models.Base
{
    public class BaseModel
    {
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime CreatedTS { get; set; }
        public DateTime UpdatedTs { get; set; }
    }
}
