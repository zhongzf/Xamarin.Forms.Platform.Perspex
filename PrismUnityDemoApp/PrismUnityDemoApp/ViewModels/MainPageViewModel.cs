using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PrismUnityDemoApp.ViewModels
{
    public class MainPageViewModel : BindableBase, INavigationAware
    {
        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        StudentBody studentBody;
        public StudentBody StudentBody
        {
            protected set { SetProperty(ref studentBody, value); }
            get { return studentBody; }
        }

        public MainPageViewModel()
        {
            Init(string.Empty);
        }

        public void Init(string fileName)
        {
            StudentBody = new StudentBody();
            StudentBody.Students.Add(new Student
            {
                FullName = "Adam Harmetz",
                FirstName = "Adam",
                PhotoFilename = "http://xamarin.github.io/xamarin-forms-book-samples/SchoolOfFineArt/AdamHarmetz.png"
            });
            StudentBody.Students.Add(new Student
            {
                FullName = "Alan Brewer",
                FirstName = "Alan",
                PhotoFilename = "http://xamarin.github.io/xamarin-forms-book-samples/SchoolOfFineArt/AlanBrewer.png"
            });
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("title"))
                Title = (string)parameters["title"] + " and Prism";
        }
    }
}
