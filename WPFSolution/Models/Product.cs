using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValitE3DProjectCreator.Models
{
    [Serializable]
    public class Product
    {
        public int RPId { get; set; }
        public string RP { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
    }
}
