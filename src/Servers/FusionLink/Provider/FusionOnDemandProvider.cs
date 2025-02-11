﻿//  Copyright (c) RXD Solutions. All rights reserved.
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows.Threading;
using RxdSolutions.FusionLink.Helpers;
using RxdSolutions.FusionLink.Interface;
using RxdSolutions.FusionLink.Listeners;
using RxdSolutions.FusionLink.Services;
using sophis.instrument;
using sophis.portfolio;
using sophis.static_data;
using sophis.utils;
using sophisTools;

namespace RxdSolutions.FusionLink.Provider
{
    public class FusionOnDemandProvider : IOnDemandProvider, IDisposable
    {
        private readonly string _className = nameof(FusionOnDemandProvider);

        private readonly Dispatcher _context;
        private readonly PositionService _positionService;
        private readonly InstrumentService _instrumentService;
        private readonly CurrencyService _currencyService;
        private readonly CurveService _curveService;
        private readonly TransactionService _transactionService;
        private readonly PriceService _priceService;
        private readonly ReportService _reportService;

        public FusionOnDemandProvider(PositionService positionService,
                                        InstrumentService instrumentService,
                                        CurrencyService currencyService,
                                        CurveService curveService,
                                        TransactionService transactionService,
                                        PriceService priceService,
                                        ReportService reportService)
        {
            _context = Dispatcher.CurrentDispatcher;
            _positionService = positionService;
            _instrumentService = instrumentService;
            _curveService = curveService;
            _transactionService = transactionService;
            _priceService = priceService;
            _reportService = reportService;
            _currencyService = currencyService;
        }

        public List<int> GetPositions(int portfolioId, PositionsToRequest positions)
        {
            return _context.Invoke(() => {

                try
                {
                    return _positionService.GetPositions(portfolioId, positions);
                }
                catch(PortfolioNotLoadedException e)
                {
                    CSMLog.Write(_className, nameof(GetPositions), CSMLog.eMVerbosity.M_verbose, e.ToString());
                    throw;
                }
                catch(PortfolioNotFoundException e)
                {
                    CSMLog.Write(_className, nameof(GetPositions), CSMLog.eMVerbosity.M_verbose, e.ToString());
                    throw;
                }
                catch (Exception e)
                {
                    CSMLog.Write(_className, nameof(GetPositions), CSMLog.eMVerbosity.M_error, e.ToString());
                    throw;
                }

            });
        }

        public List<int> GetFlatPositions(int portfolioId, PositionsToRequest positions)
        {
            return _context.Invoke(() => {

                try
                {
                    return _positionService.GetFlatPositions(portfolioId, positions);
                }
                catch (PortfolioNotLoadedException e)
                {
                    CSMLog.Write(_className, nameof(GetFlatPositions), CSMLog.eMVerbosity.M_verbose, e.ToString());
                    throw;
                }
                catch (PortfolioNotFoundException e)
                {
                    CSMLog.Write(_className, nameof(GetFlatPositions), CSMLog.eMVerbosity.M_verbose, e.ToString());
                    throw;
                }
                catch (Exception e)
                {
                    CSMLog.Write(_className, nameof(GetFlatPositions), CSMLog.eMVerbosity.M_error, e.ToString());
                    throw;
                }

            });
        }

        public List<PriceHistory> GetInstrumentPriceHistory(string reference, DateTime startDate, DateTime endDate)
        {
            return _context.Invoke(() => {

                var instrumentId = CSMInstrument.GetCode(reference);

                return GetInstrumentPriceHistoryInternal(instrumentId, startDate, endDate);

            });
        }

        public List<PriceHistory> GetInstrumentPriceHistory(int instrumentId, DateTime startDate, DateTime endDate)
        {
            return _context.Invoke(() => {

                return GetInstrumentPriceHistoryInternal(instrumentId, startDate, endDate);

            });
        }

        public List<PriceHistory> GetCurrencyPriceHistory(string reference, DateTime startDate, DateTime endDate)
        {
            return _context.Invoke(() => {

                var currencyId = CSMCurrency.StringToCurrency(reference);

                return GetCurrencyPriceHistoryInternal(currencyId, startDate, endDate);

            });
        }

        public List<PriceHistory> GetCurrencyPriceHistory(int currencyId, DateTime startDate, DateTime endDate)
        {
            return _context.Invoke(() => {

                return GetCurrencyPriceHistoryInternal(currencyId, startDate, endDate);

            });
        }

        private List<PriceHistory> GetCurrencyPriceHistoryInternal(int currencyId, DateTime startDate, DateTime endDate)
        {
            try
            {
                return _priceService.GetCurrencyPriceHistory(currencyId, startDate, endDate);
            }
            catch (InstrumentNotFoundException e)
            {
                CSMLog.Write(_className, "GetPriceHistory", CSMLog.eMVerbosity.M_verbose, e.ToString());
                throw;
            }
            catch (Exception e)
            {
                CSMLog.Write(_className, "GetPriceHistory", CSMLog.eMVerbosity.M_error, e.ToString());
                throw;
            }
        }

