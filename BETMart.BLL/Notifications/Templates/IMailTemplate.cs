using System.Collections.Generic;
using System.IO;
using BETMart.BLL.Models;

namespace BETMart.BLL.Notifications
{
    public interface IMailTemplate
    {
        string ToAddress { get; set; }
        string DisplayName { get; set; }
        string HeaderText { get; set; }
        string Subject { get; set; }
        string TemplateName { get; }
        string Content { get; set; }
        string AttachmentFileName { get; set; }
        Stream AttachmentStream { get; set; }
        string DocumentName { get; set; }
        List<OrderDetail> OrderDetails { get; set; }
        decimal TotalCost { get; set; }
    }
}