using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fish
{
    public class Fish
    {
        public int id { get; set; }
        public string name { get; set; }
        public double weight { get; set; }
        public Fish(string name, double weight)
        {
            this.name = name;
            this.weight = weight;
        }
    }
}
