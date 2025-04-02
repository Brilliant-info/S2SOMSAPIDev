namespace S2SOMSAPI.Model
{
    public class S2SOrderListRespn
    {
        public int statuscode { get; set; }
        public string status { get; set; }
        public List<S2SOrders> S2SOrders { get; set; }
    }

    public class S2SOrders
    {
        public string S2SOrderNo { get; set; }
        public string WincashOrderNo { get; set; }
        public string Ordertype { get; set; }
        public string Sourcestore { get; set; }
        public string DestinationStore { get; set; }
        public string Performedby { get; set; }
        public string Receivedby { get; set; }
    }

    public class S2SOrderHistoryResp
    {
        public int statuscode { get; set; }
        public string status { get; set; }
        public List<S2SOrderHistories> S2SOrderHistories { get; set; }
    }

    public class S2SOrderHistories
    {
        public string S2SOrderNo { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; }
        public string Updatedby { get; set; }
    }

    public class S2SOrderViewResp
    {
        public int statuscode { get; set; }
        public string desc { get; set; }
        public string WincashOrderNo { get; set; }
        public string S2SOrderNo { get; set; }
        public string Status { get; set; }
        public string Ordertype { get; set; }
        public string Sourcestore { get; set; }
        public string DestinationStore { get; set; }
        public string Performedby { get; set; }
        public string Receivedby { get; set; }
        public DateTime CreationDate { get; set; }
        public List<Skulist> Skulist { get; set; }
    }

    public class Skulist
    {
        public string SKU { get; set; }
        //public string Skuname { get; set; }
        public string serialnumber { get; set; }
        public Decimal Quantity { get; set; }

    }
    public class S2SOrderDriverlistResp
    {
        public int statuscode { get; set; }
        public string status { get; set; }
        public List<S2SOrderDriverlist> S2SOrderDriverlist { get; set; }
    }

    public class S2SOrderDriverlist
    {
        public string DriverName { get; set; }
        public string ContactNo { get; set; }
        public string EmailID { get; set; }


    }

    public class SaveReqResponce
    {
        public int statuscode { get; set; }
        public string status { get; set; }
    }
}

