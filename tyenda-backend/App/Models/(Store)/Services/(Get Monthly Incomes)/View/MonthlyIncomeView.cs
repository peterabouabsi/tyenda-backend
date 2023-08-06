using System;
using System.Collections.Generic;

namespace tyenda_backend.App.Models._Store_.Services._Get_Monthly_Incomes_.View
{
    public class MonthlyIncomePerYearView
    {
        public List<YearView> Years { get; set; } = new List<YearView>();
        public List<MonthlyIncomeView> Incomes { get; set; } = new List<MonthlyIncomeView>();
    }

    public class YearView
    {
        public int Id { get; set; }
        public int Value { get; set; }
    }
    public class MonthlyIncomeView{
        public string Month { get; set; } = "";
        public decimal Price { get; set; } = 0;
        public bool IsUp { get; set; } = false;
    }

}