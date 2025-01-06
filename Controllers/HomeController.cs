using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        string constring = "Data Source=.;Initial Catalog=orderingdb;Integrated Security=true;";

        public ActionResult Index()
        {
            return View();
        }

        //List product code to dropdown
        public JsonResult ListProductData()
        {
            return Json(ListProduct(), JsonRequestBehavior.AllowGet);
        }

        //Return product code method
        public List<Product> ListProduct()
        {
            List<Product> product = new List<Product>();
            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT ProductID,ProductCode FROM Product", con);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    product.Add(new Product
                    {
                        ProductID = Convert.ToInt32(rdr["ProductId"]),
                        ProductCode = rdr["ProductCode"].ToString(),
                    });
                }
                return product;
            }
        }

        //List customer code to dropdown
        public JsonResult ListCustomerData()
        {
            return Json(ListCustomer(), JsonRequestBehavior.AllowGet);
        }

        //Return customer code method
        public List<Customer> ListCustomer()
        {
            List<Customer> customer = new List<Customer>();
            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT CustomerID, CustomerCode FROM Customer", con);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    customer.Add(new Customer
                    {
                        CustomerID = Convert.ToInt32(rdr["CustomerId"]),
                        CustomerCode = rdr["CustomerCode"].ToString(),
                    });
                }
                return customer;
            }
        }

        //Get customer name to label
        public JsonResult getCustomerName(string customerid)
        {
            return Json(getCustomerNamebyCode(customerid), JsonRequestBehavior.AllowGet);
        }

        //Return customer name
        public string getCustomerNamebyCode(string customerid)
        {
            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT CustomerName FROM Customer WHERE CustomerID ='"+ customerid + "'", con);
                SqlDataReader rdr = cmd.ExecuteReader();
                string CustomerName = "";
                while (rdr.Read())
                {
                     CustomerName = rdr["CustomerName"].ToString();
                }
                return CustomerName;
            }
        }

        //Get unit price by product code
        public JsonResult getUnitPrice(string productcode)
        {
            return Json(gettUnitPricebyid(productcode), JsonRequestBehavior.AllowGet);
        }

        //Return unit price
        public decimal gettUnitPricebyid(string productcode)
        {
            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT UnitPrice FROM Product WHERE ProductCode ='" + productcode + "'", con);
                SqlDataReader rdr = cmd.ExecuteReader();
                decimal UnitPrice = 0;
                while (rdr.Read())
                {
                    UnitPrice = Convert.ToInt32(rdr["UnitPrice"]);
                }
                return UnitPrice;
            }
        }

        //Add order 
        public JsonResult AddOrder(SubOrder ord)
        {
            return Json(AddOrderDetails(ord), JsonRequestBehavior.AllowGet);
        }

        //Add order method
        public int AddOrderDetails(SubOrder ord)
        {
            int i = 0;
            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO suborder(ProductCode, Qty, ItemTotal) values(@ProductCode, @Qty, @ItemTotal)", con);
                foreach(var dt in ord.Data)
                {
                    cmd.Parameters.AddWithValue("@ProductCode", dt[0]);
                    cmd.Parameters.AddWithValue("@Qty", dt[1]);
                    cmd.Parameters.AddWithValue("@ItemTotal", dt[2]);
                    cmd.Parameters.AddWithValue("@Action", "Insert");
                    i = cmd.ExecuteNonQuery();
                }
                
                if(ord.SubTotal != 0)
                {
                    SqlCommand cmd2 = new SqlCommand("INSERT INTO order(CustomerCode, SubTotal, DiscountPrice, NetTotal) values(@CustomerCode, @SubTotal, @DisPrice, @NetTotal)", con);
                    cmd2.Parameters.AddWithValue("@CustomerCode", ord.CustomerID);
                    cmd2.Parameters.AddWithValue("@SubTotal", ord.SubTotal);
                    cmd2.Parameters.AddWithValue("@DisPrice", ord.DiscountPrice);
                    cmd2.Parameters.AddWithValue("@NetTotal", ord.NetTotal);
                    cmd2.Parameters.AddWithValue("@Action", "Insert");
                    i = cmd.ExecuteNonQuery();
                }


            }
            return i;
        }

        //List order
        public JsonResult ListOrder()
        {
            return Json(ListAllOrder(), JsonRequestBehavior.AllowGet);
        }

        //Return order method
        public List<SubOrder> ListAllOrder()
        {
            List<SubOrder> ordlst = new List<SubOrder>();
            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("  Select s.OrderNo, s.ProductCode, s.Qty, p.UnitPrice, s.ItemTotal From suborder s INNER JOIN product p ON s.ProductCode = p.ProductCode ", con);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    ordlst.Add(new SubOrder
                    {
                        OrderNo = Convert.ToInt32(rdr["OrderNo"]),
                        ProductCode = rdr["ProductCode"].ToString(),
                        Qty= Convert.ToInt32(rdr["Qty"]),
                        UnitPrice = Convert.ToDecimal(rdr["UnitPrice"]),
                        ItemTotal = Convert.ToDecimal(rdr["ItemTotal"]),
                    });
                }
                return ordlst;
            }
        }

        //Order record get by ID
        public JsonResult GetbyID(int ID)
        {
            var SubOrder = ListAllOrder().Find(x => x.OrderNo.Equals(ID));
            return Json(SubOrder, JsonRequestBehavior.AllowGet);
        }

        //Update order record
        public JsonResult Update(SubOrder ord)
        {
            return Json(UpdateOrder(ord), JsonRequestBehavior.AllowGet);
        }

        //Updating product record method 
        public int UpdateOrder(SubOrder ord)
        {
            int i;
            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("Update suborder SET ProductCode = @ProductCode, Qty = @Qty, ItemTotal = @Itemtotal WHERE OrderNo = @OrderNo", con);
                cmd.Parameters.AddWithValue("@OrderNo", ord.OrderNo);
                cmd.Parameters.AddWithValue("@ProductCode", ord.ProductCode);
                cmd.Parameters.AddWithValue("@Qty", ord.Qty);
                cmd.Parameters.AddWithValue("@Itemtotal", ord.ItemTotal);
                cmd.Parameters.AddWithValue("@Action", "Update");
                i = cmd.ExecuteNonQuery();
            }
            return i;
        }

        //Delete order record
        public JsonResult Delete(int ID)
        {
            return Json(DeleteOrder(ID), JsonRequestBehavior.AllowGet);
        }

        //Delete order record method
        public int DeleteOrder(int ID)
        {
            int i;
            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("Delete FROM suborder WHERE OrderNo = @OrderNo", con);
                cmd.Parameters.AddWithValue("@OrderNo", ID);
                i = cmd.ExecuteNonQuery();
            }
            return i;
        }
    }
}