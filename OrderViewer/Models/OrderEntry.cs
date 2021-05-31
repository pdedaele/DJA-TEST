using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace OrderViewer.Models
{
    public class OrderEntry 
    {
        public bool Status { get; set; }
        public string FileName { get; set; }
        public string XMLContent { get; set; }
        public DateTime FileDate { get; set; }
       
    }
}