namespace S2SOMSAPI.Model
{
    public class UserConfigListResponse
    {
        public int statuscode { get; set; }
        public string status { get; set; }
        public int TotalRecords { get; set; }
        public List<UserGridList> UserGridList { get; set; }

    }

    public class UserGridList
    {
        public int ID { get; set; }
        public string Customer {  get; set; }
        public string UserName {  get; set; }
        public string UserType {  get; set; }
        public string Active { get; set; }
    }

    public class S2SUserListResp
    {
        public int statuscode { get; set; }
        public string status { get; set; }

        public List<UserList> UserList { get; set; }

    }

    public class UserList
    {
        public long ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserType { get; set; }
        public string UserName { get; set; }
        public string EmailID { get; set; }
    }

    public class SaveUserConfiResp
    {
        public int statuscode { get; set; }
        public string status { get; set; }
    }
}
