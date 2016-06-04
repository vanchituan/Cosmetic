namespace DataLayer.Framework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Order")]
    public partial class Order
    {
        public int Id { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? OrderDate { get; set; }

        public int? CustomerId { get; set; }

        [StringLength(50)]
        public string ShipName { get; set; }

        [StringLength(50)]
        public string ShipMobile { get; set; }

        [StringLength(50)]
        public string ShipAddress { get; set; }

        [StringLength(50)]
        public string ShipEmail { get; set; }

        public decimal? Total { get; set; }

        public int PromotionId { get; set; }

        public int Status { get; set; }

        public int? ProvinceId { get; set; }

        public int? DistrictId { get; set; }

        public int? PrecinctId { get; set; }
    }
}
