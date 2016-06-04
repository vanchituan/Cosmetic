using DataLayer.Framework;
using DataLayer.ViewModel.Admin.Invoice;
using DataLayer.ViewModel.Admin.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DataAccessObj
{
    public class InvoiceDetailDao
    {
        OnlineShopDBContext db;
        public InvoiceDetailDao()
        {
            db = new OnlineShopDBContext();
        }

        public void Insert(int invoiceId, int productId, int quantity)
        {
            InvoiceDetail invoiceDetail = new InvoiceDetail
            {
                InvoiceId = invoiceId,
                ProductId = productId,
                Quantity = quantity
            };
            db.InvoiceDetails.Add(invoiceDetail);
            db.SaveChanges();
        }

    }
}
