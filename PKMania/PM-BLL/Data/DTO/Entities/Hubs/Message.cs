using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PM_BLL.Data.DTO.Entities.Hubs
{
    public class Message
    {
        public string Sender { get; set; } = string.Empty;
        public string Msg { get; set; } = string.Empty;
    }
}
