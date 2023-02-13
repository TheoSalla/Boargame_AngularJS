using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using System.Xml;

namespace BoardGame.Services
{
    public class GetCollectionCount : ApiCall<string>
    {
        private string count = null;
        private XmlReaderSettings settings = new XmlReaderSettings();
        protected override XmlReader readers(string userName)
        {
            string collection = "https://www.boardgamegeek.com/xmlapi/collection/" + userName + "?own=1";
            XmlReader reader = XmlReader.Create(collection,settings);
            return reader;
        }
        public override async Task<string> GetCollection(string userName)
        {
            try
            {
                settings.Async = true;
                var collection = readers(userName);
                while (await collection.ReadAsync())
                {
                    if (collection.Name == "items")
                    {
                        if (collection.HasAttributes)
                        {
                            return count = collection.GetAttribute("totalitems");
                        }
                    }
                }
            }
            catch (Exception msg)
            {
                Console.WriteLine(msg.Message);
               
            }
            return null;
        }
       
    }
}
