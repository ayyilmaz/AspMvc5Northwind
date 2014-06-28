using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AspMvc5Northwind.Models;
using AspMvc5Northwind.ViewModels;
using PagedList;

namespace AspMvc5Northwind.Controllers
{
    public class ProductsController : Controller
    {
        private NorthwindEntities db = new NorthwindEntities();

        // GET: Products
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            int pageNum = page ?? 1;
            int resultsPerPage = 5;

            // Keep track of filters / sort
            ViewBag.CurrentSort = sortOrder;

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            
            // Toggle the sort params
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "nameDesc" : "";
            ViewBag.PriceSortParm = sortOrder == "unitPrice" ? "unitPriceDesc" : "unitPrice";

            // Get products to show based on input parameters
            // Lazy load: get products, later queries can get related data
            // This results in a database query per product listed in the view
            //var products = from p in db.Products select p;

            // Eager load: load products and related data in one go
            // Results in 2 queries (why not one?), regardless of the number of 
            // products listed in the view.
            var products = db.Products
                .Include(p => p.Supplier)
                .Include(p => p.Category);

            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.ProductName.ToUpper().Contains(searchString.ToUpper()));
            }

            // NOTE: paged list requires sorting
            switch(sortOrder)
            {
                case "nameDesc":
                    products = products.OrderByDescending(p => p.ProductName);
                    break;
                case "unitPrice":
                    products = products.OrderBy(p => p.UnitPrice);
                    break;
                case "unitPriceDesc":
                    products = products.OrderByDescending(p => p.UnitPrice);
                    break;
                default:
                    products = products.OrderBy(p => p.ProductName);
                    break;
            }
            
            return View(products.ToPagedList(pageNum, resultsPerPage));
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            PopulateCategoryDropDownList();
            PopulateSupplierDropDownList();
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,ProductName,SupplierID,CategoryID,QuantityPerUnit,UnitPrice,UnitsInStock,UnitsOnOrder,ReorderLevel,Discontinued")] Product product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Products.Add(product);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                // Log the error (uncomment dex variable name and add a line here to write a log.)
                ModelState.AddModelError("", 
                    "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }

            PopulateCategoryDropDownList(product.CategoryID);
            PopulateSupplierDropDownList(product.SupplierID);
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            PopulateCategoryDropDownList(product.CategoryID);
            PopulateSupplierDropDownList(product.SupplierID);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,ProductName,SupplierID,CategoryID,QuantityPerUnit,UnitPrice,UnitsInStock,UnitsOnOrder,ReorderLevel,Discontinued")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            PopulateCategoryDropDownList(product.CategoryID);
            PopulateSupplierDropDownList(product.SupplierID);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Products/Stats
        // uses the ProductCategoryGroup ViewModel to represent product data 
        public ActionResult Stats(int? id)
        {
            var categories = from product in db.Products
                             join category in db.Categories
                             on product.CategoryID equals category.CategoryID
                             group product by category.CategoryName into g
                             select new ProductCategoryGroup()
                             {
                                 Category = g.Key,
                                 ProductCount = g.Count()
                             };
            return View(categories.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        // -----------------------------------------------------------------------------
        // private parts

        private void PopulateCategoryDropDownList(object selectedCategory = null)
        {
            var categories = from c in db.Categories
                             orderby c.CategoryName
                             select c;
            ViewBag.CategoryID = new SelectList(categories, "CategoryID", "CategoryName", selectedCategory);
        }

        private void PopulateSupplierDropDownList(object selectedSupplier = null)
        {
            var suppliers = from s in db.Suppliers
                            orderby s.CompanyName
                            select s;
            ViewBag.SupplierID = new SelectList(suppliers, "SupplierID", "CompanyName", selectedSupplier);
        }
    }
}
