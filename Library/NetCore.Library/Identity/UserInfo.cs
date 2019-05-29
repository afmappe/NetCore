using System;

namespace NetCore.Library.Identity
{
    public class UserInfo
    {
        public string UserName { get; set; }

        public string FullName { get; set; }

        public DateTime CreationDate { get; set; }

        public int Id { get; set; }
    }
}