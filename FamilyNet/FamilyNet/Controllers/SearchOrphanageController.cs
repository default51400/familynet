using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FamilyNet.Models;

namespace FamilyNet.Controllers
{
    public class SearchOrphanageController : Controller
    {
        public IActionResult Index() {

            return View();
        }

       
    }
}