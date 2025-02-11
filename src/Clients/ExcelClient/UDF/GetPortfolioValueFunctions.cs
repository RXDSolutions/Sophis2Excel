﻿//  Copyright (c) RXD Solutions. All rights reserved.
using ExcelDna.Integration;

namespace RxdSolutions.FusionLink.ExcelClient
{
    public static class GetPortfolioValueFunctions
    {
        [ExcelFunction(Name = "GETPORTFOLIOVALUE",
                       Description = "Returns a portfolio cell value",
                       HelpTopic = "Get-Portfolio-Value")]
        public static object GetPortfolioValue(int portfolioId, string column)
        {
            if (ExcelDnaUtil.IsInFunctionWizard())
                return null;

            return ExcelAsyncUtil.Observe(nameof(GetPortfolioValue), new object[] { portfolioId, column }, () => new PortfolioValueExcelObservable(portfolioId, column, AddIn.Client));
        }

        [ExcelFunction(Name = "PRV",
                       Description = "Returns a portfolio cell value",
                       HelpTopic = "Get-Portfolio-Value")]
        public static object PRV(int portfolioId, string column)
        {
            return GetPortfolioValue(portfolioId, column);
        }
    }
}
