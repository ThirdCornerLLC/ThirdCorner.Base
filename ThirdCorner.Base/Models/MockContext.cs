using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThirdCorner.Base.Models
{
    public partial class MockContext : DataContext
    {
        private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();

        public MockContext(string fileOrServerOrConnection) : base(fileOrServerOrConnection)
        {
        }

        public MockContext(string fileOrServerOrConnection, MappingSource mapping)
            : base(fileOrServerOrConnection, mapping)
        {
        }

        public MockContext(IDbConnection connection) : base(connection)
        {
        }

        public MockContext(IDbConnection connection, MappingSource mapping) : base(connection, mapping)
        {
        }

        public MockContext() :
            base("connstring", mappingSource)
        {
        }

        /// <summary>
        /// Mock... normally would be Table&lt;User&gt;
        /// </summary>
        public IQueryable<User> Users
        {
            get
            {
                return new List<User>
                {
                    new User
                    {
                        UserId = 1,
                        FullName = "Sean Goodpasture",
                        IsAdmin = true,
                        UserName = "sean"
                    },
                    new User
                    {
                        UserId = 2,
                        FullName = "John Jones",
                        IsAdmin = false,
                        UserName = "john"
                    }
                }.AsQueryable();
            }
        }
    }
}
