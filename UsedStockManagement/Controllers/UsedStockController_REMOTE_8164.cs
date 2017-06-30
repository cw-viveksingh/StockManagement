using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UsedStockManagement.ElasticSerchClient;
using UsedStockManagement.Models;

namespace UsedStockManagement.Controllers
{
    public class UsedStockController : Controller
    {
        private static ElasticClient elasticClient = ElasticClientInstance.GetInstance();
        private QueryContainer queryContainer = new QueryContainer();
        private QueryDescriptor<UsedCarStock> query = new QueryDescriptor<UsedCarStock>();
        
        // GET: UsedStock
        public ActionResult Index()
        {
            try
            {
                var searchResults = elasticClient.Search<UsedCarStock>(s => s
                                    .Index("usedstockes")
                                    .Type("usedcarstock")
                                    .Query(q => q
                                    .Term(p => p.IsDeleted, 0))
                                );
                return View("~/Views/Home/UsedStockSearch.cshtml", searchResults.Documents);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult FilterSearch(string city = null, int minBudget = 0, int maxBudget = int.MaxValue)
        {
            queryContainer = query.Term(p => p.IsDeleted, 0);
            
            if (city != null)
                queryContainer &= query.Term(p => p.City, city); 
           
            if (minBudget != 0 || maxBudget != int.MaxValue)
                queryContainer &= query.Range(r => r.OnField(fi => fi.Price).LowerOrEquals(maxBudget).GreaterOrEquals(minBudget));
         
            var searchResults = elasticClient.Search<UsedCarStock>(s => s
                .Index("usedstockes")
                .Type("usedcarstock")
                .Query(queryContainer)
            );
            return View("~/Views/Home/UsedStockSearch.cshtml", searchResults.Documents);
        }
    }
}