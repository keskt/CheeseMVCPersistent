﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheeseMVC.Models
{
    public class CheeseCategory
    {
        public string Name { get; set; }
        public int ID { get; set; }

        public IList<Cheese> Cheeses { get; set; }
    }
}