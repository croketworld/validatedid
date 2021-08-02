
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApplication1.Models;
namespace WebApplication1.Controllers
{



    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {

        #region "data & data functions"

        private readonly string _basketCreated = "[BASKET CREATED]: Created[<'{0}'>], {1}";
        private readonly string _itemAdded = "[ITEM ADDED TO SHOPPING CART]: Added[<'{0}'>], {1}, {2}, {3}u[{4}[<€{5}>]";
        private readonly string _userId = "12345";
        /// <summary>
        /// This private function actually don`t do anything at all, but can be used to store in session, cookies, aplication or wathever. very useful to cache data from DB
        /// </summary>
        /// <returns></returns>
        private List<Product> AllProducts() {
            return InMemoryStore.AllProducts();
        }

        ShoppingCartModel shoppingCart;
        private ShoppingCartModel ShoppingCart() {
            AppDomain domain = AppDomain.CurrentDomain;
            if (shoppingCart == null) { 
                shoppingCart = new ShoppingCartModel();
                try
                {
                    object tempData = domain.GetData("ShoppingCart_" + _userId);
                    if (tempData != null && tempData.GetType() == shoppingCart.GetType())
                    {
                        shoppingCart = (ShoppingCartModel)tempData;
                    }
                }
                catch { }
               
                Log(
                    string.Format(_basketCreated,
                        System.DateTime.Now.ToString("dd-MM-yyyy"),
                        _userId
                       
                    )
                );
            }
            
            domain.SetData("ShoppingCart_" + _userId,shoppingCart);
            return shoppingCart;

        }

        #endregion


        [HttpGet(Order = 1)]
        public IEnumerable<Product> Get()
        {
           return AllProducts();
        }

        /// <summary>
        /// Function to retrieve the shopping cart for a view or partial view
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("shoppingcart")]
        public string GetShoppingCart() {
            Response.ContentType = "text/plain";
            ShoppingCartModel result = ShoppingCart();
            return result.ToString();
            
        }

        [HttpGet]
        [Route("product/{id}")]
        public Product GetProduct(int id)
        {
            int idToSearch = id;//moved to another variable for security checkings and parsing values
            return AllProducts().FirstOrDefault(product => product.Id == idToSearch);
        }

        /// <summary>
        /// Add a product to the shopping cart and return to the index
        /// </summary>
        /// <param name="id">The identifier for the product to add</param>
        /// <returns></returns>
        /// <remarks>Must be placed a remove function</remarks>
        [HttpGet]
        [Route("add/{id}/{units}")]
        public ObjectResult AddProduct(int id,int units = 1) {
            ShoppingCartModel _shoppingCart = ShoppingCart();
            int idToSearch = id;//moved to another variable for security checkings and parsing values
            Product productToAdd = AllProducts().FirstOrDefault(product => product.Id == idToSearch);
            if (productToAdd != null && productToAdd.Id > 0)
            {
                for (int i = 0; i < units; i++)
                {
                    _shoppingCart.Add(productToAdd);
                }
              
            }
            else {
                return NotFound("Product not found");
            }
            shoppingCart = _shoppingCart;
            Log(
                string.Format(_itemAdded,
                    System.DateTime.Now.ToString("dd-MM-yyyy"),
                    _userId,
                    idToSearch,
                    units,
                    (double)productToAdd.Price,
                   System.Convert.ToString(productToAdd.Price * units)
                )
            );
            return new OkObjectResult(idToSearch);

        }


        #region "logging"
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(ILogger<ProductsController> logger)
        {
            _logger = logger;
        }

        private bool Log(string content) {
            bool okResult = false;
            try {
                _logger.LogInformation(content);
                System.Diagnostics.Debug.Write(content + System.Environment.NewLine);
                okResult = true;
            }
            catch { }
            return okResult;
        }

        #endregion
    }
}
