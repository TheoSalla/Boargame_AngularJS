using BoardGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace BoardGame.Services
{
    public class GetGameInfo : ApiCall<GameInfo>, IGetGameInfo
    {
        private GameInfo game = new GameInfo();
        private XmlReaderSettings settings = new XmlReaderSettings();
        protected override XmlReader readers(string id)
        {
            string collection = "https://www.boardgamegeek.com/xmlapi/boardgame/" + id;
            XmlReader reader = XmlReader.Create(collection, settings);
            return reader;
        }
        public override async Task<GameInfo> GetCollection(string objectId)
        {
            try
            {        
                settings.Async = true;
                var getGame = readers(objectId);
                while (await getGame.ReadAsync())
                {
                    if (getGame.Name == "description")
                    {
                        game.Description = getGame.ReadInnerXml();
                        continue;
                    }
                    if (getGame.Name == "name")
                    {
                        game.GameName = getGame.ReadInnerXml();
                    }
                    if (getGame.Name == "yearpublished")
                    {
                        game.Year = getGame.ReadInnerXml();
                    }
                    if (getGame.Name == "image")
                    {
                        game.Image = getGame.ReadInnerXml();
                    }
                    if (getGame.Name == "boardgamedesigner")
                    {
                        game.Designer = getGame.ReadInnerXml();
                    }
                    if (getGame.Name == "boardgameartist")
                    {
                        game.Artist = getGame.ReadInnerXml();
                    }
                    if (getGame.Name == "minplayers")
                    {
                        game.MinPlayer = int.Parse(getGame.ReadInnerXml());
                    }
                    if (getGame.Name == "maxplayers")
                    {
                        game.MaxPlayer = int.Parse(getGame.ReadInnerXml());
                    }
                    if (getGame.Name == "maxplaytime")
                    {
                        game.PlayingTime = int.Parse(getGame.ReadInnerXml());
                    }
                    if (getGame.Name == "boardgamecategory")
                    {
                        game.Category = getGame.ReadInnerXml();
                    }
                }
            }
            catch (Exception msg)
            {
                Console.WriteLine(msg.Message);
            }
            return game;
        }

       
    }
}

