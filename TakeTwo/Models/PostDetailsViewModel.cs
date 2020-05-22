using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace TakeTwo.Models
{
    public class PostDetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string AuthorId { get; set; }
        public IEnumerable<CommentViewModel> Comments { get; set; }
        public static Expression<Func<Post, PostDetailsViewModel>> ViewModel
        {
            get
            {
                return p => new PostDetailsViewModel()
                {
                    Id = p.Id,
                    Title = p.Title,
                    Body = p.Body,
                    Comments = p.Comments.AsQueryable().Select(CommentViewModel.ViewModel),
                    AuthorId = p.Author.Id
                };
            }
        }
    }

    public class CommentViewModel
    {
        public string Text { get; set; }
        public string Author { get; set; }
        public int CommentId { get; set; }
        public static Expression<Func<Comment, CommentViewModel>> ViewModel
        {
            get
            {
                return c => new CommentViewModel()
                {
                    Text = c.Text,
                    Author = c.Author.FullName,
                    CommentId = c.Id
                };
            }
        }
    }
}