using ExpressServices.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpressServices.Core.Services
{
    public partial class CaseTableService
    {
        public async Task<List<Service>> GetServicesByCaseIdAsync(string caseId)
        {
            var caseServices = await CaseServiceTable.GetSyncTable().Where(x => x.CaseId == caseId).ToListAsync();

            var services = new List<Service>();

            foreach (var item in caseServices)
            {
                var service = new Service
                {
                    Id = item.ServiceId,
                    Name = item.Name,
                    SaleQuantity = item.Quantity,
                    Price = item.Price
                };
                services.Add(service);
            }

            return services;
        }

        public async Task<CaseService> GetCaseServiceByCaseIdAndServiceIdAsync(string caseId, string serviceId)
        {
            return (await CaseServiceTable.GetSyncTable().Where(x => x.CaseId == caseId && x.ServiceId == serviceId).ToListAsync()).FirstOrDefault();
        }
    }
}