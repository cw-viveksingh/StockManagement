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
            try
            {

                MemcachedClient mc = new MemcachedClient();
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
            return View("~/Views/Home/StockDetails.cshtml", obj);
        }
    }
}