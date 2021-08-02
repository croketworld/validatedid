using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{


    public class ShoppingCartModel :List<Product>
    {

        /// <summary>
        /// The creation time in format dd-MM-YYYY
        /// </summary>
        /// <remarks>Stored as string because we only need a format</remarks>
        public string Creation { get; set; }


        /// <summary>
        /// This function can be overloaded or extended
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string text = "Creation date: " + Creation + Environment.NewLine;
            double totalPrice = 0.00;
            Dictionary<string, int> products = new Dictionary<string, int>();

            foreach (Product product in this) {
                if (products.ContainsKey(product.Title))
                {
                    products[product.Title] += 1;
                }
                else {
                    products.Add(product.Title, 1);
                }
                totalPrice += product.Price.Price;
            }

            foreach (KeyValuePair<string,int> key in products) {
                string title = key.Key;
                int units = key.Value;
                double price = this.FirstOrDefault(pro => pro.Title == title).Price.Price;
                text += string.Format("- {0}x   {1}   //  {2} x {3} = €{4}{5}",
                    units, title, units, price.ToString("F"), string.Format("{0:0.00}", price * units), Environment.NewLine);
            }
            text += "- Total: €" + totalPrice.ToString("F");
            return text;
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <remarks>Adds the creation time</remarks>
        public ShoppingCartModel() {
            this.Creation = DateTime.Now.ToString("dd-MM-yyyy");
        }

    }
}
