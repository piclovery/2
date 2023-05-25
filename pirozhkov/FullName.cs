using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pirozhkov

{
    public partial class User
    {
        public string fullname => $"{UserSurname} {UserName} {UserPatronymic}";
    }
}
