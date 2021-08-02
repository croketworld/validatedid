using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public static class InMemoryStore
    {

        public static List<Product> AllProducts(EProductType filterType = EProductType.None) { 
            List<Product> Result = new List<Product>();
            Result.AddRange(new Product[]{
                new Product(10001,"Lord of the Rings",EProductType.Book, 10.00),
                new Product(10002,"The Hobbit",EProductType.Book, 5.00),
                 new Product(20001,"Game of Thrones",EProductType.Dvd, 9.00),
                new Product(20110,"Breaking Bad",EProductType.Dvd, 7.00),
            });

            if (filterType != EProductType.None) {
                var resultParsed = Result.Where(product => product.ProductType.HasFlag(filterType)).ToList();
                Result = resultParsed;
            }

            return Result;
        }

    }
}
