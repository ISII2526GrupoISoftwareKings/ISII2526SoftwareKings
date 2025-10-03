
﻿namespace AppForSEII2526.API.Models
{
﻿using System.Numerics;

    namespace AppForSEII2526.API.Models
    {
        [PrimaryKey(nameof(ItemId), nameof(RestockId))]
        public class RestockItem
        {
            public int ItemId { get; set; }
            public Item Item { get; set; }

            public int RestockId { get; set; }
            public Restock Restock { get; set; }

            public int Quantity { get; set; }
            public decimal? RestockPrice { get; set; }


        }
    }
}
