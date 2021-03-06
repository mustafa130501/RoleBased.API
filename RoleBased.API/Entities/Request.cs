using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RoleBased.API.Entities
{
    public class Request
    {
       
        [BsonId]
        // standard BSonId generated by MongoDb
        public ObjectId InternalId { get; set; }

        [DataMember]
        public string reqId
        {
            get { return InternalId.ToString(); }
            set { InternalId = ObjectId.Parse(value); }
        }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; }

        public DateTime DateTime { get; set; } = DateTime.Now;

        public string ResultRequest { get; set; }   
            
        public User User { get; set; }

        
    }
}
