using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace BoardGame.Services
{
    public abstract class ApiCall<T>
    {
        public abstract Task<T> GetCollection(string userName); 
        protected abstract XmlReader readers(string userName);
    }
}
