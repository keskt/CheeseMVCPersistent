using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheeseMVC.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CheeseMVC.ViewModels
{
    public class AddMenuItemViewModel
    {
        [Display(Name = "Cheese")]
        public int CheeseID { get; set; }
        public List<SelectListItem> Cheeses { get; set; } = new List<SelectListItem>();
        public int MenuID { get; set; }
        public Menu Menu { get; set; }

        public AddMenuItemViewModel() { }

        public AddMenuItemViewModel(Menu menu, List<Cheese> cheeses) : this()
        {
            foreach (Cheese cheese in cheeses)
            {
                Cheeses.Add(new SelectListItem
                {
                    Value = cheese.ID.ToString(),
                    Text = cheese.Name
                });
            }

            Menu = menu;
        }

    }
}