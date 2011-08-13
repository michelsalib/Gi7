using System;
using System.Collections.ObjectModel;
using Gi7.Model;

namespace Gi7.Resources.DesignData
{
    public class CommitDataModel
    {
        public String RepoName { get; set; }

        public Push Commit { get; set; }

        public ObservableCollection<Comment> Comments { get; set; }

        public String CommitText
        {
            get
            {
                return String.Format("Showing {0} changed files with {1} additions and {2} deletions.", Commit.Files.Count, Commit.Stats.Additions, Commit.Stats.Deletions);
            }
        }

        public CommitDataModel()
        {
            RepoName = "michelsalib/Gi7";

            Commit = new Push()
            {
                Author = new User()
                {
                    Login = "michelsalib",
                    AvatarUrl = "https://secure.gravatar.com/avatar/5c4663b12b9e6d8dc6dcdfbfb3dc1317",
                },
                Commit = new Commit()
                {
                    Message = "-- general performance improvements\n  -- removed most of the sub view models\n  -- support late loading of the panorama/pivot items",
                    Author = new Committer()
                    {
                        Date = DateTime.Now,
                    }
                },
                Stats = new PushStats()
                {
                    Additions = 468,
                    Deletions = 352,
                    Total = 820
                },
                Files = new System.Collections.Generic.List<File>()
                {
                    new File(){
                        Status = "modified",
                        Patch = "--- a/Gi7/Views/UserView.xaml\n+++ b/Gi7/Views/UserView.xaml\n@@ -9,6 +9,8 @@\n     xmlns:d=\"http://schemas.microsoft.com/expression/blend/2008\"\n     xmlns:mc=\"http://schemas.openxmlformats.org/markup-compatibility/2006\" \n     xmlns:utils=\"clr-namespace:Gi7.Utils\"\n+    xmlns:Interactivity=\"clr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity\"\n+    xmlns:Command=\"clr-namespace:GalaSoft.MvvmLight.Command;assembly=GalaSoft.MvvmLight.Extras.WP71\"\n     mc:Ignorable=\"d\" d:DesignWidth=\"480\" d:DesignHeight=\"768\"\n     FontFamily=\"{StaticResource PhoneFontFamilyNormal}\"\n     FontSize=\"{StaticResource PhoneFontSizeNormal}\"\n@@ -21,7 +23,13 @@\n     <!--LayoutRoot is the root grid where all page content is placed-->\n     <Grid x:Name=\"LayoutRoot\" Background=\"Transparent\">\n         <!--Pivot Control-->\n-        <controls:Pivot Title=\"{Binding User}\">\n+        <controls:Pivot Title=\"{Binding Username}\">\n+            <Interactivity:Interaction.Triggers>\n+                <Interactivity:EventTrigger EventName=\"SelectionChanged\">\n+                    <Command:EventToCommand Command=\"{Binding PivotChangedCommand}\" PassEventArgsToCommand=\"True\" />\n+                </Interactivity:EventTrigger>\n+            </Interactivity:Interaction.Triggers>\n+            \n             <!--Pivot item one-->\n             <controls:PivotItem Header=\"Details\">\n                 <localControls:UserPanel />",
                        Additions = 9,
                        Filename = "Gi7/Views/UserView.xaml",
                        Deletions = 1,
                        Changes = 10
                    }
                }
            };

            Comments = new ObservableCollection<Comment>();
            Comments.Add(new Comment()
            {
                Body = "My comment is nice !",
                User = new User()
                {
                    Login = "michelsalib",
                    AvatarUrl = "https://secure.gravatar.com/avatar/5c4663b12b9e6d8dc6dcdfbfb3dc1317",
                },
                UpdatedAt = DateTime.Now,
            });
        }
    }
}
