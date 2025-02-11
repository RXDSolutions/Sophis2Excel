﻿//  Copyright (c) RXD Solutions. All rights reserved.


namespace RxdSolutions.FusionLink.Client
{
    public static class ExcelHelper
    {
        public static string GetPositionFormula(long id, string column)
        {
            return $"=GETPOSITIONVALUE({id}, \"{column}\")";
        }

        public static string GetFlatPositionFormula(int portfolioId, int instrumentId, string column)
        {
            return $"=GETFLATPOSITIONVALUE({portfolioId}, {instrumentId}, \"{column}\")";
        }

        public static string GetPortfolioFormula(long id, string column)
        {
            return $"=GETPORTFOLIOVALUE({id}, \"{column}\")";
        }
    }
}