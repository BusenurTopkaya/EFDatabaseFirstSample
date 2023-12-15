using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EFDatabaseFirstSample
{
    public class ProductDal
    {
        public List<Product> GetAll()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                //IEnumarable dönüyor; collection a select atıldı
               // return context.Products.ToList().Where(x => x.ProductName.Contains("c")
               //).ToList();

                return context.Products.ToList();
            }
        }

        public List<Product> GetByName(string ifade)
        {
            //IQueryable dönüyor; direkt DB'e select atıldı
            using (DatabaseContext context = new DatabaseContext())
            {
                return context.Products.ToList().Where(c => c.ProductName.ToLower().Contains(ifade.ToLower())).ToList();
            }
        }

        public Product GetById(int id)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return context.Products.FirstOrDefault(p => p.ProductID == id);
                //return context.Products.SingleOrDefault(p => p.ProductID == id);
            }
        }

        public void Add(Product product)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                //context.Products.Add(product);
                var entity = context.Entry(product);
                entity.State = EntityState.Added;
                context.SaveChanges();
            }
        }

        public void Update(Product product)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                var entity = context.Entry(product);
                entity.State = EntityState.Modified;
                context.SaveChanges();
            }

        }

        public void Delete(Product product)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                var entity = context.Entry(product);
                entity.State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        

    }
}
