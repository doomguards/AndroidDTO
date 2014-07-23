using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDMAPI.Domain
{
    public class BomModel
    {
        public BomModel()
        {
            Items = new List<BomItem>();
        }
        public string BOMType { get; set; }
        public string Purpose { get; set; }
        public string Version { get; set; }
        public decimal? MCost { get; set; }
        public DateTime? LastEditTime { get; set; }
        public DateTime? AddTime { get; set; }
        public List<BomItem> Items { get; private set; }
    }

    public class BomItem
    {
        public string MNo { get; set; }
        public string MName { get; set; }
        public string MU { get; set; }
        public decimal? Qty { get; set; }
        public string Brand { get; set; }
        public string Supplier { get; set; }
        public bool? HasBOM { get; set; }
        public decimal? Price { get; set; }


    }
}
