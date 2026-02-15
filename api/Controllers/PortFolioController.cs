using System.Security.Claims;
using api.Interface;
using api.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using api.Extensions;
using api.Repository;
namespace api.Controllers;

[Route("api/portfolio")]
[ApiController]
public class PortFolioController : ControllerBase
{
  private readonly UserManager<AppUser> _userManager;
  private readonly IPortFolioRepository _portFolioRepository;
  private readonly IStockRepository _stockRepository;
 public PortFolioController(UserManager<AppUser> userManager, IPortFolioRepository portFolioRepository, IStockRepository stockRepository)
 {
  _userManager = userManager;
  _portFolioRepository = portFolioRepository;
  _stockRepository = stockRepository;
 }

 [HttpGet]
 [Authorize]
 public async Task<ActionResult> GetUserPortFolio()
 {
  // User is created by the Identity framework and it is stored in the HttpContext.User.Claims
  var userName = User.GetUserName();
  var portfolios = await  _portFolioRepository.GetUserPortFolios(userName);
  return Ok(portfolios);
 }

  [HttpPost]
  [Route("add")]
  [Authorize]
 public async Task<ActionResult> CreatePortFolio([FromBody]string sympol)
 {
  var userName = User.GetUserName();
  var appUser = await _userManager.FindByNameAsync(userName);
  var stock = await _stockRepository.GetBySympolAsync(sympol);
  if(stock == null)
  {
    return BadRequest("Stock not found");
  }

  var getUserPortFolio = await _portFolioRepository.GetUserPortFolios(userName);
  
  if(getUserPortFolio.Any(x=>x.Symbol.ToLower() == sympol.ToLower())) return BadRequest("Stock already in portfolio");

  var newPortFolio = new Portfolio
  {
    AppUserId = appUser.Id,
    StockId = stock.Id
  }; 
  var result = await _portFolioRepository.AddToPortFolio(newPortFolio);

  if(result == null) return StatusCode(500,"Failed to add stock to portfolio");

  return Ok(newPortFolio);
 }

 [Authorize]
 [HttpDelete]
 [Route("delete/{sympol}")]
  public async Task<ActionResult> DeleteFromPortFolio(string sympol)
  {
    var userName = User.GetUserName();
    var stock = await _stockRepository.GetBySympolAsync(sympol);
    if(stock == null)
    {
      return BadRequest("Stock not found");
    }
    var result = await _portFolioRepository.DeleteFromPortFolio(userName, stock.Id);
    if(result == false) return StatusCode(500,"stock is not in portfolio");  
    return Ok(result);
  }
}