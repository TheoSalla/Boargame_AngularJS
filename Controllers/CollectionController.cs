using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using BoardGame.Services;
using BoardGamePickerWithAngularJS.Data;
using BoardGamePickerWithAngularJS.Models;
using BoardGamePickerWithAngularJS.Services;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BoardGamePickerWithAngularJS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CollectionController : ControllerBase
    {
 

        private readonly IGetUserCollection _userCollection;
        private readonly ApplicationDbContext _context;
        private GetCollectionCount _collectionCount = new GetCollectionCount();
        private User _user { get; set; } = new User();
        public CollectionController(IGetUserCollection userCollection, ApplicationDbContext context)
        {
            _userCollection = userCollection;
            _context = context;
        }



        // api/collection/{username}
        [HttpGet("{userName}")]
        public async Task<IActionResult> Get(string userName)
        {

            bool connected = CheckForInternetConnection();
            Console.WriteLine("Connected: " + connected);

            if (!connected)
            {
                var checkIfUserInDb = await _context.Users.FirstOrDefaultAsync(x => x.UserName == userName.ToLower());
                var userCollection = await _context.Games.Where(x => x.UserId == checkIfUserInDb.Id).ToListAsync();
                return Ok(userCollection);
            }

            bool toSaveOrNotToSave = true;
            var collectionCount = await _collectionCount.GetCollection(userName);

            try
            {
                var checkIfUserInDb = await _context.Users.FirstOrDefaultAsync(x => x.UserName == userName.ToLower());
                //If user is in database
                if (checkIfUserInDb != null)
                {                    
                   var userCollection = await _context.Games.Where(x => x.UserId == checkIfUserInDb.Id).ToListAsync();
                    if (userCollection.Count == int.Parse(collectionCount))
                    {
                        Console.WriteLine("Getting collection from database");
                        return Ok(userCollection);
                    }
                    else
                    {
                        toSaveOrNotToSave = false;
                    }
                    
                }
            }
            catch (Exception msg)
            {
                Console.WriteLine(msg.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            // User is not in database
            // Get collection from bbgs api with the GetCollection Class
            var collection = await _userCollection.GetCollection(userName.ToLower());
            // If the collection from the api is null
            if (collection == null || collection.Count == 0)
            {
                return NotFound();
            }

            // If saving process havent begun
            if (toSaveOrNotToSave)
            {
                // Starting background job to save collection to database
                BackgroundJob.Enqueue(() => SaveToDb(userName, collection));
            }
          

            // Returning the user collection in json format
            Console.WriteLine("Getting collection from xml service");
            return Ok(collection);
        }
        [HttpGet]
        //api/collection/
        public async Task<IActionResult> Get()
        {
            try
            {
                //Getting users from database
                var users = await _context.Users.ToListAsync();
                return Ok(users);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        // Function that saves collection to database
        public async Task SaveToDb(string userName, List<Game> collection)
        {

            try
            {
                //Linking the user model with the user name entered
                _user.UserName = userName.ToLower();
                // Adding and saving the user in the database 
                await _context.AddAsync(_user);
                await _context.SaveChangesAsync();
                // Adding and saving the games to the database 
                foreach (var game in collection)
                {
                    game.UserId = _user.Id;
                    await _context.Games.AddAsync(game);
                    await _context.SaveChangesAsync();
                };
            }
            catch (Exception msg)
            {

                Console.WriteLine(msg.InnerException);
            }

        }

        // Function to check for internet connection
        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://google.com/"))
                    return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
