﻿//  Copyright (c) RXD Solutions. All rights reserved.
using System;
using System.Collections.Generic;
using System.Data;
using System.ServiceModel;
using RxdSolutions.FusionLink.Interface;

namespace RxdSolutions.FusionLink
{
    public class OnDemandDataServer : IOnDemandServer
    {
        private readonly IOnDemandProvider _onDemandProvider;

        public OnDemandDataServer()
        {

        }

        public OnDemandDataServer(IOnDemandProvider onDemandProvider)
        {
            _onDemandProvider = onDemandProvider;
        }

        public List<int> GetPositions(int portfolioId, PositionsToRequest position)
        {
            try
            {
                return _onDemandProvider.GetPositions(portfolioId, position);
            }
            catch (PortfolioNotFoundException)
            {
                throw new FaultException<PortfolioNotFoundFaultContract>(new PortfolioNotFoundFaultContract() { PortfolioId = portfolioId });
            }
            catch (PortfolioNotLoadedException)
            {
                throw new FaultException<PortfolioNotLoadedFaultContract>(new PortfolioNotLoadedFaultContract() { PortfolioId = portfolioId });
            }
            catch (Exception ex)
            {
                throw new FaultException<ErrorFaultContract>(new ErrorFaultContract() { Message = ex.Message });
            }
        }

        public List<int> GetFlatPositions(int portfolioId, PositionsToRequest position)
        {
            try
            {
                return _onDemandProvider.GetFlatPositions(portfolioId, position);
            }
            catch (PortfolioNotFoundException)
            {
                throw new FaultException<PortfolioNotFoundFaultContract>(new PortfolioNotFoundFaultContract() { PortfolioId = portfolioId });
            }
            catch (PortfolioNotLoadedException)
            {
                throw new FaultException<PortfolioNotLoadedFaultContract>(new PortfolioNotLoadedFaultContract() { PortfolioId = portfolioId });
            }
            catch (Exception ex)
            {
                throw new FaultException<ErrorFaultContract>(new ErrorFaultContract() { Message = ex.Message });
            }
        }

        public List<PriceHistory> GetInstrumentPriceHistory(int instrumentId, DateTime startDate, DateTime endDate)
        {
            try
            {
                return _onDemandProvider.GetInstrumentPriceHistory(instrumentId, startDate, endDate);
            }
            catch (InstrumentNotFoundException)
            {
                throw new FaultException<InstrumentNotFoundFaultContract>(new InstrumentNotFoundFaultContract() { Instrument = instrumentId.ToString() });
            }
            catch (Exception ex)
            {
                throw new FaultException<ErrorFaultContract>(new ErrorFaultContract() { Message = ex.Message });
            }
        }

        public List<PriceHistory> GetInstrumentPriceHistory(string reference, DateTime startDate, DateTime endDate)
        {
            try
            {
                return _onDemandProvider.GetInstrumentPriceHistory(reference, startDate, endDate);
            }
            catch (InstrumentNotFoundException)
            {
                throw new FaultException<InstrumentNotFoundFaultContract>(new InstrumentNotFoundFaultContract() { Instrument = reference });
            }
            catch (Exception ex)
            {
                throw new FaultException<ErrorFaultContract>(new ErrorFaultContract() { Message = ex.Message });
            }
        }

        public List<PriceHistory> GetCurrencyPriceHistory(int currencyId, DateTime startDate, DateTime endDate)
        {
            try
            {
                return _onDemandProvider.GetCurrencyPriceHistory(currencyId, startDate, endDate);
            }
            catch (CurrencyNotFoundException)
            {
                throw new FaultException<CurrencyNotFoundFaultContract>(new CurrencyNotFoundFaultContract() { Currency = currencyId.ToString() });
            }
            catch (Exception ex)
            {
                throw new FaultException<ErrorFaultContract>(new ErrorFaultContract() { Message = ex.Message });
            }
        }

        public List<PriceHistory> GetCurrencyPriceHistory(string reference, DateTime startDate, DateTime endDate)
        {
            try
            {
                return _onDemandProvider.GetCurrencyPriceHistory(reference, startDate, endDate);
            }
            catch (CurrencyNotFoundException)
            {
                throw new FaultException<CurrencyNotFoundFaultContract>(new CurrencyNotFoundFaultContract() { Currency = reference });
            }
            catch (Exception ex)
            {
                throw new FaultException<ErrorFaultContract>(new ErrorFaultContract() { Message = ex.Message });
            }
        }

        public List<CurvePoint> GetCurvePoints(string currency, string family, string reference)
        {
            try
            {
                return _onDemandProvider.GetCurvePoints(currency, family, reference);
            }
            catch (CurrencyNotFoundException)
            {
                throw new FaultException<CurrencyNotFoundFaultContract>(new CurrencyNotFoundFaultContract() { Currency = currency });
            }
            catch (CurveFamilyFoundException)
            {
                throw new FaultException<CurveFamilyNotFoundFaultContract>(new CurveFamilyNotFoundFaultContract() { CurveFamily = family });
            }
            catch (CurveNotFoundException)
            {
                throw new FaultException<CurveNotFoundFaultContract>(new CurveNotFoundFaultContract() { Curve = reference });
            }
            catch (Exception ex)
            {
                throw new FaultException<ErrorFaultContract>(new ErrorFaultContract() { Message = ex.Message });
            }
        }

