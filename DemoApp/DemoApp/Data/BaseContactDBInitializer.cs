using System.Diagnostics;
using System.Net.NetworkInformation;
using Even.Data;
using Even.Models;

namespace Even.Data
{
    public class BaseContactDBInitializer
    {
        public static void Initialize(BaseContactDBContext mycontext)
        {

            // Look for any BC.
           // if (mycontext.BaseContacts.Any())
           // {
          //      return;   // DB has been seeded
          //  }

           // var baseContacts = new BaseContact[]
           // {
            //    new BaseContact{Email="devzzhr@outlook.fr",Creation=DateTime.Parse("2023-09-01 ").ToUniversalTime(),Modification=DateTime.Parse("2023-09-01").ToUniversalTime()},
            //  };

            //mycontext.BaseContacts.AddRange(baseContacts);
            //mycontext.SaveChanges();


           
        }

    }
}




 