        private List<PriceHistory> GetInstrumentPriceHistoryInternal(int instrumentId, DateTime startDate, DateTime endDate)
        {
            try
            {
                return _priceService.GetPriceHistory(instrumentId, startDate, endDate);
            }
            catch (InstrumentNotFoundException e)
            {
                CSMLog.Write(_className, "GetPriceHistory", CSMLog.eMVerbosity.M_verbose, e.ToString());
                throw;
            }
            catch (Exception e)
            {
                CSMLog.Write(_className, "GetPriceHistory", CSMLog.eMVerbosity.M_error, e.ToString());
                throw;
            }
        }

        public List<CurvePoint> GetCurvePoints(string currency, string family, string reference)
        {
            return _context.Invoke(() => {

                try
                {
                    return _curveService.GetCurvePoints(currency, family, reference);
                }
                catch(CurrencyNotFoundException e)
                {
                    CSMLog.Write(_className, nameof(GetCurvePoints), CSMLog.eMVerbosity.M_verbose, e.ToString());
                    throw;
                }
                catch (CurveFamilyFoundException e)
                {
                    CSMLog.Write(_className, nameof(GetCurvePoints), CSMLog.eMVerbosity.M_verbose, e.ToString());
                    throw;
                }
                catch (CurveNotFoundException e)
                {
                    CSMLog.Write(_className, nameof(GetCurvePoints), CSMLog.eMVerbosity.M_verbose, e.ToString());
                    throw;
                }
                catch (Exception e)
                {
                    CSMLog.Write(_className, nameof(GetCurvePoints), CSMLog.eMVerbosity.M_error, e.ToString());
                    throw;
                }
            });
        }

        public List<Transaction> GetPositionTransactions(int positionId, DateTime startDate, DateTime endDate)
        {
            return _context.Invoke(() => {

                try
                {
                    return _transactionService.GetPositionTransactions(positionId, startDate, endDate);
                }
                catch (PositionNotFoundException e)
                {
                    CSMLog.Write(_className, nameof(GetPositionTransactions), CSMLog.eMVerbosity.M_verbose, e.ToString());
                    throw;
                }
                catch (Exception e)
                {
                    CSMLog.Write(_className, nameof(GetPositionTransactions), CSMLog.eMVerbosity.M_error, e.ToString());
                    throw;
                }
            });
        }

        public List<Transaction> GetPortfolioTransactions(int portfolioId, DateTime startDate, DateTime endDate)
        {
            return _context.Invoke(() => {

                try
                {
                    return _transactionService.GetPortfolioTransactions(portfolioId, startDate, endDate);
                }
                catch (PortfolioNotFoundException e)
                {
                    CSMLog.Write(_className, nameof(GetPortfolioTransactions), CSMLog.eMVerbosity.M_verbose, e.ToString());
                    throw;
                }
                catch (PortfolioNotLoadedException e)
                {
                    CSMLog.Write(_className, nameof(GetPortfolioTransactions), CSMLog.eMVerbosity.M_verbose, e.ToString());
                    throw;
                }
                catch (Exception e)
                {
                    CSMLog.Write(_className, nameof(GetPortfolioTransactions), CSMLog.eMVerbosity.M_error, e.ToString());
                    throw;
                }
            });
        }

        public DateTime AddBusinessDays(DateTime date, int daysToAdd, string currency, CalendarType calendarType, string name)
        {
            return _context.Invoke(() => {

                try
                {
                    int currencyId = CSMCurrency.StringToCurrency(currency);
                    using var curObject = CSMCurrency.GetCSRCurrency(currencyId);

                    CSMCalendar GetCalendar()
                    {
                        switch (calendarType)
                        {
                            case CalendarType.Currency:
                                return curObject;

                            case CalendarType.Place:
                                {
                                    for(int i = 0; i < curObject.GetPlaceCount(); i++)
                                    {
                                        var place = curObject.GetNthPlace(i);

#if SOPHIS713
                                        using var placeName = new CMString();
                                        place.GetName(placeName);
#endif

#if SOPHIS2021
                                        using var placeName = place.GetName();
#endif

                                        if (placeName.StringValue == name)
                                        {
                                            return place;
                                        }
                                        
                                        if (placeName.StringValue == name)
                                        {
                                            return place;
                                        }
                                        
                                        place.Dispose();
                                    }

                                    return null;
                                }
                                
                            case CalendarType.Market:
                                {
                                    for (int i = 0; i < curObject.GetMarketCount(); i++)
                                    {
                                        using var market = curObject.GetNthMarket(i);

#if SOPHIS713
                                        using var marketName = new CMString();
                                        market.GetName(marketName);
#endif

#if SOPHIS2021
                                        using var marketName = market.GetName();
#endif

                                        if (marketName.StringValue == name)
                                        {
                                            return market;
                                        }
                                    }

                                    return null;
                                }

                            default:
                                throw new CalendarNotFoundException($"{name} with type {calendarType} cannot be found");
                        }
                    }

                    using var day = new CSMDay(date.Day, date.Month, date.Year);
                    using CSMCalendar calendarToUse = GetCalendar();
                    
                    if(calendarToUse is null)
                        throw new CalendarNotFoundException($"{currency} with type {calendarType} cannot be found");

                    int newDate = calendarToUse.AddNumberOfDays(day.toLong(), daysToAdd);

                    return (DateTime)newDate.ToDateTime();
                }
                catch (CalendarNotFoundException e)
                {
                    CSMLog.Write(_className, nameof(AddBusinessDays), CSMLog.eMVerbosity.M_verbose, e.ToString());
                    throw;
                }
                catch (Exception e)
                {
                    CSMLog.Write(_className, nameof(AddBusinessDays), CSMLog.eMVerbosity.M_error, e.ToString());
                    throw;
                }
            });
        }

