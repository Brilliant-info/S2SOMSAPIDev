﻿using System.ComponentModel.DataAnnotations;

namespace S2SOMSAPI.Model
{
    public class S2SOrderListReq
    {
        public int CurrentPage {  get; set; }
        public int RecordLimit { get; set; }
        public long UserId { get; set; }
        public string Search  { get; set; }
        public string Filter { get; set; }
    }

    public class S2SOrderHistoryReq
    {
        [Required(ErrorMessage = "S2SOrderNO is required.")]
        public string S2SOrderNO { get; set; }
    }

    public class S2SOrderViewReq
    {
        [Required(ErrorMessage = "OmsorderNo is required.")]
        public string OmsorderNo { get; set; }
       // public int OMSOrderNo { get; set; }
    }
    public class DriverlistReq
    {
        public int CompanyID { get; set; }  
        public int UserID { get; set; }
    }

    public class AssignDriverReq
    {
        [Required(ErrorMessage = "ObjectName is required.")]
        public string ObjectName { get; set; }

        [Required(ErrorMessage = "ReferenceID is required.")]
        public long ReferenceID { get; set; }

        [Required(ErrorMessage = "DriverId is required.")]
        public long DriverId { get; set; }
        public long AssignBy {  get; set; }
        public string VehicleDetail { get; set; }
    }

    public class RemoveDriverAssignReq
    {
        [Required(ErrorMessage = "ObjectName is required.")]
        public string ObjectName { get; set; }

        [Required(ErrorMessage = "ReferenceID is required.")]
        public long ReferenceID { get; set; }

        [Required(ErrorMessage = "DriverId is required.")]
        public long DriverId { get; set; }
    }
    public class AssignDriverlistReq
    {
        public int ReferenceID { get; set; }
        public string objectName { get; set; }
    }
    public class S2SdocumentlistReq
    {
        public int ReferenceID { get; set; }
        public int CompanyID { get; set; }
        public string ObjectName { get; set; }
    }

    public class CancelOrderReq
    {
        public long OrderID { get; set; }
        public long UserID { get; set; }    
    }

    public class S2SOrderReportReq
    {
        public string OrderNo { get; set; }
        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }
        public string Status { get; set; }
    }

}
