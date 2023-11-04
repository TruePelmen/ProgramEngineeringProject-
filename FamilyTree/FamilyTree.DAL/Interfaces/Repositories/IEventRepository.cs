﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FamilyTree.DAL.Models;

namespace FamilyTree.DAL.Interfaces.Repositories
{
    public interface IEventRepository : IGenericRepository<Event>
    {
        public IEnumerable<Event> GetEventsByPersonId(int personId);
        public IEnumerable<Event> GetEventsByPersonIdAndEventType(int personId, string eventType);
    }
}
