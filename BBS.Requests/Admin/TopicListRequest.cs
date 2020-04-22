using BBS.Framework.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace BBS.Requests.Admin
{
    public class TopicListRequest : BeginEndRequest, IHasCustomProperties<List<SelectListItem>>
    {
        public TopicListRequest()
        {
            CustomProperties = new Dictionary<string, List<SelectListItem>>();
        }
        public int AccountId { get; set; }
        public string Title { get; set; }
        public int CatalogId { get; set; }

        public IDictionary<string, List<SelectListItem>> CustomProperties { get; }
    }
}
