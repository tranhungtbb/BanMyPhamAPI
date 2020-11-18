using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Library.DataBase;

namespace WebBanMyPham.Author
{
    public class CustomerRepository: IDisposable
    {
        MyDBDataContext db = new MyDBDataContext();
        //This method is used to check and validate the user credentials
        public Customer ValidateUser(string username, string password)
        {
            var res = db.Customers.FirstOrDefault(x =>
            x.UserName.Equals(username)
            && x.Password == password);
            return res;
        }
        public void Dispose()
        {
            db.Dispose();
        }
    }
}