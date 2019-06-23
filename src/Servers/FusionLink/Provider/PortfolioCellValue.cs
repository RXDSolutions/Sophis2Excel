﻿//  Copyright (c) RXD Solutions. All rights reserved.
//  FusionLink is licensed under the MIT license. See LICENSE.txt for details.

using System;
using System.Runtime.ExceptionServices;
using RxdSolutions.FusionLink.Properties;
using sophis.portfolio;

namespace RxdSolutions.FusionLink
{
    internal class PortfolioCellValue : CellValueBase
    {
        public CSMPortfolio Portfolio { get; }

        public int FolioId { get; }

        public PortfolioCellValue(int folioId, string column) : base(column)
        {
            FolioId = folioId;
            Portfolio = CSMPortfolio.GetCSRPortfolio(folioId);
        }

        [HandleProcessCorruptedStateExceptions]
        public override object GetValue()
        {
            try
            {
                if(Error is object)
                {
                    return Error.Message;
                }

                if (Portfolio is null)
                {
                    return string.Format(Resources.PortfolioNotFoundMessage, FolioId);
                }

                if (!Portfolio.IsLoaded())
                {
                    return string.Format(Resources.PortfolioNotLoadedMessage, FolioId);
                }

                if (Column is null)
                {
                    return string.Format(Resources.ColumnNotFoundMessage, ColumnName);
                }

                Column.GetPortfolioCell(Portfolio.GetCode(), Portfolio.GetCode(), sophis.globals.CSMExtraction.gMain(), ref CellValue, CellStyle, false);

                var value = CellValue.ExtractValueFromSophisCell(CellStyle);

                return value;
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        private bool disposedValue = false;

        protected override void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Portfolio?.Dispose();
                }

                disposedValue = true;
            }

            base.Dispose(disposing);
        }
    }
}