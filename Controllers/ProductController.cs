using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class ProductController : Controller
    {
        string constring = "Data Source=.;Initial Catalog=orderingdb;Integrated Security=true;";

        // GET: Product
        public ActionResult Index()
        {
            return View();
        }

        //Product
        public ActionResult Product()
        {
            return View();
        }

        //List all product records
        public JsonResult List()
        {
            return Json(ListAllProduct(), JsonRequestBehavior.AllowGet);
        }

        //Return list of all products method
        public List<Product> ListAllProduct()
        {
            List<Product> prolst = new List<Product>();
            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM product", con);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    prolst.Add(new Product
                    {
                        ProductID = Convert.ToInt32(rdr["ProductId"]),
                        ProductCode = rdr["ProductCode"].ToString(),
                        ProductName = rdr["ProductName"].ToString(),
                        Quantity = Convert.ToInt32(rdr["Quantity"]),
                        UnitPrice = Convert.ToDecimal(rdr["UnitPrice"]),
                    });
                }
                return prolst;
            }
        }

        //Insert product record
        public JsonResult Add(Product pro)
        {
            return Json(AddProduct(pro), JsonRequestBehavior.AllowGet);
        }

        //Insert a product method
        public int AddProduct(Product pro)
        {
            int i;
            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Product(ProductCode, ProductName, Quantity, UnitPrice) values(@ProductCode, @ProductName, @Quantity, @UnitPrice)", con);
                cmd.Parameters.AddWithValue("@ProductCode", pro.ProductCode);
                cmd.Parameters.AddWithValue("@ProductName", pro.ProductName);
                cmd.Parameters.AddWithValue("@Quantity", pro.Quantity);
                cmd.Parameters.AddWithValue("@UnitPrice", pro.UnitPrice);
                cmd.Parameters.AddWithValue("@Action", "Insert");
                i = cmd.ExecuteNonQuery();
            }
            return i;
        }

        //Product record get by ID
        public JsonResult GetbyID(int ID)
        {
            var Product = ListAllProduct().Find(x => x.ProductID.Equals(ID));
            return Json(Product, JsonRequestBehavior.AllowGet);
        }

        //Update product record
        public JsonResult Update(Product pro)
        {
            return Json(UpdateProduct(pro), JsonRequestBehavior.AllowGet);
        }

        //Updating product record method 
        public int UpdateProduct(Product pro)
        {
            int i;
            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("Update Product SET ProductCode = @ProductCode, ProductName = @ProductName, Quantity = @Quantity, UnitPrice = @UnitPrice WHERE ProductID = @ProductID", con);
                cmd.Parameters.AddWithValue("@ProductID", pro.ProductID);
                cmd.Parameters.AddWithValue("@ProductCode", pro.ProductCode);
                cmd.Parameters.AddWithValue("@ProductName", pro.ProductName);
                cmd.Parameters.AddWithValue("@Quantity", pro.Quantity);
                cmd.Parameters.AddWithValue("@UnitPrice", pro.UnitPrice);
                cmd.Parameters.AddWithValue("@Action", "Update");
                i = cmd.ExecuteNonQuery();
            }
            return i;
        }

        //Delete product record
        public JsonResult Delete(int ID)
        {
            return Json(DeleteProduct(ID), JsonRequestBehavior.AllowGet);
        }

        //Delete product record method
        public int DeleteProduct(int ID)
        {
            int i;
            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("Delete FROM Product WHERE ProductID = @ProductId", con);
                cmd.Parameters.AddWithValue("@ProductId", ID);
                i = cmd.ExecuteNonQuery();
            }
            return i;
        }
    }
}