        public DataTable GetInstrumentSet(int instrumentId, string property)
        {
            return _context.Invoke(() => {

                try
                {
                    return _instrumentService.GetInstrumentSet(instrumentId, property);
                }
                catch (InstrumentNotFoundException e)
                {
                    CSMLog.Write(_className, nameof(GetInstrumentSet), CSMLog.eMVerbosity.M_verbose, e.ToString());
                    throw;
                }
                catch (InvalidFieldException e)
                {
                    CSMLog.Write(_className, nameof(GetInstrumentSet), CSMLog.eMVerbosity.M_verbose, e.ToString());
                    throw;
                }
                catch (Exception e)
                {
                    CSMLog.Write(_className, nameof(GetInstrumentSet), CSMLog.eMVerbosity.M_error, e.ToString());
                    throw;
                }
            });
        }

        public DataTable GetInstrumentSet(string reference, string property)
        {
            return _context.Invoke(() => {

                try
                {
                    return _instrumentService.GetInstrumentSet(reference, property);
                }
                catch (InstrumentNotFoundException e)
                {
                    CSMLog.Write(_className, nameof(GetInstrumentSet), CSMLog.eMVerbosity.M_verbose, e.ToString());
                    throw;
                }
                catch (InvalidFieldException e)
                {
                    CSMLog.Write(_className, nameof(GetInstrumentSet), CSMLog.eMVerbosity.M_verbose, e.ToString());
                    throw;
                }
                catch (Exception e)
                {
                    CSMLog.Write(_className, nameof(GetInstrumentSet), CSMLog.eMVerbosity.M_error, e.ToString());
                    throw;
                }
            });
        }

        public DataTable GetCurrencySet(int currencyId, string property)
        {
            return _context.Invoke(() => {

                try
                {
                    return _currencyService.GetCurrencySet(currencyId, property);
                }
                catch (CurrencyNotFoundException e)
                {
                    CSMLog.Write(_className, nameof(GetCurrencySet), CSMLog.eMVerbosity.M_verbose, e.ToString());
                    throw;
                }
                catch (InvalidFieldException e)
                {
                    CSMLog.Write(_className, nameof(GetCurrencySet), CSMLog.eMVerbosity.M_verbose, e.ToString());
                    throw;
                }
                catch (Exception e)
                {
                    CSMLog.Write(_className, nameof(GetCurrencySet), CSMLog.eMVerbosity.M_error, e.ToString());
                    throw;
                }
            });
        }

        public DataTable GetCurrencySet(string reference, string property)
        {
            return _context.Invoke(() => {

                try
                {
                    return _currencyService.GetCurrencySet(reference, property);
                }
                catch (CurrencyNotFoundException e)
                {
                    CSMLog.Write(_className, nameof(GetCurrencySet), CSMLog.eMVerbosity.M_verbose, e.ToString());
                    throw;
                }
                catch (InvalidFieldException e)
                {
                    CSMLog.Write(_className, nameof(GetCurrencySet), CSMLog.eMVerbosity.M_verbose, e.ToString());
                    throw;
                }
                catch (Exception e)
                {
                    CSMLog.Write(_className, nameof(GetCurrencySet), CSMLog.eMVerbosity.M_error, e.ToString());
                    throw;
                }
            });
        }

        public DataTable GetReportSqlSourceResults(string reportName, string sourceName)
        {
            return _context.Invoke(() => {

                try
                {
                    return _reportService.GetReportSqlSourceResults(reportName, sourceName);
                }
                catch (ReportNotFoundException e)
                {
                    CSMLog.Write(_className, nameof(GetReportSqlSourceResults), CSMLog.eMVerbosity.M_verbose, e.ToString());
                    throw;
                }
                catch (Exception e)
                {
                    CSMLog.Write(_className, nameof(GetReportSqlSourceResults), CSMLog.eMVerbosity.M_error, e.ToString());
                    throw;
                }
            });
        }

        #region IDisposable Support

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}