﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace BBS.Domain
{
    public partial class TopicTag
    {
        public int Id { get; set; }
        public int TopicId { get; set; }
        public int TagId { get; set; }
        public DateTime CreateTime { get; set; }
    }
}