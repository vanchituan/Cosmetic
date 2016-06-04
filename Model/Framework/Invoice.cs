namespace DataLayer.Framework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Invoice")]
    public partial class Invoice
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int InvoiceId { get; set; }
        public DateTime? CreatedDate { get; set; }

        public DateTime? ShippedDate { get; set; }

        public int? CustomerId { get; set; }

        [StringLength(250)]
        public string ShipName { get; set; }

        [StringLength(250)]
        public string ShipMobile { get; set; }

        [StringLength(250)]
        public string ShipAddress { get; set; }

        [StringLength(250)]
        public string ShipEmail { get; set; }

        public decimal? Total { get; set; }

        public decimal? Profit { get; set; }
        public int PromotionId { get; set; }
        public int OrderId { get; set; }

        public int? ProvinceId { get; set; }

        public int? DistrictId { get; set; }

        public int? PrecinctId { get; set; }
    }
}
