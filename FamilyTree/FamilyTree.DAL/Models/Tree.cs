﻿using System;
using System.Collections.Generic;

namespace FamilyTree.DAL.Models;

public partial class Tree
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<TreePerson> TreePeople { get; set; } = new List<TreePerson>();

    public virtual ICollection<UserTree> UserTrees { get; set; } = new List<UserTree>();
}