using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{

    #region "product class"

    /// <summary>
    /// Product class with int Id
    /// </summary>
    public class Product : Product<int> {


        /// <summary>
        /// Base constructor
        /// </summary>
        public Product() { }

        /// <summary>
        /// Constructor inherated
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        public Product(int id, string title, EProductType type = EProductType.None) :base(id, title,type)
        {
            
        }

        /// <summary>
        /// Constructor inherited
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="type"></param>
        /// <param name="price"></param>
        public Product(int id, string title, EProductType type,double price) : base(id, title, type,price)
        {
            
        }


    }

    /// <summary>
    /// Product base class
    /// </summary>
    /// <typeparam name="Tid">I make this to ensure further compatibility and for make generic functions, if the store is to large, can be used a long (int64) instead int (int32) or any other numerical value</typeparam>
    public class Product<Tid>
    {

        #region "properties"

        /// <summary>
        /// The unique identifier
        /// </summary>
        public Tid Id { get; set; }

        /// <summary>
        /// The title of the product
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The type for this product
        /// </summary>
        public EProductType ProductType { get; set; }

        /// <summary>
        /// This would be a virtual reference as a 1:1 relation
        /// </summary>
        public virtual ProductPrice<Tid> Price { get; set;}

        #endregion
        ///I use a region for better reading, the best practice is to make partial classes

        #region "constructors"

        /// <summary>
        /// Base constructor
        /// </summary>
        public Product() { }

        /// <summary>
        /// Constructor class for fill the values for the model. All properties should be validated before fill the object
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        public Product(Tid id, string title, EProductType type = EProductType.None) {
            this.Id = id;
            this.Title = title;
            this.ProductType = type;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="price"></param>
        public Product(Tid id, string title, EProductType type, double price)
        {
            this.Id = id;
            this.Title = title;
            this.ProductType = type;
            this.Price = new ProductPrice<Tid>() { Price = price, ProductId = id };
        }
        #endregion
    }


    #endregion

    #region "product properties classes"
    /// <summary>
    /// Normally, this kind of values should be as an satelitar class in a 1:1 or 1:n relation
    /// </summary>
    /// <typeparam name="Tid">The type for use as Id</typeparam>
    /// <remarks>an explicit conversion function must be added to use Product.Price instead Product.Price.Price</remarks>
    public class ProductPrice<Tid> { 

        /// <summary>
        /// The product related
        /// </summary>
        public Tid ProductId { get; set; }

        /// <summary>
        /// Price in Euro
        /// </summary>
        public double Price { get; set; }

        public static implicit operator double(ProductPrice<Tid> productprice) {
            return productprice.Price;
        }

    }

    #endregion

    #region "enumerations"

    /// <summary>
    /// An enumeration with all the product kinds
    /// </summary>
    /// <remarks>It used the attribute class Flags for use binary operators as band, the next values must be x2 (4, 8, 16, 32..) it would be useful when a product can be many types at the same time</remarks>
    [Flags]
    public enum EProductType { 
        None = 0,
        Book = 1,
        Dvd = 2
    }

    #endregion

    ///I use regions instead many project files only for this test as a easy to read demo
}
