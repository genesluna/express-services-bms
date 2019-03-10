using ExpressServices.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpressServices.Core.Services
{
    public partial class CaseTableService
    {
        public async Task<List<Product>> GetProductsByCaseIdAsync(string caseId)
        {
            var caseProducts = await CaseProductTable.GetSyncTable().Where(x => x.CaseId == caseId).ToListAsync();

            var products = new List<Product>();

            foreach (var item in caseProducts)
            {
                var product = new Product
                {
                    Id = item.ProductId,
                    Name = item.Name,
                    Quantity = item.Quantity,
                    Price = item.Price
                };
                products.Add(product);
            }

            return products;
        }

        public async Task<CaseProduct> GetCaseProductByCaseIdAndProductIdAsync(string caseId, string productId)
        {
            return (await CaseProductTable.GetSyncTable().Where(x => x.CaseId == caseId && x.ProductId == productId).ToListAsync()).FirstOrDefault();
        }
    }
}