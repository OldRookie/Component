using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component.ViewModel
{
    public class AttachmentVM
    {
        public Guid Id { get; set; }

        public float Size{ get; set; }

        public string Name { get; set; }

        public string Path { get; set; }
    }
}
