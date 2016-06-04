namespace DataLayer.Framework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Promotion")]
    public partial class Promotion
    {
        public int PromotionId { get; set; }

        [StringLength(250)]
        public string PromotionName { get; set; }

        public decimal? Value { get; set; }

        public int? Discount { get; set; }
    }
}
