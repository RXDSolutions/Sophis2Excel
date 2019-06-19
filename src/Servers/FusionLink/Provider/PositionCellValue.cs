﻿//  Copyright (c) RXD Solutions. All rights reserved.
//  FusionLink is licensed under the MIT license. See LICENSE.txt for details.

using System.Runtime.ExceptionServices;
using RxdSolutions.FusionLink.Properties;
using sophis.portfolio;

namespace RxdSolutions.FusionLink
{
    internal class PositionCellValue : CellValueBase
    {
        public CSMPosition Position { get; private set; }

        public int PositionId { get; }

        public PositionCellValue(int positionId, string column) : base(column)
        {
            PositionId = positionId;
            Position = CSMPosition.GetCSRPosition(positionId);
        }

        [HandleProcessCorruptedStateExceptions]
        public override object GetValue()
        {
            if (Position is null)
            {
                Position = CSMPosition.GetCSRPosition(PositionId);
            }

            if (Column is null)
            {
                return string.Format(Resources.ColumnNotFoundMessage, ColumnName);
            }

            if (Position is object)
            {
                Column.GetPositionCell(Position, Position.GetPortfolioCode(), Position.GetPortfolioCode(), sophis.globals.CSMExtraction.gMain(), 0, Position.GetInstrumentCode(), ref CellValue, CellStyle, false);

                var value = CellValue.ExtractValueFromSophisCell(CellStyle);

                return value;
            }
            else
            {
                return string.Format(Resources.PositionNotLoadedOrMissingMessage, PositionId);
            }
        }

        private bool disposedValue = false;

        protected override void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Position?.Dispose();
                }

                disposedValue = true;
            }

            base.Dispose(disposing);
        }
    }
}