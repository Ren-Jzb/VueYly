using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    #region
    public class BaseServiceEf<T> where T : IDisposable, new()
    {
        public T _context;

        public BaseServiceEf()
        {
            if (_context == null)
            {
                _context = new T();
            }
        }
        public void Dispose()
        {
            if (_context != null)
                _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
    #endregion
}
