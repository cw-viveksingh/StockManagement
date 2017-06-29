using Dapper;
using MySql.Data.MySqlClient;
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
                par.Add("price",stockObj.price);
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
                using (var multi = connectionTwo.QueryMultiple("table_insert", par, commandType: CommandType.StoredProcedure))
                {
                    temp = multi.Read<Db>().First();
                }
                return Ok("http://localhost:52227/api/Stock/stocks/" + temp.Id);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
        //[HttpDelete]
        [Route("api/Stock/stocks/{id}")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                IDbConnection connectionTwo = new MySqlConnection(ConfigurationManager.ConnectionStrings["myconn"].ConnectionString);
                Check obj = new Check();
                var par = new DynamicParameters();
                par.Add("par_id", id);
                using (var multi = connectionTwo.QueryMultiple("check_exist", par, commandType: CommandType.StoredProcedure))
                {
                    obj = multi.Read<Check>().First();
                }
                if (obj.exist == 0)
                    return BadRequest("The Stock Id You Entered Is Wrong!!!");
                else
                    return Ok("Data Deleted Successfully");
            }
            catch (Exception e)
            {

                return InternalServerError(e);
            }
        }
    }
}
