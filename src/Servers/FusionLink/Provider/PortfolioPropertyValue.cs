﻿//  Copyright (c) RXD Solutions. All rights reserved.
using System;
using System.Runtime.ExceptionServices;
using RxdSolutions.FusionLink.Interface;
using RxdSolutions.FusionLink.Properties;
using sophis.portfolio;
using sophis.utils;

namespace RxdSolutions.FusionLink.Provider
{
    internal class PortfolioPropertyValue : IDisposable
    {
        public CSMPortfolio Portfolio { get; }

        public int FolioId { get; }

        public PortfolioProperty Property { get; }

        public Exception Error { get; set; }

        public PortfolioPropertyValue(int folioId, PortfolioProperty property)
        {
            FolioId = folioId;
            Property = property;
            Portfolio = CSMPortfolio.GetCSRPortfolio(folioId);
        }

        [HandleProcessCorruptedStateExceptions]
        public object GetValue()
        {
            try
            {
                if (Error is object)
                {
                    return Error.Message;
                }

                if (Portfolio is null)
                {
                    return string.Format(Resources.PortfolioNotFoundMessage, FolioId);
                }

                switch (Property)
                {
                    case PortfolioProperty.Name:
                        using (var name = new CMString())
                        {
                            Portfolio.GetName(name);
                            return name.StringValue;
                        }

                    case PortfolioProperty.ParentId:
                        return Portfolio.GetParentCode();

                    case PortfolioProperty.FullPath:
                        using (var fullName = new CMString())
                        {
                            Portfolio.GetFullName(fullName);
                            return fullName.StringValue;
                        }

                    case PortfolioProperty.Locked:
                        return Portfolio.IsLocked();
                        
                    case PortfolioProperty.Comment:
                        using (var comment = Portfolio.GetComment())
                            return comment.StringValue;

                    case PortfolioProperty.Closed:
                        return Portfolio.IsMarkedAsClosed();
                        
                    case PortfolioProperty.Entity:
                        return Portfolio.GetEntity();
                        
                    case PortfolioProperty.Currency:
                        return Portfolio.GetCurrency(); 
                        
                    case PortfolioProperty.Security:
                        return Portfolio.GetUnderlyingCode(); 
                    
                    default:
                        return $"Unknown Portfolio Property '{Property}'";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        #region IDisposable Support

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Portfolio?.Dispose();
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