﻿//  Copyright (c) RXD Solutions. All rights reserved.


using System.Runtime.Serialization;

namespace RxdSolutions.FusionLink.Interface
{
    [DataContract]
    public class PortfolioNotFoundFaultContract
    {
        [DataMember]
        public int PortfolioId { get; set; }
    }
}