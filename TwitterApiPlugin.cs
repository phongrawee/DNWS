using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
namespace DNWS
{
    class TwitterApiPlugin : TwitterPlugin
    {
        public List<User> GetUser()
        {
            using (var context = new TweetContext())
            {
                try
                {
                    List<User> users = context.Users.ToList();
                    return users;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public List<Following> GetFollow(string name)
        {
            using (var context = new TweetContext())
            {
                try
                {
                    List<User> followings = context.Users.Where(b => b.Name.Equals(name)).Include(b => b.Following).ToList();
                    return followings[0].Following;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public override HTTPResponse GetResponse(HTTPRequest request)
        {
            HTTPResponse response = new HTTPResponse(200);
          
                string u = request.getRequestByKey("user");
                string p = request.getRequestByKey("password");
                string f = request.getRequestByKey("following");
                string text = request.getRequestByKey("text");
                string timeline = request.getRequestByKey("timeline");
                string []path = request.Filename.Split("?");
            if (path[0] == "user")
            {
                if (request.Method == "GET")
                {
                    string temp = JsonConvert.SerializeObject(GetUser());
                    response.body = Encoding.UTF8.GetBytes(temp);
                }
                if (request.Method == "POST")
                {
                    Twitter.AddUser(u, p);
                    response.body = Encoding.UTF8.GetBytes("ADD Success");
                }
                if (request.Method == "DELETE")
                {
                    Twitter.DeleteUser(u);
                    response.body = Encoding.UTF8.GetBytes("Delete Success");
                }
            } if (path[0] == "follow")
            {
                Twitter twitter = new Twitter(u);
                if (request.Method == "GET")
                {
                    string temp = JsonConvert.SerializeObject(GetFollow(u));
                    response.body = Encoding.UTF8.GetBytes(temp);
                }
                if (request.Method == "POST")
                {
                    Twitter follow = new Twitter(u);
                    follow.AddFollowing(f);
                    response.body = Encoding.UTF8.GetBytes("Success Following");  
                }

            }
            if (path[0] == "Tweet")
            {
                Twitter twitter = new Twitter(u);
                if (request.Method == "GET")
                {
                    if (timeline == "user")
                    {
                        string temp = JsonConvert.SerializeObject(twitter.GetUserTimeline());
                        response.body = Encoding.UTF8.GetBytes(temp);
                    }
                     if(timeline == "follow")
                    {
                        string temp = JsonConvert.SerializeObject(twitter.GetFollowingTimeline());
                        response.body = Encoding.UTF8.GetBytes(temp);
                    }
                }
                if (request.Method == "POST")
                {
                    twitter.PostTweet(text);
                    response.body = Encoding.UTF8.GetBytes("Post Success");
                }  
            }
            return response;
        }
       
    }
}
