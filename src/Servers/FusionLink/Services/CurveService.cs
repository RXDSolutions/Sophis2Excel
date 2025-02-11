﻿//  Copyright (c) RXD Solutions. All rights reserved.


using System.Collections.Generic;
using sophis.market_data;
using sophis.static_data;

namespace RxdSolutions.FusionLink.Services
{
    public class CurveService
    {
        public List<CurvePoint> GetCurvePoints(string currency, string family, string reference)
        {
            int currencyCode = CSMCurrency.StringToCurrency(currency);

            if (currencyCode == 0)
            {
                throw new CurrencyNotFoundException();
            }

            int familyCode = CSMYieldCurveFamily.GetYieldCurveFamilyCode(currencyCode, family);

            if (familyCode == 0)
            {
                throw new CurveFamilyFoundException();
            }

            int curveId = CSMYieldCurve.LookUpYieldCurveId(familyCode, reference);

            if (curveId == 0)
            {
                throw new CurveNotFoundException();
            }

            var results = new List<CurvePoint>();

            using var yieldCurve = CSMYieldCurve.GetCSRYieldCurve(curveId);
            using SSMYieldCurve activeCurve = yieldCurve.GetActiveSSYieldCurve();
            
            for (int i = 0; i < GetPointCount(activeCurve); i++)
            {
                using var yieldPoint = GetPointList(activeCurve).GetNthElement(i);

                var cp = new CurvePoint();
                results.Add(cp);

                double multiplier = 1;

#if SOPHIS713
                if (yieldPoint.fType == 'x')
#endif

#if SOPHIS2021
                if (yieldPoint.fType == eMPeriodNature.M_eRelativeEndMonthFuture)
#endif
                {
                    string startDateOffset = yieldPoint.fStartDate > 0 ? $"{yieldPoint.fStartDate}" : "";
                    cp.Tenor = $"{yieldPoint.fMaturity}{yieldPoint.fType}{startDateOffset}";
                    multiplier = 0.01d;
                }
                else
                {
                    string startDateOffset = yieldPoint.fStartDate > 0 ? $"+{yieldPoint.fStartDate}" : "";
                    cp.Tenor = $"{yieldPoint.fMaturity}{yieldPoint.fType}{startDateOffset}";
                }

                cp.PointType = yieldPoint.IsPointOfType(eMTypeSegment.M_etsFutureFRA) ? "FutureFRA" : yieldPoint.IsPointOfType(eMTypeSegment.M_etsMoneyMarket) ? "Money Market" : yieldPoint.IsPointOfType(eMTypeSegment.M_etsSwap) ? "Swap" : "Unknown";
                cp.Rate = yieldPoint.fYield * multiplier;
                cp.IsEnabled = yieldPoint.fInfoPtr.fIsUsed;
                cp.RateCode = yieldPoint.fInfoPtr.fRateCode.ToString();
            }
            
            return results;
        }

        private int GetPointCount(SSMYieldCurve curve)
        {
#if SOPHIS2021
            return curve.fPoints.fPointCount;
#else
            return curve.fPointCount;
#endif
        }

        private SSMYieldPoint GetPointList(SSMYieldCurve curve)
        {
#if SOPHIS2021
            return curve.fPoints.fPointList;
#else
            return curve.fPointList;
#endif
        }
    }
}
