using api.data;
using api.Dtos.Comment;
using api.Extensions;
using api.Interface;
using api.Mappers;
using api.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[Route("api/comment")]
public class CommentController : ControllerBase
{

    private readonly ICommentRepository _commentRepository;
    private readonly IStockRepository _stockRepository;
    private readonly UserManager<AppUser> _userManager;
    public CommentController( ICommentRepository commentRepository, IStockRepository stockRepository, UserManager<AppUser> userManager)
    {
        _commentRepository = commentRepository;
        _stockRepository = stockRepository;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllComments()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var comments = await _commentRepository.GetAllCommentsAsync();
        var commentDtos = comments.Select(c => c.ToCommentDto());
        return Ok(commentDtos);
    }

    [HttpGet("{commentId:int}")]
    public async Task<IActionResult> GetCommentsByCommentId([FromRoute] int commentId){
    //validation
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var comment = await _commentRepository.GetCommentByIdAsync(commentId);
        if(comment == null){
            return NotFound();
        } 
        return Ok(comment.ToCommentDto());  
    }

    [HttpPost("{stockId:int}")]
    [Authorize]
    public async  Task<IActionResult> AddComment([FromRoute] int stockId, [FromBody] CreateCommentDto commentDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var stock = await _stockRepository.StockExistsAsync(stockId);
        if (!stock)
        {
            return NotFound($"Stock with ID {stockId} not found.");
        }
        var userName = User.GetUserName();
        var AppUser = await _userManager.FindByNameAsync(userName);
        var newComment = new Comment
        {
            Title = commentDto.Title,
            content = commentDto.content,
            stockId = stockId,
            AppUserId = AppUser.Id
        };
        var comment = await _commentRepository.AddCommentAsync(newComment);
        return CreatedAtAction(nameof(GetCommentsByCommentId), new { commentId = comment.Id }, comment.ToCommentDto());
    }

    [HttpPut("{commentId:int}")]
    public async Task<IActionResult> UpdateComment([FromRoute] int commentId, [FromBody] UpdateCommentDto updatedComment)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var comment = await _commentRepository.UpdateCommentAsync(commentId, updatedComment.ToCommentModelFromUpdateDto());
        if (comment == null)
        {
            return NotFound();
        }
        return Ok(comment.ToCommentDto());
    }

    [HttpDelete]
    [Route("{commentId:int}")]
    public async Task<IActionResult> DeleteComment([FromRoute] int commentId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var comment = await _commentRepository.DeleteCommentAsync(commentId);
        if (comment == null)
        {
            return NotFound();
        }
        return Ok(comment.ToCommentDto());
    }
}