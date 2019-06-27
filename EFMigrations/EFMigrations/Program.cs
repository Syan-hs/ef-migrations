using System;
using System.Linq;
using EFMigrations.DataAccess;

namespace EFMigrations
{
    class Program
    {
        static void Main(string[] args)
        {

            using (EFMigrationContext context = new EFMigrationContext())
            {


                var dealers = context.Dealers.ToList();
                foreach (var dealer in dealers)
                {
                    Console.WriteLine($"----------------Dealer---------------------");
                    Console.WriteLine($"DealerId:{dealer.Id}: DealerName:{dealer.Name} - IsActive:{dealer.IsActive} ");
                    Console.WriteLine($"Dealer Location:{dealer?.DealerLocation.AddressLine1} City{dealer?.DealerLocation.City}");
                    Console.WriteLine($"Manufacturers {string.Join(", ", dealer.Makes.Select(s => s.Name).ToList())}");

                }


                foreach (var make in context.Makes.ToList())
                {
                    Console.WriteLine($"----------------MakeInfo---------------------");

                    Console.WriteLine($"MakeId:{make.Id}: Name:{make.Name} ");
                    Console.WriteLine($"Models: {string.Join(", ", make.Models.Select(s => s.Name).ToList())}");
                    Console.WriteLine($"Dealers: {string.Join(", ", make.Dealers.Select(s => s.Name).ToList())}");
                }

            }

            Console.ReadKey();
        }
    }
}
