using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class CustomerController : Controller
    {
        string constring = "Data Source=.;Initial Catalog=orderingdb;Integrated Security=true;";

        // GET: Customer
        public ActionResult Index()
        {
            return View();
        }

        //Customer
        public ActionResult Customer()
        {
            return View();
        }
        
        //List all customer records
        public JsonResult List()
        {
            return Json(ListAllCustomer(), JsonRequestBehavior.AllowGet);
        }

        //Return list of all customers method
        public List<Customer> ListAllCustomer()
        {
            List<Customer> cuslst = new List<Customer>();
            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM customer", con);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    cuslst.Add(new Customer
                    {
                        CustomerID = Convert.ToInt32(rdr["CustomerId"]),
                        CustomerCode = rdr["CustomerCode"].ToString(),
                        CustomerName = rdr["CustomerName"].ToString(),
                    });
                }
                return cuslst;
            }
        }

        //Insert customer record
        public JsonResult Add(Customer cus)
        {
            return Json(AddCustomer(cus), JsonRequestBehavior.AllowGet);
        }

        //Insert a customer method
        public int AddCustomer(Customer cus)
        {
            int i;
            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO customer(CustomerCode, CustomerName) values(@CustomerCode, @CustomerName)", con);
                cmd.Parameters.AddWithValue("@CustomerCode", cus.CustomerCode);
                cmd.Parameters.AddWithValue("@CustomerName", cus.CustomerName);
                cmd.Parameters.AddWithValue("@Action", "Insert");
                i = cmd.ExecuteNonQuery();
            }
            return i;
        }

        //Customer record get by ID
        public JsonResult GetbyID(int ID)
        {
            var Customer = ListAllCustomer().Find(x => x.CustomerID.Equals(ID));
            return Json(Customer, JsonRequestBehavior.AllowGet);
        }

        //Update customer record
        public JsonResult Update(Customer cus)
        {
            return Json(UpdateCustomer(cus), JsonRequestBehavior.AllowGet);
        }

        //Updating customer record method 
        public int UpdateCustomer(Customer cus)
        {
            int i;
            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("Update customer SET CustomerCode = @CustomerCode, CustomerName = @CustomerName WHERE CustomerID = @CustomerID", con);
                cmd.Parameters.AddWithValue("@CustomerID", cus.CustomerID);
                cmd.Parameters.AddWithValue("@CustomerCode", cus.CustomerCode);
                cmd.Parameters.AddWithValue("@CustomerName", cus.CustomerName);
                cmd.Parameters.AddWithValue("@Action", "Update");
                i = cmd.ExecuteNonQuery();
            }
            return i;
        }

        //Delete customer record
        public JsonResult Delete(int ID)
        {
            return Json(DeleteCustomer(ID), JsonRequestBehavior.AllowGet);
        }

        //Delete customer record method
        public int DeleteCustomer(int ID)
        {
            int i;
            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("Delete FROM customer WHERE CustomerID = @CustomerId", con);
                cmd.Parameters.AddWithValue("@CustomerId", ID);
                i = cmd.ExecuteNonQuery();
            }
            return i;
        }
    }
}