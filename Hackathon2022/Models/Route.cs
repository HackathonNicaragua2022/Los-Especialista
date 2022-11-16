namespace Hackathon2022.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class Route
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public string Type { get; set; }

        public string Description { get; set; }

        public string Code { get; set; }
    }
}
