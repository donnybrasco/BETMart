using System.Collections.Generic;
using System.IO;
using BETMart.BLL.Models;

namespace BETMart.BLL.Notifications.Templates
{
    public class InvoiceTemplate
        : IMailTemplate
    {
        public string TemplateName => "InvoiceTemplate";
        public string ToAddress { get; set; }
        public string DisplayName { get; set; }
        public string HeaderText { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public string AttachmentFileName { get; set; }
        public Stream AttachmentStream { get; set; }
        public string DocumentName { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
        public decimal TotalCost { get; set; }
    }
}
