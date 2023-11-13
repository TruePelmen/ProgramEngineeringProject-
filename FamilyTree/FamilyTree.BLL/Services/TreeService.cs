﻿namespace FamilyTree.BLL.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FamilyTree.BLL.Interfaces;
    using FamilyTree.DAL.Interfaces.Repositories;
    using FamilyTree.DAL.Models;

    public class TreeService : ITreeService
    {
        private IGenericRepository<Tree> treeRepository;
        private ITreePersonService treePersonService;

        public TreeService(IGenericRepository<Tree> treeRepository, ITreePersonService treePersonService)
        {
            this.treeRepository = treeRepository;
            this.treePersonService = treePersonService;
        }

        public IEnumerable<Tree> GetAllTrees()
        {
            return this.treeRepository.GetAll();
        }

        public Tree GetTreeById(int id)
        {
            return this.treeRepository.GetById(id);
        }

        public int GetPrimaryPersonId(int treeId)
        {
            var primaryPerson = this.treePersonService.GetTreePeopleByTreeId(treeId).FirstOrDefault(person => person.PrimaryPerson);
            if (primaryPerson != null)
            {
                return primaryPerson.Id;
            }

            return -1;
        }

        public int AddTree(string name)
        {
            Tree tree = new Tree
            {
                Name = name,
            };

            this.treeRepository.Add(tree);
            this.treeRepository.Save();
            return tree.Id;
        }

        public void UpdateTree(int id, string name)
        {
            Tree tree = this.treeRepository.GetById(id);

            if (tree != null)
            {
                tree.Name = name;

                this.treeRepository.Update(tree);
                this.treeRepository.Save();
            }
        }

        public void DeleteTree(int id)
        {
            this.treeRepository.Remove(this.GetTreeById(id));
            this.treeRepository.Save();
        }
    }
}