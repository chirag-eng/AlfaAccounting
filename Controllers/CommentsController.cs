using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AlfaAccounting.Models;
using Microsoft.AspNet.Identity;
/// <summary>
/// Name:Mie Tanaka
/// Name:26/05/2017
/// Description: allows users to edit and delete their own comments


namespace AlfaAccounting.Controllers
{

    public class CommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Comments
        //public ActionResult Index()
        //{
        //    var comments = db.Comments.Include(c => c.ApplicationUser).Include(c => c.Blog);
        //    return View(comments.ToList());
        //}

        //// GET: Comments/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Comment comment = db.Comments.Find(id);
        //    if (comment == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(comment);
        //}


        /// <summary>
        /// gets tempdata tempBlog from BlogViewModelIndex
        /// creates new comment with passed blog id and user id from the model
        /// Returns the blank comment form
        /// </summary>
        /// <returns>Returns the blank comment form</returns>
        /// <includesource>yes</includesource>
        public ActionResult Create()
        {
            //Recieve blog TempData from previous view
            Blog passedblog = TempData["tempBlog"] as Blog;
            //            ViewBag.Id = new SelectList(db.Users, "Id", "Forename");
  //          ViewBag.BlogId = new SelectList(db.Blogs, "BlogId", "BlogTitle");
            var cmt = new Comment();
            cmt.CommentedDate = DateTime.Today;
            cmt.BlogId = passedblog.BlogId;
            cmt.Id = User.Identity.GetUserId();
            ///pass blog tempData to next View
            TempData["tempBlog"] = passedblog;
            return View(cmt);
        }

        /// <summary>
        /// gets tempdata tempBlog from BlogViewModelIndex
        /// and gets the posted comment data
        /// comment's bologid and Userid is null at this point
        /// it get filled with the passed temp data
        /// saves the comment to database
        /// </summary>
        /// <param name="comment"></param>
        /// <returns>Returns BlogViewModelIndex view</returns>
        /// <includesource>yes</includesource>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CommentId,CommentedDate,CommentTitle,CommentBody,BlogId,Id")] Comment comment)
        {
            //Recieve blog TempData from previous view
            Blog passedblog = TempData["tempBlog"] as Blog;
            comment.CommentedDate = DateTime.Today;
            comment.BlogId = passedblog.BlogId;
            comment.Id = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                db.Comments.Add(comment);
                db.SaveChanges();
                return RedirectToAction("BlogViewModelIndex", "BlogViewModels", new { blogid = comment.BlogId });
            }

            ///pass blog tempData to next View
            TempData["tempBlog"] = passedblog;
            return View(comment);
        }

        // GET: Comments/Edit/5
        /// <summary>
        /// Returns Comments/Edit view with selected user comment
        /// </summary>
        /// <param name="id"></param>
        /// <returns>comment</returns>
        /// <includesource>yes</includesource>
        public ActionResult Edit(int? id)
        {
            //Recieve blog TempData from previous view
                 Blog passedblog = TempData["tempBlog"] as Blog;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = new SelectList(db.Users, "Id", "Forename", comment.Id);
            ViewBag.BlogId = new SelectList(db.Blogs, "BlogId", "BlogTitle", comment.BlogId);
            ///pass blog tempData to next View
            TempData["tempComment"] = comment;
            return View(comment);
        }


        /// <summary>
        /// Returns BlogViewModel/BlogViewModelIndex view
        /// after successfully saving the valid edited data onto database
        /// </summary>
        /// <param name="comment"></param>
        /// <returns>BlogViewmodels/BlogViewModelIndex view</returns>
        /// <includesource>yes</includesource>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CommentId,CommentedDate,CommentTitle,CommentBody,BlogId,Id")] Comment comment)
        {
            //Recieve blog TempData from previous view
            Comment passedComment = TempData["tempComment"] as Comment;
            comment.Id = passedComment.Id ;
            comment.BlogId = passedComment.BlogId;
            comment.CommentedDate = passedComment.CommentedDate;
            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("BlogViewModelIndex", "BlogViewModels", new { blogid = comment.BlogId });
            }
            ViewBag.Id = new SelectList(db.Users, "Id", "Forename", comment.Id);
            ViewBag.BlogId = new SelectList(db.Blogs, "BlogId", "BlogTitle", comment.BlogId);
            ///pass blog tempData to next View
       //     TempData["tempBlog"] = db.Blogs.Find(comment.BlogId);

            return View(comment);
        }


        // POST: Comments/Delete/5
        /// <summary>
        /// Returns BlogViewModelIndex 
        /// after successfully deleteing the comment from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns> BlogViewModelIndex </returns>
        /// <includesource>yes</includesource>
        //[HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            Comment comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
            db.SaveChanges();
            return RedirectToAction("BlogViewModelIndex", "BlogViewModels", new { blogid = comment.BlogId });
        }

        /// <summary>
        /// dispose the database memory of the deleted comment
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
