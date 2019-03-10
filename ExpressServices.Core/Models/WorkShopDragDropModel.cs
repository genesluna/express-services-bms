using System.Collections.Generic;

namespace ExpressServices.Core.Models
{
    public class WorkShopDragDropModel
    {
        public string Type { get; set; }

        public List<WorkShopItemsModel> Items { get; set; } = new List<WorkShopItemsModel>();
    }
}