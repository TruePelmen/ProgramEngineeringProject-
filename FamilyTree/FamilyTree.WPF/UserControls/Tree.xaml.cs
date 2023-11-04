﻿// <copyright file="Tree.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FamilyTree.WPF.UserControls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Shapes;
    using FamilyTree.BLL;
    using FamilyTree.BLL.Interfaces;

    /// <summary>
    /// Interaction logic for Tree.xaml.
    /// </summary>
    public partial class Tree : UserControl
    {
        private readonly double halfWidth = 90;
        private int numberOfChildren;
        private double center;
        private int idFocusPerson;
        private int idFocusPersonSpounse;
        private IPersonService personService;
        private IRelationshipService relationshipService;

        /// <summary>
        /// Gets or sets the unique identifier of the focus person.
        /// This property is marked as obsolete and should be avoided.
        /// </summary>
        [System.Obsolete("This property is marked as obsolete and should be avoided.")]
        public int FocusPersonId
        {
            get
            {
                return this.idFocusPerson;
            }

            set
            {
                this.idFocusPerson = value;
                this.RedrawTree();
            }
        }

        /// <summary>
        /// Marks a person as empty based on the specified type.
        /// This method is marked as obsolete and should be avoided.
        /// </summary>
        /// <param name="type">The type of the person (e.g., 'male', 'female', etc.).</param>
        [System.Obsolete("This method is marked as obsolete and should be avoided.")]
        public void EmptyPerson(string type)
        {
            switch (type)
            {
                case "male":
                    this.maleFocus.IsEmpty = true;
                    break;
                case "female":
                    this.femaleFocus.IsEmpty = true;
                    break;
                case "maleFather":
                    this.maleFather.IsEmpty = true;
                    break;
                case "femaleFather":
                    this.femaleFather.IsEmpty = true;
                    break;
                case "maleMother":
                    this.maleMother.IsEmpty = true;
                    break;
                case "femaleMother":
                    this.femaleFather.IsEmpty = true;
                    break;
            }
        }

        /// <summary>
        /// Adds a child to the family tree with information from a <see cref="PersonCardInformation"/> object.
        /// This method is marked as obsolete and should be avoided.
        /// </summary>
        /// <param name="person">A <see cref="PersonCardInformation"/> object containing information about the child.</param>
        [System.Obsolete("This method is marked as obsolete and should be avoided.")]
        public void AddChild(PersonCardInformation person)
        {
            PersonCard child = new PersonCard();
            child.RenewPersonCard(person);
            child.Margin = new Thickness(20, 0, 20, 0);
            child.Width = 180;
            child.Height = 100;
            child.MouseLeftButtonDown += this.CardMouseLeftButtonDown;
            this.childrenPanel.Children.Add(child);
            this.numberOfChildren++;
        }

        private void ChildrenPanelLoaded(object sender, RoutedEventArgs e)
        {
            this.center = (this.childrenPanel.ActualWidth / 2) - 110;
            this.RedrawLines();
        }

        [System.Obsolete]
        private void RedrawTree()
        {
            this.childrenPanel.Children.Clear();
            var person = this.personService.GetFullInformarionAboutPerson(this.idFocusPerson);
            this.idFocusPersonSpounse = this.relationshipService.GetSpouseIdByPersonId(this.idFocusPerson);
            if (person.Person.Gender == "male")
            {
                this.maleFocus.RenewPersonCard(person);
                this.maleFather.IdPerson = this.relationshipService.GetFatherIdByPersonId(this.idFocusPerson);
                this.maleFather.RenewPersonCard(this.personService.GetShortInformationAboutPerson(this.maleFather.IdPerson));
                this.maleMother.IdPerson = this.relationshipService.GetMotherIdByPersonId(this.idFocusPerson);
                this.maleMother.RenewPersonCard(this.personService.GetShortInformationAboutPerson(this.maleMother.IdPerson));
                this.femaleFocus.RenewPersonCard(this.personService.GetFullInformarionAboutPerson(this.idFocusPersonSpounse));
                this.femaleFather.IdPerson = this.relationshipService.GetFatherIdByPersonId(this.idFocusPersonSpounse);
                this.femaleFather.RenewPersonCard(this.personService.GetShortInformationAboutPerson(this.femaleFather.IdPerson));
                this.femaleMother.IdPerson = this.relationshipService.GetMotherIdByPersonId(this.idFocusPersonSpounse);
                this.femaleMother.RenewPersonCard(this.personService.GetShortInformationAboutPerson(this.femaleMother.IdPerson));
            }
            else
            {
                this.femaleFocus.RenewPersonCard(person);
                this.femaleFather.IdPerson = this.relationshipService.GetFatherIdByPersonId(this.idFocusPerson);
                this.femaleFather.RenewPersonCard(this.personService.GetShortInformationAboutPerson(this.femaleFather.IdPerson));
                this.femaleMother.IdPerson = this.relationshipService.GetMotherIdByPersonId(this.idFocusPerson);
                this.femaleMother.RenewPersonCard(this.personService.GetShortInformationAboutPerson(this.femaleMother.IdPerson));
                this.maleFocus.RenewPersonCard(this.personService.GetFullInformarionAboutPerson(this.idFocusPersonSpounse));
                this.maleFather.IdPerson = this.relationshipService.GetFatherIdByPersonId(this.idFocusPersonSpounse);
                this.maleFather.RenewPersonCard(this.personService.GetShortInformationAboutPerson(this.maleFather.IdPerson));
                this.maleMother.IdPerson = this.relationshipService.GetMotherIdByPersonId(this.idFocusPersonSpounse);
                this.maleMother.RenewPersonCard(this.personService.GetShortInformationAboutPerson(this.maleMother.IdPerson));
            }

            var children = this.relationshipService.GetChildrenIdByPersonId(this.idFocusPerson);
            foreach (var child in children)
            {
                this.AddChild(this.personService.GetShortInformationAboutPerson(child));
            }

            this.AddChild(new PersonCardInformation());
        }

        private void RedrawLines()
        {
            this.childrenLines.Children.Clear();
            this.DrawVerticalLine(this.center, 0, 35);
            this.DrawMainHorizontalLine();
            if (this.numberOfChildren == 1)
            {
                this.DrawVerticalLine(this.center, 35, 56);
            }
            else
            {
                if (this.numberOfChildren % 2 != 0)
                {
                    this.DrawVerticalLine(this.center, 35, 56);
                    this.DrawOddNumberOfChildren();
                }
                else
                {
                    this.DrawEvenNumberOfChildren();
                }
            }
        }

        private void DrawOddNumberOfChildren()
        {
            double x = 0;
            for (int i = 0; i < this.numberOfChildren / 2; ++i)
            {
                x += 220;
                this.DrawVerticalLine(this.center - x, 35, 56);
                this.DrawVerticalLine(this.center + x, 35, 56);
            }
        }

        private void DrawEvenNumberOfChildren()
        {
            double x = this.halfWidth + 20;
            this.DrawVerticalLine(this.center - x, 35, 56);
            this.DrawVerticalLine(this.center + x, 35, 56);

            for (int i = 1; i < this.numberOfChildren / 2; ++i)
            {
                x += 220;
                this.DrawVerticalLine(this.center - x, 35, 56);
                this.DrawVerticalLine(this.center + x, 35, 56);
            }
        }

        private void DrawMainHorizontalLine()
        {
            double leftBorder = this.center - ((this.halfWidth + 20.0) * (this.numberOfChildren - 1));
            double rightBorder = this.center + ((this.halfWidth + 20.0) * (this.numberOfChildren - 1));
            this.childrenLines.Children.Add(new Line { X1 = leftBorder, Y1 = 35, X2 = rightBorder, Y2 = 35, Stroke = Brushes.Black, StrokeThickness = 1 });
        }

        private void DrawVerticalLine(double x, double y1, double y2)
        {
            this.childrenLines.Children.Add(new Line { X1 = x, Y1 = y1, X2 = x, Y2 = y2, Stroke = Brushes.Black, StrokeThickness = 1 });
        }

        [System.Obsolete]
        private void CardMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IPersonCard personCard = sender as IPersonCard;
            if (personCard.IdPerson != this.FocusPersonId || !personCard.IsEmpty)
            {
                this.FocusPersonId = personCard.IdPerson;
            }
        }
    }
}