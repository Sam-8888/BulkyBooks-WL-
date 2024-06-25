using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private ApplicationDBContext _db;

        public ProductRepository(ApplicationDBContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Product obj)
        {
            _db.Products.Update(obj);
            //return;
            //var objfromdb = _db.Products.FirstOrDefault(u => u.Id == obj.Id);
            //if (objfromdb != null)
            //{
            //    objfromdb.Title = obj.Title;
            //    objfromdb.ISBN = obj.ISBN;
            //    objfromdb.Price = obj.Price;
            //    objfromdb.Price50 = obj.Price50;
            //    objfromdb.ListPrice = obj.ListPrice;
            //    objfromdb.Price100 = obj.Price100;
            //    objfromdb.Description = obj.Description;
            //    objfromdb.CategoryId = obj.CategoryId;
            //    objfromdb.Author = obj.Author;
            //    objfromdb.CoverTypeId = obj.CoverTypeId;
            //    objfromdb.ImageUrl = obj.ImageUrl ?? objfromdb.ImageUrl;
            //}
        }
    }
}
