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
         /*IHttpActionResult validation(Stocks stockObj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!(stockObj.price >= 10000 && stockObj.price <= 30000000))
            {
                return BadRequest("Please Enter a valid price b/w 10000 and 3 Crore");
            }
            if (!(stockObj.year >= 2010 && stockObj.year <= DateTime.Today.Year))
            {
                return BadRequest("Please Enter a valid Date b/w 2010 and " + DateTime.Today.Year);
            }
            if (!(stockObj.kilometer >= 100 && stockObj.kilometer <= 300000))
            {
                return BadRequest("Please Enter a valid Kilometers b/w 100 and 3 Lakh");
            }
            if (stockObj.fuelEconomy != -1)
            {
                if (!(stockObj.fuelEconomy >= 1 && stockObj.fuelEconomy <= 50))
                {
                    return BadRequest("Please Enter a valid Fuel Economy b/w 1 and 50 km");
                }
            }
            return Ok();
        }
        IHttpActionResult dbValidation(Stocks stockObj)
         {

            IDbConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["myconn"].ConnectionString);
                var par = new DynamicParameters();
                par.Add("color_id", stockObj.color);
                par.Add("model_id", stockObj.model);
                par.Add("city_id", stockObj.city);
                par.Add("make_id", stockObj.make);
                par.Add("fuel_id", stockObj.fuelType);
                par.Add("version_id", stockObj.version);

                Color colorObj = new Color();
                Model modelObj = new Model();
                Fuel fuelObj = new Fuel();
                City cityObj = new City();
                Make makeObj = new Make();
                Versions versionObj = new Versions();
                using (var multi = connection.QueryMultiple("verify", par,commandType: CommandType.StoredProcedure))
                {

                    colorObj = multi.Read<Color>().First();
                    modelObj = multi.Read<Model>().First();
                    fuelObj = multi.Read<Fuel>().First();
                    cityObj = multi.Read<City>().First();
                    makeObj = multi.Read<Make>().First();
                    versionObj = multi.Read<Versions>().First();
                   // return Ok(colorObj.color + " " + modelObj.model + " " + fuelObj.fuel + " " + cityObj.city + " " + makeObj.make + " " + versionObj.version);
                }
                int check = colorObj.color * modelObj.model * fuelObj.fuel * cityObj.city * makeObj.make * versionObj.version;
            if(check !=1 )
                return BadRequest("Wrong Parameters");
            
                return Ok();
         }*/
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
                //var value = validation(stockObj);
                //var cal = dbValidation(stockObj);
                
                var par = new DynamicParameters();
                par.Add("color_id", stockObj.color);
                par.Add("model_id", stockObj.model);
                par.Add("city_id", stockObj.city);
                par.Add("make_id", stockObj.make);
                par.Add("fuel_id", stockObj.fuelType);
                par.Add("version_id", stockObj.version);

                Color colorObj = new Color();
                Model modelObj = new Model();
                Fuel fuelObj = new Fuel();
                City cityObj = new City();
                Make makeObj = new Make();
                Versions versionObj = new Versions();
                IDbConnection connectionOne = new MySqlConnection(ConfigurationManager.ConnectionStrings["myconn"].ConnectionString);
                using (var multi = connectionOne.QueryMultiple("verify", par, commandType: CommandType.StoredProcedure))
                {

                    colorObj = multi.Read<Color>().First();
                    modelObj = multi.Read<Model>().First();
                    fuelObj = multi.Read<Fuel>().First();
                    cityObj = multi.Read<City>().First();
                    makeObj = multi.Read<Make>().First();
                    versionObj = multi.Read<Versions>().First();
                    // return Ok(colorObj.color + " " + modelObj.model + " " + fuelObj.fuel + " " + cityObj.city + " " + makeObj.make + " " + versionObj.version);
                }
                int check = colorObj.color * modelObj.model * fuelObj.fuel * cityObj.city * makeObj.make * versionObj.version;
                if (check != 1)
                    return BadRequest("Wrong Parameters");
                IDbConnection connectionOne = new MySqlConnection(ConfigurationManager.ConnectionStrings["myconn"].ConnectionString);
                using (var multi = connectionOne.QueryMultiple())
                {

                }
                return Ok("Your stock is Added to the Database");
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }
    }
}
