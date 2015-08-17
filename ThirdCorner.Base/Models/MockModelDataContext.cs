using System;
using System.Data.Linq;
using System.Linq;


namespace ThirdCorner.Base.Models
{
    public partial class MockContext
    {
        public static readonly DataLoadOptions DataContextLoadOptions =
            (new Func<DataLoadOptions>(() =>
                                           {
                                               var loadOptions = new DataLoadOptions();
                                               //loadOptions.LoadWith<Parent>(fa => fa.Child);
                                               return loadOptions;
                                           }))();

        /// <summary>
        /// Get a datacontext with the load options
        /// </summary>
        /// <returns>a datacontext</returns>
        public static MockContext GetContext()
        {
            return new MockContext { LoadOptions = DataContextLoadOptions };
        }

    }
}