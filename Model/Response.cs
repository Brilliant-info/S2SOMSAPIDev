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
        public string Id { get; set; }
        public string S2SOrderNo { get; set; }
        public string WincashOrderNo { get; set; }
        public string Ordertype { get; set; }
        public string Sourcestore { get; set; }
        public string DestinationStore { get; set; }
        public string Performedby { get; set; }
        public string Receivedby { get; set; }
        public string Status { get; set; }
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
       // public string Ordertype { get; set; }
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
        public string driverID { get; set; }
        public string DriverName { get; set; }
        public string ContactNo { get; set; }
        public string EmailID { get; set; }


    }

    public class SaveReqResponce
    {
        public int statuscode { get; set; }
        public string status { get; set; }
    }

    public class S2SAssignDriverlistResp
    {
        public int statuscode { get; set; }
        public string status { get; set; }
        public List<S2SDriverlist> S2SDriverlist { get; set; }
    }
    
    public class S2SDriverlist
    {
        public string Id { get; set; }
        public string AssignDate { get; set; }
        public string DriverId { get; set; }
        public string DriverName { get; set; }
        public string TrackDetails { get; set; }
        public string ReferenceId { get; set; }
    }
    public class S2SOrderDocumentListRespn
    {
        public int statuscode { get; set; }
        public string status { get; set; }
        public List<documentlist> documentlist { get; set; }
    }

    public class documentlist
    {
        public string Id { get; set; }
        public string DocumentName { get; set; }
        public string Description { get; set; }
        public string Ordertype { get; set; }
        public string DocumentType { get; set; }
        public string FileType { get; set; }
        public string AttachedFile { get; set; }
        public string ReferenceID { get; set; }
    }
}

