using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class UserSession
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid RefreshToken { get; set; }
        public DateTimeOffset Created { get; set; }
        public bool IsActive { get; set; } = true;

        public virtual User? User { get; set; }




    }
}
