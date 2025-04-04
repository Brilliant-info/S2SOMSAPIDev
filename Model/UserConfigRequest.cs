using System.ComponentModel.DataAnnotations;
using System.Numerics;
namespace S2SOMSAPI.Model
{
    public class S2SUserConfigList
    {
        public int CurrentPage { get; set; }
        public int RecordLimit { get; set; }
        public long CompanyID {  get; set; }
        public long UserID {  get; set; }
        public string Search { get; set; }
        public string Filter { get; set; }
    }

    public class UserListReq
    {
        public long CompanyID { get; set; }  
        public string SearchFName { get; set; }
    }

    public class InsertUserConfigReq
    {
        public string ObjectName { get; set; }
        [Required(ErrorMessage = "UserID is Required")]
        public long UserID { get; set; }
        public long CreatedBy { get; set; }
        public long CompanyID { get; set; }
    }

    public class RemoveConfigReq
    { 
        public string ObjectName { get; set; }
        public int ID { get; set; }
    }
}
