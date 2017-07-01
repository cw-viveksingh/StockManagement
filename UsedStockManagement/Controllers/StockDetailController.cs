using Dapper;
using Enyim.Caching;
using Enyim.Caching.Memcached;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UsedStockManagement.Models;

namespace UsedStockManagement.Controllers
{
    public class StockDetailController : Controller
    {
        // GET: StockDetail
        public ActionResult Info(int id)
        {
            var par = new DynamicParameters();
            par.Add("par_id", id);
            Details obj = new Details();
            MemcachedClient mc = new MemcachedClient();
            try
            {

                
                obj = (Details)mc.Get("memid" + id.ToString());
                if (obj == null)
                {
                    IDbConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["myconn"].ConnectionString);
                    using (var multi = connection.QueryMultiple("getdata", par, commandType: CommandType.StoredProcedure))
                    {
                        obj = multi.Read<Details>().First();
                    }
                    mc.Store(StoreMode.Add, "memid" + id.ToString(), obj, DateTime.Now.AddSeconds(600000));
                }


            }
            catch (Exception e)
            {

                throw;
            }
            UsedCarStock usedCarStock = new UsedCarStock();
            usedCarStock.FuelType = (String)mc.Get("usFuelType" + obj.fuelType.ToString());
            usedCarStock.City = (String)mc.Get("usCity" + obj.city.ToString());
            usedCarStock.Color = (String)mc.Get("usColor" + obj.color.ToString());
            usedCarStock.Make = (String)mc.Get("usMake" + obj.make.ToString());
            usedCarStock.Model = (String)mc.Get("usModel" + obj.model.ToString());
            usedCarStock.Version = (String)mc.Get("usVersion" + obj.version.ToString());
            usedCarStock.Price = obj.price;
            usedCarStock.Kilometer = obj.kilometer;
            usedCarStock.Year = obj.yer;
            usedCarStock.FuelEconomy = (decimal)(obj.fuelEconomy == -1 ? -1:(obj.fuelEconomy) );
            return View("~/Views/Home/StockDetails.cshtml", usedCarStock);
        }
    }
}