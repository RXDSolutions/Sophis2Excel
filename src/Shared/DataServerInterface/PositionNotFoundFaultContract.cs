﻿//  Copyright (c) RXD Solutions. All rights reserved.
using System.Runtime.Serialization;

namespace RxdSolutions.FusionLink.Interface
{
    [DataContract]
    public class PositionNotFoundFaultContract
    {
        [DataMember]
        public int PositionId { get; set; }
    }
}