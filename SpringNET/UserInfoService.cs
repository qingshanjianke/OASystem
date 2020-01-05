using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpringNET
{
    class UserInfoService : IUserInfoService
    {
        public String Name { get; set; }
        public Person Person { get; set; }
        public void showMsg()
        {
            Console.WriteLine("你好:"+Name+":"+ Person.Age);
        }
    }
}
