using AlfaAccounting.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;


namespace AlfaAccounting.Controllers
{

    /// <summary>
    /// Name:Mie Tanaka
    /// Name:26/05/2017
    /// Description: allows users to view blog and comment attached to the blog and add comment

    public class BlogViewModelsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// returns  a BlogViewModelIndex view with a selected blog and list of the blog comments.
        /// </summary>
        /// <param name="blogid"></param>
        /// <param name="returnUrl"></param>
        /// <returns>BlogViewModelIndex View wiht model data</returns>
        /// <includesource>yes</includesource>
        [AllowAnonymous]
        public ActionResult BlogViewModelIndex(int blogid, string returnUrl)
        { 
            //Recieve blog TempData from previous view
            Blog passedblog = TempData["tempBlog"] as Blog;
            // in case of entering this view by means of back button or back from addComment
            if (blogid.Equals(null)||blogid==0)
            {if (passedblog.BlogId != 0 || !passedblog.BlogId.Equals(null))
                {
                    blogid = passedblog.BlogId;
                }
                else return RedirectToAction("Index","Blogs");
            }
            
            //create a blog to find the selected blog.
            Blog blog = db.Blogs.Find(blogid);
            // create mymodel that get passed the view
            // and allocate the blog value in to my model
            BlogViewModel mymodel = new BlogViewModel()
            {
                BlogTitle = blog.BlogTitle,
                BlogDate = blog.BlogDate,
                BlogContent = blog.BlogContent,
                Category = blog.Category,
                ApplicationUser = blog.ApplicationUser,
                Comments = db.Comments.Where(c => c.BlogId == blogid).ToList()
            };
            //pass chosen blog temp data to next view
            TempData["tempBlog"] = blog;
            return View(mymodel);
        }

        /// <summary>
        /// Returns a blank BlogViewModels/AddComment view and pass input cmt to next view AddComment
        /// </summary>
        /// <returns>Returns blank comment with cmt data</returns>
        ///<includesource>yes</includesource>
        public ActionResult AddCommentCreate()
        {
            //Recieve blog TempData from previous view
            Blog passedblog = TempData["tempBlog"] as Blog;
            var cmt = new Comment();
            cmt.BlogId = passedblog.BlogId;
            cmt.Id = User.Identity.GetUserId();

            //pass chosen blog temp data to next blog
            TempData["tempBlog"] = passedblog;
            return View(cmt);
        }

        /// <summary>
        /// savaes the posted input data on to database
        /// </summary>
        /// <param name="comment"></param>
        /// <returns>Returns to BlogVeiwModelIndex view</returns>
        /// <includesource>yes</includesource>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCommentCreate([Bind(Include = "CommentId, CommentedDate, CommentTitle, CommentBody, BlogId, Id")] Comment comment)
        {
            if(comment.CommentBody == null)
            {
                ViewBag.Error = "Comment Context is required";
                return View();
            }
            //Receive blog TempData from previous view
            Blog passedblog = TempData["tempBlog"] as Blog;
            comment.BlogId = passedblog.BlogId;
            comment.Id = User.Identity.GetUserId();

            if (ModelState.IsValid)
            {
                db.Comments.Add(comment);
                db.SaveChanges();
                //pass chosen blog temp data to next blog
                TempData["tempBlog"] = passedblog;
                return RedirectToAction("BlogViewModelIndex", "BlogViewModels", new { blogid = passedblog.BlogId });
            }

            //pass chosen blog temp data to next blog
            TempData["tempBlog"] = passedblog;
            return View(comment);
        }

    }
}