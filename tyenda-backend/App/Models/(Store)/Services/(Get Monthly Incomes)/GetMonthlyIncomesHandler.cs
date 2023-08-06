using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;
using tyenda_backend.App.Models._Store_.Services._Get_Monthly_Incomes_.View;
using tyenda_backend.App.Models.Enums;
using tyenda_backend.App.Services.Token_Service;

namespace tyenda_backend.App.Models._Store_.Services._Get_Monthly_Incomes_
{
    public class GetMonthlyIncomesHandler : IRequestHandler<GetMonthlyIncomes, MonthlyIncomePerYearView>
    {
        private readonly TyendaContext _context;
        private readonly ITokenService _tokenService;

        public GetMonthlyIncomesHandler(TyendaContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<MonthlyIncomePerYearView> Handle(GetMonthlyIncomes request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Year == 0)
                {
                    throw new Exception("Something went wrong. Year not mentioned");
                }
                
                var accountId = _tokenService.GetHeaderTokenClaim(Constants.AccountId);
                var account = await _context.Accounts
                    .Include(account => account.Store)
                    .SingleOrDefaultAsync(account => account.Id == Guid.Parse(accountId),cancellationToken);
                if (account == null)
                {
                    throw new UnauthorizedAccessException("Account not found");
                }

                var orders = await _context.Orders
                    .Where(order => 
                        order.OrderStatus == OrderStatus.Completed && 
                        order.Item!.StoreId == account.Store!.Id && 
                        order.UpdatedAt.Year == request.Year)
                    .Include(order => order.Item)
                    .Include(order => order.OrderItems)
                    .GroupBy(order => order.UpdatedAt.Month)
                    .ToDictionaryAsync(
                        group => CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(group.Key),
                        group => group.ToArray(),
                        cancellationToken);
                
                List<YearView> years = await _context.Orders
                    .Select(order => new YearView
                    {
                        Id = order.UpdatedAt.Year,
                        Value = order.UpdatedAt.Year
                    })
                    .Distinct()
                    .ToListAsync(cancellationToken);
                
                List<MonthlyIncomeView> formattedOrders = new List<MonthlyIncomeView>();
                int index = 0;
                foreach (var keyValue in orders)
                {
                    
                    var monthlyIncome = new MonthlyIncomeView()
                    {
                        Month = keyValue.Key,
                        Price = 0,
                        IsUp = false
                    };
                    
                    foreach (var order in keyValue.Value)
                    {
                        if (order.Item!.Discount == 0)
                        {
                            monthlyIncome.Price += order.Item!.Price * order.OrderItems.Sum(orderItem => orderItem.Quantity);
                        }
                        else
                        {
                            monthlyIncome.Price += (order.Item!.Price -
                                                   (order.Item!.Price * ((decimal) order.Item!.Discount / 100))) *
                                                   (order.OrderItems.Sum(orderItem => orderItem.Quantity));
                        }
                    }
                    
                    monthlyIncome.IsUp = index > 0? formattedOrders[index-1].Price < monthlyIncome.Price : true;
                    formattedOrders.Add(monthlyIncome);
                    index++;
                }

                var monthlyIncomePerYear = new MonthlyIncomePerYearView()
                {
                    Years = years,
                    Incomes = formattedOrders
                };
                    
                return monthlyIncomePerYear;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }
    }
}