using FamilyNet.Models.EntityFramework;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyNet.Models {
    public class SeedData {


        private static Dictionary<int, string> _forAddress = new Dictionary<int, string> {
            [1] = "Украина",
            [3] = "Днепр",
            [2] = "Днепропетровская"
        };

        //public static void EnsurePopulated(IApplicationBuilder app) {
        //    ApplicationDbContext _context = app.ApplicationServices
        //        .GetRequiredService<ApplicationDbContext>();
        //    _context.Database.Migrate();
        //    if (!_context.Addresses.Any()) {
        //        _context.AddRange(
        //            new Address {
        //                Country = _forAddress[1],
        //                Region = _forAddress[2],
        //                City = _forAddress[3],
        //                Street = "Инженерная",
        //                House = "10"
        //            },
        //             new Address {
        //                 Country = _forAddress[1],
        //                 Region = _forAddress[2],
        //                 City = _forAddress[3],
        //                 Street = "Петразаводская",
        //                 House = "371"
        //             },

        //             new Address {
        //                 Country = _forAddress[1],
        //                 Region = _forAddress[2],
        //                 City = _forAddress[3],
        //                 Street = "Надежды Алексеенко",
        //                 House = "371"
    //}
    //             });
    //    }
}
    }




