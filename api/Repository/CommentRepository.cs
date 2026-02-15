using api.data;
using api.Repository;
using api.models;
using Microsoft.EntityFrameworkCore;
using api.Interface;
using api.Dtos.Comment;

namespace api.Repository;

public class CommentRepository : ICommentRepository
{
    private readonly ApplicationDbContext _context;

    public CommentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Comment>> GetAllCommentsAsync()
    {
        return await _context.Comments.Include(c=>c.AppUser).ToListAsync();
    }

    public async Task<Comment> AddCommentAsync(Comment comment)
    {
        await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync();
        return comment;
    }

    public async Task<Comment?> GetCommentByIdAsync(int id)
    {
        var comment = await _context.Comments.Include(c => c.AppUser).FirstOrDefaultAsync(x => x.Id == id);
        return comment;
    }

  public async Task<Comment?> UpdateCommentAsync(int id, Comment updatedComment)
  {
    var comment =await _context.Comments.FirstOrDefaultAsync(x => x.Id == id);
    if (comment == null)
    {
      return null;
    }
    comment.Title = updatedComment.Title;
    comment.content = updatedComment.content;
    await _context.SaveChangesAsync(); 
    return comment;
  }

  public async Task<Comment> DeleteCommentAsync(int id)
  {
    var comment = await _context.Comments.FirstOrDefaultAsync(x => x.Id == id);
    if (comment == null)
    {
      return null;
    }
    _context.Comments.Remove(comment);
    await _context.SaveChangesAsync();
    return comment;
  }
}