using Xunit;
using Shouldly;
using api.Services;
using api.Repository;
using api.Servivces;
using System.Runtime.CompilerServices;
using NSubstitute;
using api.Interface;
using api.Helper;
using System.Linq.Expressions;
using api.models;
using api.Dtos;
public class StockServiceTest
{
    private readonly StockServices _stockServices;
    private readonly IStockRepository _stockRepository;
    public StockServiceTest()
    {
        _stockRepository = Substitute.For<IStockRepository>();
        _stockServices = new StockServices(_stockRepository);
    }
    
    [Theory]
    [InlineData(0, 1)]  // 输入 0，预期 Repository 收到 1
    [InlineData(-5, 1)] // 输入负数，预期 Repository 收到 1
    [InlineData(10, 10)] // 输入正常正数，保持原样
    public async Task GetAllStocks_Should_Correct_Invalid_PageNumber(int inputPage, int expectedPage)
    {
        // 1. Arrange
        var query = new QuerySearch { pageNumber = inputPage };
        _stockRepository.GetAllStocksAsync(Arg.Any<Expression<Func<Stock, bool>>>(), Arg.Any<int>())
            .Returns(new List<StockDto>()); // 返回空列表即可，我们关注的是参数

        // 2. Act
        await _stockServices.GetAllStocks(query);

        // 3. Assert
        // 验证 Repository 接收到的 pageNumber 参数是否符合预期
        await _stockRepository.Received(1).GetAllStocksAsync(
            Arg.Any<Expression<Func<Stock, bool>>>(),
            Arg.Is(expectedPage) // 关键点：断言传入的页码
        );
    }

    [Fact]
    public async Task GetAllStocks_WhenQueryIsEmpty_ShouldStillCallRepository()
    {
        // 1. Arrange
        var query = new QuerySearch { CompanyName = null, Symbol = null };
        var mockData = new List<StockDto> { new StockDto { Symbol = "MSFT" } };

        _stockRepository.GetAllStocksAsync(Arg.Any<Expression<Func<Stock, bool>>>(), Arg.Any<int>())
            .Returns(mockData);

        // 2. Act
        var result = await _stockServices.GetAllStocks(query);

        // 3. Assert
        result.Count.ShouldBe(1);
        result[0].Symbol.ShouldBe("MSFT");
    }
}