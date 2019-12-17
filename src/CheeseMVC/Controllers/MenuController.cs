using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CheeseMVC.Data;
using CheeseMVC.Models;
using CheeseMVC.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CheeseMVC.Controllers
{
    public class MenuController : Controller
    {
        private readonly CheeseDbContext context;

        public MenuController(CheeseDbContext dbContext)
        {
            context = dbContext;
        }

        public IActionResult Index()
        {
            IList<Menu> menus = context.Menus.ToList();
            return View(menus);
        }

        public IActionResult Add()
        {
            AddMenuViewModel addMenuViewModel = new AddMenuViewModel();
            return View(addMenuViewModel);
        }

        [HttpPost]
        public IActionResult Add(AddMenuViewModel addMenuViewModel)
        {
            if (ModelState.IsValid)
            {
                Menu newMenu = new Menu
                {
                    Name = addMenuViewModel.Name
                };

                context.Menus.Add(newMenu);
                context.SaveChanges();

                return Redirect($"/Menu/ViewMenu/{newMenu.ID}");
            }

            return View(addMenuViewModel);
        }

        public IActionResult ViewMenu(int id)
        {
            if (id == 0)
            {
                return Redirect("/");
            }

            Menu theMenu = context.Menus.Single(m => m.ID == id);
            List<CheeseMenu> items = context.CheeseMenus.Include(item => item.Cheese).Where(cm => cm.MenuID == id).ToList();

            ViewMenuViewModel viewMenuViewModel = new ViewMenuViewModel
            {
                Menu = theMenu,
                Items = items
            };

            ViewBag.Title = $"MENU: {theMenu.Name}";

            return View(viewMenuViewModel);
        }

        public IActionResult AddItem(int id)
        {
            IList<Menu> menu = context.Menus.Where(m => m.ID == id).ToList();
            if (menu.Count == 1)
            {
                List<Cheese> cheeses = context.Cheeses.ToList();

                AddMenuItemViewModel addMenuItemViewModel = new AddMenuItemViewModel(menu[0], cheeses);

                return View(addMenuItemViewModel);
            }

            return Redirect($"/Menu");
        }

        [HttpPost]
        public IActionResult AddItem(AddMenuItemViewModel addMenuItemViewModel)
        {
            if (ModelState.IsValid)
            {
                int cheeseID = addMenuItemViewModel.CheeseID;
                int menuID = addMenuItemViewModel.MenuID;

                IList<CheeseMenu> existingItems = context.CheeseMenus
                                                    .Where(cm => cm.CheeseID == cheeseID)
                                                    .Where(cm => cm.MenuID == menuID)
                                                    .ToList();

                if (existingItems.Count == 0)
                {
                    CheeseMenu newMenuItem = new CheeseMenu
                    {
                        //Cheese = context.Cheeses.Single(c => c.ID == cheeseID),
                        //Menu = context.Menus.Single(m => m.ID == menuID)
                        CheeseID = cheeseID,
                        MenuID = menuID
                    };

                    context.CheeseMenus.Add(newMenuItem);
                    context.SaveChanges();
                }
                //else
                //{
                //    // The item is already on the menu
                //    // Redirect back to the menu 
                //    // (or, alternatively, redirect back to the Add Item form to choose a different item)

                //    //return View(addMenuItemViewModel);
                //    return Redirect($"/Menu/ViewMenu/{addMenuItemViewModel.MenuID}");
                //}

                return Redirect($"/Menu/ViewMenu/{addMenuItemViewModel.MenuID}");
            }

            return View(addMenuItemViewModel);
        }
    }
}