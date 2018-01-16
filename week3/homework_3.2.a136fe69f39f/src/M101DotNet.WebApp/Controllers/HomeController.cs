using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MongoDB.Driver;
using M101DotNet.WebApp.Models;
using M101DotNet.WebApp.Models.Home;
using MongoDB.Bson;
using System.Linq.Expressions;

namespace M101DotNet.WebApp.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            var blogContext = new BlogContext();
            
            var recentPosts =
                await blogContext.Posts.Find(x => true).SortByDescending(p => p.CreatedAtUtc).Limit(10).ToListAsync();

            var model = new IndexModel
            {
                RecentPosts = recentPosts
            };

            return View(model);
        }

        [HttpGet]
        public ActionResult NewPost()
        {
            return View(new NewPostModel());
        }

        [HttpPost]
        public async Task<ActionResult> NewPost(NewPostModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var content = model.Content?.Trim();
            if (string.IsNullOrEmpty(content))
            {
                ModelState.AddModelError("Content", "Post should not be empty");
                return View(model);
            }

            var blogContext = new BlogContext();

            var p = new Post()
            {
                Author = User.Identity.Name,
                Content = content,
                Title = model.Title,
                Tags = model.Tags.Split(new[] {' ', ','}, StringSplitOptions.RemoveEmptyEntries),
                CreatedAtUtc = DateTime.UtcNow
            };

            await blogContext.Posts.InsertOneAsync(p);

            return RedirectToAction("Post", new { id = p.PostId.ToString() });
        }

        [HttpGet]
        public async Task<ActionResult> Post (string id)
        {
            var blogContext = new BlogContext();            

            var objId = ObjectId.Parse(id);
            var post = await blogContext.Posts.Find(p => p.PostId == objId).FirstOrDefaultAsync();

            if (post == null)            
                return RedirectToAction("Index");            

            var model = new PostModel
            {
                Post = post
            };

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Posts(string tag = null)
        {
            var blogContext = new BlogContext();

            tag = tag?.Trim();

            // XXX WORK HERE
            // Find all the posts with the given tag if it exists.
            // Otherwise, return all the posts.
            // Each of these results should be in descending order.

            return View(await blogContext.Posts.Find(p => string.IsNullOrEmpty(tag) || p.Tags.Contains(tag)).SortByDescending(p => p.CreatedAtUtc).ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult> NewComment(NewCommentModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Post", new { id = model.PostId });
            }

            var blogContext = new BlogContext();
            // XXX WORK HERE
            // add a comment to the post identified by model.PostId.
            // you can get the author from "this.User.Identity.Name"

            var bld = Builders<Post>.Filter;
            var comment = new Comment() {Author = User.Identity.Name, CreatedAtUtc = DateTime.UtcNow, Content = model.Content};

            await
                blogContext.Posts.UpdateOneAsync(bld.Eq(p => p.PostId, ObjectId.Parse(model.PostId)),
                    Builders<Post>.Update.Push(p => p.Comments, comment));

            return RedirectToAction("Post", new { id = model.PostId });
        }
    }
}