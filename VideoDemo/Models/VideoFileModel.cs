using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VideoDemo.Models
{
    public class VideoFileModel
    {
        public string Count { get; set; }
        public string Name { get; set; }
        public IFormFile Files { get; set; }

        public string isLast { get; set; }

    }
}
