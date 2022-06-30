using System.Collections.ObjectModel;
using System.ComponentModel;
using System;
using Octokit;
using Octokit.Internal;

namespace PrCollector
{
    public class PrListViewModel : INotifyPropertyChanged
    {
        #region Properties

       public  List<ProjectConfig> Projects = new List<ProjectConfig>();
        public DateTime PrFromTime = DateTime.Now;
        public ObservableCollection<PrModel> PRList { get; set; }
        #endregion

        #region Constructor

        public PrListViewModel()
        {
            
            Projects.Add(new ProjectConfig("filecoin-project", "venus"));
            Projects.Add(new ProjectConfig("filecoin-project", "venus-auth"));
            Projects.Add(new ProjectConfig("filecoin-project", "venus-messager"));
            Projects.Add(new ProjectConfig("filecoin-project", "venus-miner"));
            Projects.Add(new ProjectConfig("filecoin-project", "venus-wallet"));
            Projects.Add(new ProjectConfig("filecoin-project", "venus-docs"));
            Projects.Add(new ProjectConfig("filecoin-project", "venus-market"));
            Projects.Add(new ProjectConfig("ipfs-force-community", "venus-cluster"));
            Projects.Add(new ProjectConfig("ipfs-force-community", "venus-gateway"));
            Projects.Add(new ProjectConfig("ipfs-force-community", "chain-co"));
            Projects.Add(new ProjectConfig("ipfs-force-community", "go-fvm-sdk"));
            PRList = new ObservableCollection<PrModel>();
        }




        #endregion

        #region Interface Member

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        #endregion
    }
}