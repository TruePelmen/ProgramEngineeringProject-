﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FamilyTree.DAL.Models;

namespace FamilyTree.DAL.Interfaces.Repositories
{
    public interface IMediaRepository : IGenericRepository<Media>
    {
        Media GetMainPhotoByPersonId(int personId);
    }
}
