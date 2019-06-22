﻿//  Copyright (c) RXD Solutions. All rights reserved.
//  FusionLink is licensed under the MIT license. See LICENSE.txt for details.

using System.Runtime.Serialization;

namespace RxdSolutions.FusionLink.Interface
{
    [DataContract]
    public class CurveFamilyNotFoundFaultContract
    {
        [DataMember]
        public string CurveFamily { get; set; }
    }
}