        public DateTime AddBusinessDays(DateTime date, int daysToAdd, string currency, CalendarType calendarType, string name)
        {
            try
            {
                return _onDemandProvider.AddBusinessDays(date, daysToAdd, currency, calendarType, name);
            }
            catch (CalendarNotFoundException)
            {
                throw new FaultException<CalendarNotFoundFaultContract>(new CalendarNotFoundFaultContract() { Calendar = $"{currency} - {calendarType} - {name}" });
            }
            catch (Exception ex)
            {
                throw new FaultException<ErrorFaultContract>(new ErrorFaultContract() { Message = ex.Message });
            }
        }

        public List<Transaction> GetPositionTransactions(int positionId, DateTime startDate, DateTime endDate)
        {
            try
            {
                return _onDemandProvider.GetPositionTransactions(positionId, startDate, endDate);
            }
            catch (PositionNotFoundException)
            {
                throw new FaultException<PositionNotFoundFaultContract>(new PositionNotFoundFaultContract() { PositionId = positionId });
            }
            catch (Exception ex)
            {
                throw new FaultException<ErrorFaultContract>(new ErrorFaultContract() { Message = ex.Message });
            }
        }

        public List<Transaction> GetPortfolioTransactions(int portfolioId, DateTime startDate, DateTime endDate)
        {
            try
            {
                return _onDemandProvider.GetPortfolioTransactions(portfolioId, startDate, endDate);
            }
            catch (PortfolioNotFoundException)
            {
                throw new FaultException<PortfolioNotFoundFaultContract>(new PortfolioNotFoundFaultContract() { PortfolioId = portfolioId });
            }
            catch (PortfolioNotLoadedException)
            {
                throw new FaultException<PortfolioNotLoadedFaultContract>(new PortfolioNotLoadedFaultContract() { PortfolioId = portfolioId });
            }
            catch (Exception ex)
            {
                throw new FaultException<ErrorFaultContract>(new ErrorFaultContract() { Message = ex.Message });
            }
        }

        public DataTable GetInstrumentSet(int instrumentId, string property)
        {
            try
            {
                return _onDemandProvider.GetInstrumentSet(instrumentId, property);
            }
            catch (InstrumentNotFoundException)
            {
                throw new FaultException<InstrumentNotFoundFaultContract>(new InstrumentNotFoundFaultContract() { Instrument = instrumentId.ToString() });
            }
            catch (Exception ex)
            {
                throw new FaultException<ErrorFaultContract>(new ErrorFaultContract() { Message = ex.Message });
            }
        }

        public DataTable GetInstrumentSet(string reference, string property)
        {
            try
            {
                return _onDemandProvider.GetInstrumentSet(reference, property);
            }
            catch (InvalidFieldException)
            {
                throw new FaultException<InvalidFieldFaultContract>(new InvalidFieldFaultContract());
            }
            catch (InstrumentNotFoundException)
            {
                throw new FaultException<InstrumentNotFoundFaultContract>(new InstrumentNotFoundFaultContract() { Instrument = reference});
            }
            catch (Exception ex)
            {
                throw new FaultException<ErrorFaultContract>(new ErrorFaultContract() { Message = ex.Message });
            }
        }

        public DataTable GetReportSqlSourceResults(string reportName, string sourceName)
        {
            try
            {
                return _onDemandProvider.GetReportSqlSourceResults(reportName, sourceName);
            }
            catch (ReportNotFoundException)
            {
                throw new FaultException<ReportNotFoundFaultContract>(new ReportNotFoundFaultContract() { Report = reportName, Source = sourceName });
            }
            catch (Exception ex)
            {
                throw new FaultException<ErrorFaultContract>(new ErrorFaultContract() { Message = ex.Message });
            }
        }

        public DataTable GetCurrencySet(int currencyId, string property)
        {
            try
            {
                return _onDemandProvider.GetCurrencySet(currencyId, property);
            }
            catch (CurrencyNotFoundException)
            {
                throw new FaultException<CurrencyNotFoundFaultContract>(new CurrencyNotFoundFaultContract() { Currency = currencyId.ToString() });
            }
            catch (Exception ex)
            {
                throw new FaultException<ErrorFaultContract>(new ErrorFaultContract() { Message = ex.Message });
            }
        }

        public DataTable GetCurrencySet(string reference, string property)
        {
            try
            {
                return _onDemandProvider.GetCurrencySet(reference, property);
            }
            catch (InvalidFieldException)
            {
                throw new FaultException<InvalidFieldFaultContract>(new InvalidFieldFaultContract());
            }
            catch (CurrencyNotFoundException)
            {
                throw new FaultException<CurrencyNotFoundFaultContract>(new CurrencyNotFoundFaultContract() { Currency = reference });
            }
            catch (Exception ex)
            {
                throw new FaultException<ErrorFaultContract>(new ErrorFaultContract() { Message = ex.Message });
            }
        }
    }
}