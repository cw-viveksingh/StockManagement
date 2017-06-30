using Dapper;
using Enyim.Caching;
using Enyim.Caching.Memcached;
using MySql.Data.MySqlClient;
using Nest;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class StockController : ApiController
    {
        private static ElasticClient elasticClient = ElasticClientInstance.GetInstance();
        [HttpPost]
        public IHttpActionResult Post([FromBody]Stocks stockObj)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (!(stockObj.price >= 10000 && stockObj.price <= 30000000))
                    return BadRequest("Please Enter a valid price b/w 10000 and 3 Crore");

                if (!(stockObj.year >= 2010 && stockObj.year <= DateTime.Today.Year))
                    return BadRequest("Please Enter a valid Date b/w 2010 and " + DateTime.Today.Year);

                if (!(stockObj.kilometer >= 100 && stockObj.kilometer <= 300000))
                    return BadRequest("Please Enter a valid Kilometers b/w 100 and 3 Lakh");

                if (stockObj.fuelEconomy != -1)
                    if (!(stockObj.fuelEconomy >= 1 && stockObj.fuelEconomy <= 50))
                        return BadRequest("Please Enter a valid Fuel Economy b/w 1 and 50 km");


                var par = new DynamicParameters();
                par.Add("color_id", stockObj.color);
                par.Add("model_id", stockObj.model);
                par.Add("city_id", stockObj.city);
                par.Add("make_id", stockObj.make);
                par.Add("fuel_id", stockObj.fuelType);
                par.Add("version_id", stockObj.version);

                Entity Obj = new Entity();

                IDbConnection connectionOne = new MySqlConnection(ConfigurationManager.ConnectionStrings["myconn"].ConnectionString);
                using (var multi = connectionOne.QueryMultiple("verify", par, commandType: CommandType.StoredProcedure))
                {

                    Obj = multi.Read<Entity>().First();

                }

                int check = Obj.color * Obj.model * Obj.fuel * Obj.city * Obj.make * Obj.version;
                if (check != 1)
                    return BadRequest("Wrong Parameters");
                par = new DynamicParameters();
                par.Add("price", stockObj.price);
                par.Add("yer", stockObj.year);
                par.Add("kilometer", stockObj.kilometer);
                par.Add("fuel_type", stockObj.fuelType);
                par.Add("model", stockObj.model);
                par.Add("city", stockObj.city);
                par.Add("color", stockObj.color);
                par.Add("fueleconomy", stockObj.fuelEconomy);
                par.Add("version", stockObj.version);
                par.Add("make", stockObj.make);
                IDbConnection connectionTwo = new MySqlConnection(ConfigurationManager.ConnectionStrings["myconn"].ConnectionString);
                Db temp = new Db();
                //connectionTwo.Execute("table_insert", par, commandType: CommandType.StoredProcedure);
                using (var multi = connectionTwo.QueryMultiple("table_insert", par, commandType: CommandType.StoredProcedure))
                {
                    temp = multi.Read<Db>().First();
                }
                stockObj.stockId = temp.Id;
                var index = elasticClient.Index(stockObj, i => i
                                    .Index("usedstock")
                                    .Type("usedcarstock")
                                    .Id("id" + stockObj.stockId)
                                );
                return Ok("http://localhost:52227/api/Stock/stocks/" + temp.Id);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
        //[HttpDelete]
        [Route("api/Stock/{id}")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                IDbConnection connectionTwo = new MySqlConnection(ConfigurationManager.ConnectionStrings["myconn"].ConnectionString);
                Check obj = new Check();
                var par = new DynamicParameters();
                par.Add("par_id", id);
                try
                {
                    MemcachedClient client = new MemcachedClient();
                    client.Remove("memid" + id.ToString());

                }
                catch (Exception)
                {                    
                    throw;
                } 
                using (var multi = connectionTwo.QueryMultiple("check_exist", par, commandType: CommandType.StoredProcedure))
                {
                    obj = multi.Read<Check>().First();
                }
                if (obj.exist == 0)
                    return BadRequest("The Stock Id You Entered Is Wrong!!!");
                else
                {
                    var index = elasticClient.Delete(elasticClient, i => i
                                    .Index("usedstock")
                                    .Type("usedcarstock")
                                    .Id("id" + id)
                                );
                    return Ok("Data Deleted Successfully"); 
                }
            }
            catch (Exception e)
            {

                return InternalServerError(e);
            }
        }
        bool isValidUpdate(int stockId, Stocks stock)
        {
            //return true;
            if (!(stock.price >= 10000 && stock.price <= 30000000))
            {
                //BadRequest("Please Enter a valid price b/w 10000 and 3 Crore");
                return false;
            }

            if (!(stock.year >= 2010 && stock.year <= DateTime.Today.Year))
            {
                //BadRequest("Please Enter a valid Date b/w 2010 and " + DateTime.Today.Year);
                return false;
            }

            if (!(stock.kilometer >= 100 && stock.kilometer <= 300000))
            {
                //BadRequest("Please Enter a valid Kilometers b/w 100 and 3 Lakh");
                return false;
            }

            if (stock.fuelEconomy != -1)
                if (!(stock.fuelEconomy >= 1 && stock.fuelEconomy <= 50))
                {
                    //BadRequest("Please Enter a valid Fuel Economy b/w 1 and 50 km");
                    return false;
                }
            return true;
        }
        DynamicParameters makeCheckParameters(Stocks stock, int stockId = 0)
        {
            var par = new DynamicParameters();
            if (stockId != 0)
                par.Add("stock_id", stockId);
            par.Add("color_id", stock.color);
            par.Add("model_id", stock.model);
            par.Add("city_id", stock.city);
            par.Add("make_id", stock.make);
            par.Add("fuel_id", stock.fuelType);
            par.Add("version_id", stock.version);
            return par;
        }
        DynamicParameters makeParameters(Stocks stock, int stockId = 0)
        {
            var par = new DynamicParameters();
            if (stockId != 0)
                par.Add("stock_id", stockId);
            par.Add("price", stock.price);
            par.Add("yer", stock.year);
            par.Add("kilometer", stock.kilometer);
            par.Add("fuel_type", stock.fuelType);
            par.Add("model", stock.model);
            par.Add("city", stock.city);
            par.Add("color", stock.color);
            par.Add("fueleconomy", stock.fuelEconomy);
            par.Add("version", stock.version);
            par.Add("make", stock.make);
            return par;
        }
        bool isValidInDB(Stocks stock)
        {
            var par = makeCheckParameters(stock);
            Entity Obj = new Entity();
            IDbConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["myconn"].ConnectionString);
            using (var multi = conn.QueryMultiple("verify", par, commandType: CommandType.StoredProcedure))
            {
                Obj = multi.Read<Entity>().First();
            }
            int check = Obj.color * Obj.model * Obj.fuel * Obj.city * Obj.make * Obj.version;
            if (check != 1)
                return false;
            return true;
        }
        bool updateDb(Stocks stock, int stockId)
        {
            var par = makeParameters(stock, stockId);
            Db temp = new Db();

            try
            {

                using (MemcachedClient mc = new MemcachedClient())
                {
                    var obj = mc.Get("memid" + stockId.ToString());
                    if (obj != null)
                        mc.Remove("memid" + stockId.ToString());
                    IDbConnection connectionTwo = new MySqlConnection(ConfigurationManager.ConnectionStrings["myconn"].ConnectionString);
                    using (var multi = connectionTwo.QueryMultiple("table_update", par, commandType: CommandType.StoredProcedure))
                    {
                        temp = multi.Read<Db>().First();
                    }

                }
                if (temp.Id == 1){
                    stock.stockId = stockId;
                    var index = elasticClient.Index(stock, i => i
                                        .Index("usedstock")
                                        .Type("usedcarstock")
                                        .Id("id" + stock.stockId)
                                    );
                    return true;
                }
                    
                else
                    return false;

            }
            catch (Exception)
            {

                throw;
            }
            // Ok("http://localhost:52227/api/Stock/stocks/" + temp.Id);


        }
        [HttpPut, Route("api/Stock/{stockId}")]
        public IHttpActionResult Put(int stockId, [FromBody]Stocks stock)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                if (isValidUpdate(stockId, stock))
                {
                    /*if (!isValidInDB(stock))
                    {
                        return BadRequest("You are entering Invalid data... its not in our Database!!!");
                    }*/
                    if (updateDb(stock, stockId))
                        return Ok("Updated Successfully");
                    else
                        return BadRequest("This ID not found!!");

                }
                else
                {
                    return BadRequest("You are not adding valid data!!!");
                }
            }
            catch (Exception e)
            {

                return InternalServerError(e);
            }
        }
    }
}
