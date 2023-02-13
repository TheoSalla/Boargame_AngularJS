using BoardGamePickerWithAngularJS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace BoardGamePickerWithAngularJS.Services
{
    public class GetUserCollection : IGetUserCollection
    {
        private List<Game> games = new List<Game>();
        private List<int> attributeList = new List<int>();
        private Game game = new Game();
        private static int count = 0;
        private XmlReaderSettings settings = new XmlReaderSettings();
        private XmlReader readers(string userName)
        {
            string collection = "https://www.boardgamegeek.com/xmlapi/collection/" + userName + "?own=1";
            XmlReader reader = XmlReader.Create(collection, settings);
            return reader;
        }

        public async Task<List<Game>> GetCollection(string userName)
        {
        
            try
            {
                settings.Async = true;
                var getCollection = readers(userName);

                while (await getCollection.ReadAsync())
                {
                    if (getCollection.Name == "message")
                    {
                        getCollection = readers(userName);

                        continue;
                    }

                    if (getCollection.Name == "error")
                    {
                        games = null;
                        return games;
                    }

                    if (getCollection.Name == "name" || getCollection.Name == "image" || getCollection.Name == "stats" || getCollection.Name == "item")
                    {

                        if (getCollection.Name == "name")
                        {
                            string g = getCollection.ReadInnerXml();
                            game.GameName = g;
                            continue;
                        }
                        else if (getCollection.Name == "image")
                        {
                            string img = getCollection.ReadInnerXml();
                            game.Image = img;
                            continue;
                        }
                        else if (getCollection.Name == "stats")
                        {
                            if (getCollection.HasAttributes)
                            {
                                try
                                {
                                    attributeList.Add(int.Parse(getCollection.GetAttribute("minplayers")));
                                    attributeList.Add(int.Parse(getCollection.GetAttribute("maxplayers")));
                                    attributeList.Add(int.Parse(getCollection.GetAttribute("playingtime")));
                                }
                                catch (Exception)
                                {
                                }
                            }
                        }
                        else if (getCollection.Name == "item")
                        {
                            if (getCollection.HasAttributes)
                            {

                            
                            try
                            {
                                attributeList.Add(int.Parse(getCollection.GetAttribute("objectid")));
                            }
                            catch (Exception)
                            {
                                throw;
                            }
                            }
                        }
                        if (game.GameName != null && game.Image != null && attributeList.Count != 0)
                        {
                            try
                            {
                                game.GameId = attributeList[0];
                                game.MinPlayer = attributeList[1];
                                game.MaxPlayer = attributeList[2];
                                game.PlayingTime = attributeList[3];
                            }
                            catch (Exception)
                            {

                            }
                            finally
                            {
                                game.UserName = userName;
                                games.Add(game);
                                game = new Game();
                                attributeList = new List<int>();
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                count++;
                if (count == 5)
                {
                    Console.WriteLine("didnt work to get collection");
                    return games;
                }
                Thread.Sleep(5000); 
                await GetCollection(userName);
            }
            count = 0;
            
            return games;
        }
    }
}
