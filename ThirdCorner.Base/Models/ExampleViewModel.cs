using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThirdCorner.Base.Models
{
    public class ExampleViewModel : PagingSortingViewModel
    {
        public IEnumerable<User> Users;
    }
}
