using System.Collections.Generic;
using MediatR;
using tyenda_backend.App.Models._Store_.Services._Get_Monthly_Incomes_.View;

namespace tyenda_backend.App.Models._Store_.Services._Get_Monthly_Incomes_
{
    public class GetMonthlyIncomes : IRequest<MonthlyIncomePerYearView>
    {
        public GetMonthlyIncomes(int year)
        {
            Year = year;
        }

        public int Year { get; set; } = 0;
    }
}