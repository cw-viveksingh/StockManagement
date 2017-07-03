using StockImageProducer.Models;
using StockImageProducer.RabbitMqPublish;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace StockImageProducer.Controllers
{
    public class StockController : ApiController
    {
        [HttpPost]
        [Route("api/Stock/{stockId}/Image")]
        public IHttpActionResult post(int stockId, [FromBody]Url url)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                Send send = new Send();
                send.start(stockId.ToString(), url.urlName);
                return Ok("wait your image is upload process is in process");

            }
            catch (Exception)
            {
                //throw;
                return BadRequest("Error!!!");

            }
        }
    }
}
